using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Controls the transforms of Card Displays in a hand.
/// </summary>
public class HandDisplay : Display
{
    /// <summary>
    /// The spline track for cards to be on.
    /// </summary>
    [SerializeField] private SplineContainer splineContainer;

    /// <summary>
    /// How to align the cards along the hand.
    /// </summary>
    [SerializeField] private TextAlignment alignment = TextAlignment.Center;

    /// <summary>
    /// The normalized size of regular cards in the hand.
    /// </summary>
    [SerializeField][Range(0.0f, 1.0f)] private float cardSizeNormalized;

    /// <summary>
    /// The normalized size of big (selected) cards in the hand.
    /// </summary>
    [SerializeField][Range(0.0f, 1.0f)] private float bigCardSizeNormalized;

    /// <summary>
    /// The first bound of the lerped rotation to apply to cards in the hand.
    /// </summary>
    [SerializeField] private Vector3 rotationFirst;

    /// <summary>
    /// The last bound of the lerped rotation to apply to cards in the hand.
    /// </summary>
    [SerializeField] private Vector3 rotationLast;

    /// <summary>
    /// The move speed of floating card spaces.
    /// </summary>
    [SerializeField] private float cardSpaceMoveSpeed = 4f;

    /// <summary>
    /// The grow speed of floating card spaces.
    /// </summary>
    [SerializeField] private float cardSpaceGrowSpeed = 4f;

    /// <summary>
    /// Whether cards in this hand should inherit the rotation of the hand display and its parents.
    /// </summary>
    [SerializeField] private bool cardsInheritRotation = true;

    /// <summary>
    /// Whether cards in this hand should inherit the scale of the hand display and its parents.
    /// </summary>
    [SerializeField] private bool cardsInheritScale = true;

    /// <summary>
    /// The list of floating hand spaces in this hand display.
    /// </summary>
    private List<HandCardSpace> handCardSpaces;

    /// <summary>
    /// The number of cards in this hand.
    /// </summary>
    private int numCardDisplaysInHand;

    /// <summary>
    /// Initialize the hand card spaces list.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
        handCardSpaces = new List<HandCardSpace>(5);
    }

    /// <summary>
    /// Update the hand card spaces and the transforms of the card displays in this hand.
    /// </summary>
    void Update()
    {
        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            handCardSpaces[i].Update(cardSpaceGrowSpeed, cardSpaceMoveSpeed);
        }

        UpdateCardTransforms();
    }

    /// <summary>
    /// Add a card display to this hand and reposition card displays in this hand if needed.
    /// </summary>
    /// <param name="display">The card display to add.</param>
    /// <param name="indexPosition">Optional specified index of where in this hand the card should be at.</param>
    public void AddCardDisplayToHand(CardDisplay display, int indexPosition = -1)
    {
        // If not specified index position, put card at the end.
        if (indexPosition == -1)
            indexPosition = numCardDisplaysInHand;

        numCardDisplaysInHand++;

        // Try to find a Hand Card Space thats not being used, if not create a new one.
        HandCardSpace newSpace = null;
        int newSpaceIndexInArray = -1;
        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            if (handCardSpaces[i].displayRemoved)
            {
                newSpace = handCardSpaces[i];
                newSpaceIndexInArray = i;
                handCardSpaces[i].Setup(display, indexPosition);
                break;
            }
        }
        if (newSpace == null)
        {
            newSpace = new HandCardSpace(display, indexPosition);
            handCardSpaces.Add(newSpace);
            newSpaceIndexInArray = handCardSpaces.Count - 1;
        }

        // Adjust the order in the hierarchy so cards are stacked on top of eachother.
        newSpace.cardDisplay.transform.SetAsLastSibling();

        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            if (i != newSpaceIndexInArray && !handCardSpaces[i].displayRemoved && handCardSpaces[i].IncrementGoalPositionIfGreaterThanIndex(indexPosition - 1))
                handCardSpaces[i].cardDisplay.transform.SetAsLastSibling();
        }
    }

    /// <summary>
    /// Calculate transforms for cards in the hand.
    /// </summary>
    public void UpdateCardTransforms()
    {
        // Calculate overall space and normalized scale of cards in the display.
        float totalSpaceSize = 0;
        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            HandCardSpace currentSpace = handCardSpaces[i];
            totalSpaceSize += currentSpace.SpaceSize;
        }

        float scale = Math.Min(1f / (cardSizeNormalized * totalSpaceSize), 1f);
        float scaledCardSizeNormalized = cardSizeNormalized * scale;
        float scaledBigCardSizeNormalized = bigCardSizeNormalized * scale;

        // Calculate offsets of cards based on their sizes.
        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            HandCardSpace currentSpace = handCardSpaces[i];
            currentSpace.positionOffset = 0;

            for (int j = 0; j < handCardSpaces.Count; j++)
            {
                HandCardSpace compareSpace = handCardSpaces[j];
                float offset = GetCardOffsetSizeNormalized(j, scaledCardSizeNormalized, scaledBigCardSizeNormalized) * compareSpace.SpaceSize * .5f;
                offset *= Mathf.Clamp(currentSpace.SpaceIndexPosition - compareSpace.SpaceIndexPosition, -1, 1);
                currentSpace.positionOffset += offset;
            }
        }

        // Calculate starting point for positioning cards from.
        float startPoint = 0;
        if (alignment == TextAlignment.Center)
        {
            startPoint = (scaledCardSizeNormalized * .5f) + .5f - (totalSpaceSize * .5f * scaledCardSizeNormalized);
        }
        else if (alignment == TextAlignment.Right)
        {
            startPoint = 1.0f - ((totalSpaceSize - 1.0f) * scaledCardSizeNormalized);
        }


        // Combine starting point and offset to position the cards.
        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            HandCardSpace currentSpace = handCardSpaces[i];
            if (currentSpace.displayRemoved)
                continue;

            float point = startPoint + (currentSpace.SpaceIndexPosition * scaledCardSizeNormalized) + currentSpace.positionOffset;

            Vector3 position = splineContainer.Spline.EvaluatePosition(point);
            position.z = 0;
            currentSpace.cardDisplay.SetGoalTransform(position, Quaternion.Euler(Vector3.Lerp(rotationFirst, rotationLast, point)), Vector3.one);
            currentSpace.cardDisplay.ApplyTransformParentToGoalTransform(rectTransform, cardsInheritRotation, cardsInheritScale);
        }
    }

    /// <summary>
    /// Remove a Card Display from this hand and reposition the rest of the cards.
    /// </summary>
    /// <param name="display"></param>
    public void RemoveCardDisplayFromHand(CardDisplay display)
    {
        int removeIndex = numCardDisplaysInHand;

        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            if (handCardSpaces[i].cardDisplay == display && !handCardSpaces[i].displayRemoved)
            {
                handCardSpaces[i].displayRemoved = true;
                handCardSpaces[i].SetGoalSize(0);
                numCardDisplaysInHand--;
                removeIndex = (int)handCardSpaces[i].SpaceIndexPositionGoal;
                break;
            }
        }

        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            handCardSpaces[i].DecrementGoalPositionIfGreaterThanIndex(removeIndex);
        }
    }

    /// <summary>
    /// Get the normalized card size by lerping its transition amount and the card space size.
    /// </summary>
    /// <param name="index">The index of the card display.</param>
    /// <param name="scaledCardSizeNormalized">The scale normalized size of a regular card.</param>
    /// <param name="scaledBigCardSizeNormalized">The scale normalized size of a big (selected) card.</param>
    /// <returns>The calculated normalized size of a card display.</returns>
    private float GetCardOffsetSizeNormalized(int index, float scaledCardSizeNormalized, float scaledBigCardSizeNormalized)
    {
        float sizeTransition = 0;
        if (!handCardSpaces[index].displayRemoved)
            sizeTransition = handCardSpaces[index].cardDisplay.SizeTransition;
        return (scaledBigCardSizeNormalized - scaledCardSizeNormalized) * sizeTransition;
    }
}


