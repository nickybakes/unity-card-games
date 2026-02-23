using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

public class HandCardSpace
{
    public float positionOffset;

    private CardDisplay cardDisplay;

    private float spaceIndexPosition = 0;

    private float spaceIndexPositionGoal = 0;

    private float spaceSize = 0f;

    private float transitionTimeLength = .2f;

    private AnimationCurve transitionCurve;
    private float currentMoveTime = 0;

    private float currentGrowTime = 0;

    public CardDisplay CardDisplay { get => cardDisplay; }

    public float SpaceIndexPosition { get => spaceIndexPosition; }

    public float SpaceSize { get => spaceSize; }

    public HandCardSpace(CardDisplay _cardDisplay, float _spaceIndexPosition, float _transitionTimeLength, AnimationCurve _transitionCurve)
    {
        cardDisplay = _cardDisplay;
        transitionTimeLength = _transitionTimeLength;
        transitionCurve = _transitionCurve;
        spaceIndexPosition = _spaceIndexPosition;
        currentMoveTime = transitionTimeLength + 1;
        currentGrowTime = 0;
    }

    public void Update()
    {
        if (currentGrowTime <= transitionTimeLength)
        {
            currentGrowTime += Time.deltaTime;
            spaceSize = transitionCurve.Evaluate(currentGrowTime / transitionTimeLength);
        }

        if (currentMoveTime <= transitionTimeLength)
        {
            currentMoveTime += Time.deltaTime;
            spaceIndexPosition = Mathf.LerpUnclamped(spaceIndexPosition, spaceIndexPositionGoal, transitionCurve.Evaluate(currentMoveTime / transitionTimeLength));
        }
    }

    public void SetGoalPosition(float newPosition)
    {
        spaceIndexPositionGoal = newPosition;
        currentMoveTime = 0;
    }
}


