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

    [SerializeField] private float cardShiftTransitionTime = .2f;

    [SerializeField] private bool applyScaleToCards = true;
    [SerializeField] private bool applyRotationToCards = true;


    private List<HandCardSpace> handCardSpaces;

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
            handCardSpaces[i].Update();
        }

        UpdateCardTransforms();
    }

    public void AddCardDisplay(CardDisplay display)
    {
        handCardSpaces.Add(new HandCardSpace(display, handCardSpaces.Count, cardShiftTransitionTime));
    }

    public void UpdateCardTransforms()
    {
        // Calculate offsets
        float totalSpaceSize = 0;
        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            HandCardSpace currentSpace = handCardSpaces[i];
            currentSpace.positionOffset = 0;
            totalSpaceSize += currentSpace.SpaceSize;
            for (int j = 0; j < handCardSpaces.Count; j++)
            {
                HandCardSpace compareSpace = handCardSpaces[j];
                float offset = GetCardOffsetSizeNormalized(j) * compareSpace.SpaceSize * .5f;
                offset *= Mathf.Clamp(currentSpace.SpaceIndexPosition - compareSpace.SpaceIndexPosition, -1, 1);
                currentSpace.positionOffset += offset;
            }
        }

        // Calculate starting point
        float startPoint = 0;
        if (alignment == TextAlignment.Center)
        {
            startPoint = (cardSizeNormalized * .5f) + .5f - (totalSpaceSize * .5f * cardSizeNormalized);
        }
        else if (alignment == TextAlignment.Right)
        {
            startPoint = 1.0f - ((totalSpaceSize - 1.0f) * cardSizeNormalized);
        }


        // Combine starting point and offset to position the cards
        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            HandCardSpace currentSpace = handCardSpaces[i];
            float point = startPoint + (currentSpace.SpaceIndexPosition * cardSizeNormalized) + currentSpace.positionOffset;

            Vector3 position = splineContainer.Spline.EvaluatePosition(point);
            position.z = 0;
            currentSpace.CardDisplay.SetGoalTransform(position, Quaternion.AngleAxis(Mathf.Lerp(rotationFirst, rotationLast, point), Vector3.forward), Vector3.one);
            currentSpace.CardDisplay.ApplyTransformParentToGoalTransform(rectTransform, applyScaleToCards, applyRotationToCards);
        }
    }

    public void RemoveCardFromHand(int index)
    {
        handCardSpaces.RemoveAt(index);

        for (int i = index; i < handCardSpaces.Count; i++)
        {
            handCardSpaces[i].SetGoalPosition(i - 1);
        }
    }

    private float GetCardOffsetSizeNormalized(int index)
    {
        return (bigCardSizeNormalized - cardSizeNormalized) * handCardSpaces[index].CardDisplay.SizeTransition;
    }
}


