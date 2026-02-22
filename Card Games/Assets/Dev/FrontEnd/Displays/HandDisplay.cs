using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class HandDisplay : Display
{
    [SerializeField] private SplineContainer splineContainer;

    [SerializeField][Range(0.0f, 1.0f)] private float cardSizeNormalized;
    [SerializeField][Range(0.0f, 1.0f)] private float bigCardSizeNormalized;

    [SerializeField] private float rotationFirst;
    [SerializeField] private float rotationLast;

    [SerializeField] private float cardShiftTransitionTime = .2f;


    private List<CardDisplay> cardDisplaysInHand;

    private List<HandCardSpace> handCardSpaces;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SetupRectTransform();
        cardDisplaysInHand = new List<CardDisplay>(5);
        handCardSpaces = new List<HandCardSpace>(5);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            handCardSpaces[i].Update();
        }

        UpdateCardPositions();
    }

    public void AddCardDisplay(CardDisplay display)
    {
        cardDisplaysInHand.Add(display);
        handCardSpaces.Add(new HandCardSpace(cardDisplaysInHand.Count - 1, cardShiftTransitionTime));
    }

    public void UpdateCardPositions()
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
                float offset = GetCardOffsetSizeNormalized(i) * compareSpace.SpaceSize * .5f;
                offset *= Mathf.Clamp(currentSpace.SpaceIndexPosition - compareSpace.SpaceIndexPosition, -1, 1);
                currentSpace.positionOffset += offset;
            }
        }

        // Calculate starting point
        int halfCount = handCardSpaces.Count / 2;
        float evenOffset = 0;
        if (handCardSpaces.Count % 2 == 0)
        {
            evenOffset = cardSizeNormalized * .5f;
        }
        float startPoint = evenOffset + .5f - (halfCount * cardSizeNormalized);

        // Combine starting point and offset to position the cards
        for (int i = 0; i < handCardSpaces.Count; i++)
        {
            HandCardSpace currentSpace = handCardSpaces[i];
            float normalizedPosition = 0;
            if (totalSpaceSize != 0)
            {
                normalizedPosition = currentSpace.SpaceIndexPosition / totalSpaceSize;
            }
            Debug.Log(i + ", " + normalizedPosition);
            float point = startPoint + (normalizedPosition * cardSizeNormalized) + currentSpace.positionOffset;

            Vector3 position = splineContainer.Spline.EvaluatePosition(point);
            position.z = 0;
            if (i < cardDisplaysInHand.Count)
            {
                CardDisplay currentDisplay = cardDisplaysInHand[i];
                currentDisplay.SetGoalTransform(position, Quaternion.AngleAxis(Mathf.Lerp(rotationFirst, rotationLast, point), Vector3.forward), rectTransform.localScale);
            }
        }
    }

    public void RemoveCardFromHand(int index)
    {
        cardDisplaysInHand.RemoveAt(index);
        handCardSpaces.RemoveAt(index);

        for (int i = index; i < handCardSpaces.Count; i++)
        {
            handCardSpaces[i].SetGoalPosition(i - 1);
        }
    }

    private float GetCardOffsetSizeNormalized(int index)
    {
        return (bigCardSizeNormalized - cardSizeNormalized) * cardDisplaysInHand[index].SizeTransition;
    }

    private class HandCardSpace
    {
        public float positionOffset;

        float spaceIndexPosition = 0;

        float spaceIndexPositionGoal = 0;

        float spaceSize = 0f;

        float transitionTimeLength = .2f;
        float currentMoveTime = 0;

        float currentGrowTime = 0;

        public float SpaceIndexPosition { get => spaceIndexPosition; }

        public float SpaceSize { get => spaceSize; }

        public HandCardSpace(float _spaceIndexPosition, float _transitionTimeLength)
        {
            transitionTimeLength = _transitionTimeLength;
            spaceIndexPosition = _spaceIndexPosition;
            currentMoveTime = transitionTimeLength + 1;
            currentGrowTime = 0;
        }

        public void Update()
        {
            if (currentGrowTime <= transitionTimeLength)
            {
                currentGrowTime += Time.deltaTime;
                spaceSize = Mathf.Clamp01(currentGrowTime / transitionTimeLength);
            }

            if (currentMoveTime <= transitionTimeLength)
            {
                currentMoveTime += Time.deltaTime;
                spaceIndexPosition = Mathf.Lerp(spaceIndexPosition, spaceIndexPositionGoal, currentMoveTime / transitionTimeLength);
            }
        }

        public void SetGoalPosition(float newPosition)
        {
            spaceIndexPositionGoal = newPosition;
            currentMoveTime = 0;
        }
    }
}


