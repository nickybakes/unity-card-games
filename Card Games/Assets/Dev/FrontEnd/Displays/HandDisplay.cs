using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

public class HandDisplay : Display
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private TextAlignment alignment = TextAlignment.Center;

    [SerializeField][Range(0.0f, 1.0f)] private float cardSizeNormalized;
    [SerializeField][Range(0.0f, 1.0f)] private float bigCardSizeNormalized;

    [SerializeField] private float rotationFirst;
    [SerializeField] private float rotationLast;

    [SerializeField] private float cardSpaceMoveSpeed = 4f;
    [SerializeField] private float cardSpaceGrowSpeed = 4f;

    [SerializeField] private bool cardsInheritRotation = true;

    [SerializeField] private bool cardsInheritScale = true;


    private List<HandCardSpace> handCardSpaces;

    private int numCardDisplaysInHand;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SetupRectTransform();
        handCardSpaces = new List<HandCardSpace>(5);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            handCardSpaces[i].Update(cardSpaceGrowSpeed, cardSpaceMoveSpeed);
        }

        UpdateCardTransforms();
    }

    public void AddCardDisplayToHand(CardDisplay display, int indexPosition = -1)
    {
        if (indexPosition == -1)
            indexPosition = numCardDisplaysInHand;

        numCardDisplaysInHand++;

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

        newSpace.cardDisplay.transform.SetAsLastSibling();

        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            if (i != newSpaceIndexInArray && !handCardSpaces[i].displayRemoved && handCardSpaces[i].IncrementGoalPositionIfGreaterThanIndex(indexPosition - 1))
                handCardSpaces[i].cardDisplay.transform.SetAsLastSibling();
        }
    }

    public void UpdateCardTransforms()
    {


        float totalSpaceSize = 0;
        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            HandCardSpace currentSpace = handCardSpaces[i];
            totalSpaceSize += currentSpace.SpaceSize;
        }

        float scale = Math.Min(1f / (cardSizeNormalized * totalSpaceSize), 1f);
        float scaledCardSizeNormalized = cardSizeNormalized * scale;
        float scaledBigCardSizeNormalized = bigCardSizeNormalized * scale;

        // Calculate offsets
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

        // Calculate starting point
        float startPoint = 0;
        if (alignment == TextAlignment.Center)
        {
            startPoint = (scaledCardSizeNormalized * .5f) + .5f - (totalSpaceSize * .5f * scaledCardSizeNormalized);
        }
        else if (alignment == TextAlignment.Right)
        {
            startPoint = 1.0f - ((totalSpaceSize - 1.0f) * scaledCardSizeNormalized);
        }


        // Combine starting point and offset to position the cards
        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            HandCardSpace currentSpace = handCardSpaces[i];
            if (currentSpace.displayRemoved)
                continue;

            float point = startPoint + (currentSpace.SpaceIndexPosition * scaledCardSizeNormalized) + currentSpace.positionOffset;

            Vector3 position = splineContainer.Spline.EvaluatePosition(point);
            position.z = 0;
            currentSpace.cardDisplay.SetGoalTransform(position, Quaternion.AngleAxis(Mathf.Lerp(rotationFirst, rotationLast, point), Vector3.forward), Vector3.one);
            currentSpace.cardDisplay.ApplyTransformParentToGoalTransform(rectTransform, cardsInheritRotation, cardsInheritScale);
        }
    }

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

    private float GetCardOffsetSizeNormalized(int index, float scaledCardSizeNormalized, float scaledBigCardSizeNormalized)
    {
        float sizeTransition = 0;
        if (!handCardSpaces[index].displayRemoved)
            sizeTransition = handCardSpaces[index].cardDisplay.SizeTransition;
        return (scaledBigCardSizeNormalized - scaledCardSizeNormalized) * sizeTransition;
    }
}


