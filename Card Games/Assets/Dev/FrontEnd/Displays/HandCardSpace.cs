using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

public class HandCardSpace
{
    public float positionOffset;
    public CardDisplay cardDisplay;
    public bool displayRemoved;

    public float SpaceIndexPosition { get => spaceIndexPosition; }
    public float SpaceIndexPositionGoal { get => spaceIndexPositionGoal; }
    public float SpaceSize { get => spaceSize; }

    private float spaceIndexPosition;
    private float spaceIndexPositionGoal;
    private float spaceSize;
    private float spaceSizeGoal;

    private float moveDirection;
    private float growDirection;

    public HandCardSpace(CardDisplay _cardDisplay, float _spaceIndexPosition)
    {
        Setup(_cardDisplay, _spaceIndexPosition);
    }

    public void Setup(CardDisplay _cardDisplay, float _spaceIndexPosition)
    {
        cardDisplay = _cardDisplay;
        SetGoalSize(1);
        displayRemoved = false;
        spaceIndexPosition = _spaceIndexPosition;
        spaceIndexPositionGoal = _spaceIndexPosition;
        moveDirection = 0;
    }

    public void Update(float growSpeed, float moveSpeed)
    {
        if (growDirection != 0)
        {
            spaceSize += growDirection * growSpeed * Time.deltaTime;

            if ((growDirection > 0 && spaceSize >= spaceSizeGoal) || (growDirection < 0 && spaceSize <= spaceSizeGoal))
            {
                spaceSize = spaceSizeGoal;
                growDirection = 0;
            }
        }

        if (moveDirection != 0)
        {
            spaceIndexPosition += moveDirection * moveSpeed * Time.deltaTime;

            if ((moveDirection > 0 && spaceIndexPosition >= spaceIndexPositionGoal) || (moveDirection < 0 && spaceIndexPosition <= spaceIndexPositionGoal))
            {
                spaceIndexPosition = spaceIndexPositionGoal;
                moveDirection = 0;
            }
        }

    }

    public bool DecrementGoalPositionIfGreaterThanIndex(int index)
    {
        bool passed = spaceIndexPositionGoal > index;

        if (passed)
            SetGoalPosition(spaceIndexPositionGoal - 1);

        return passed;
    }

    public bool IncrementGoalPositionIfGreaterThanIndex(int index)
    {
        bool passed = spaceIndexPositionGoal > index;

        if (passed)
            SetGoalPosition(spaceIndexPositionGoal + 1);

        return passed;
    }

    public void SetGoalPosition(float newPosition)
    {
        spaceIndexPositionGoal = newPosition;
        if (spaceIndexPosition != spaceIndexPositionGoal)
            moveDirection = Math.Sign(spaceIndexPositionGoal - spaceIndexPosition);
    }

    public void SetGoalSize(float newSize)
    {
        spaceSizeGoal = newSize;
        if (spaceSize != spaceSizeGoal)
            growDirection = Math.Sign(spaceSizeGoal - spaceSize);
    }
}


