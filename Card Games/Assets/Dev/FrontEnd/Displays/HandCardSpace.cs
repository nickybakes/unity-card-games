using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// A floating position and size within a Hand Display.
/// </summary>
public class HandCardSpace
{
    /// <summary>
    /// Current extra offset to add to the position.
    /// </summary>
    public float positionOffset;

    /// <summary>
    /// The card display that is tied to this position.
    /// </summary>
    public CardDisplay cardDisplay;

    /// <summary>
    /// If the card display has been removed from the hand/this space.
    /// </summary>
    public bool displayRemoved;

    /// <summary>
    /// The current floating index in the hand that this space is at.
    /// </summary>
    public float SpaceIndexPosition { get => spaceIndexPosition; }

    /// <summary>
    /// The index in the hand that this space is trying to be at.
    /// </summary>
    public float SpaceIndexPositionGoal { get => spaceIndexPositionGoal; }

    /// <summary>
    /// The normalized size of this space, from 0 to 1.
    /// </summary>
    public float SpaceSize { get => spaceSize; }

    /// <summary>
    /// The current floating index in the hand that this space is at.
    /// </summary>
    private float spaceIndexPosition;

    /// <summary>
    /// The index in the hand that this space is trying to be at.
    /// </summary>
    private float spaceIndexPositionGoal;

    /// <summary>
    /// The normalized size of this space, from 0 to 1.
    /// </summary>
    private float spaceSize;

    /// <summary>
    /// The normalized size of this space, from 0 to 1, this this space is growing/shrinking towards.
    /// </summary>
    private float spaceSizeGoal;

    /// <summary>
    /// The direction that this space's index position is moving towards, if at all.
    /// </summary>
    private float moveDirection;

    /// <summary>
    /// Whether this space's size is growing, shrinking, or not doing either.
    /// </summary>
    private float growDirection;

    /// <summary>
    /// Contructor that stores the card display and sets up the space.
    /// </summary>
    /// <param name="_cardDisplay">The card display that is tied to this position.</param>
    /// <param name="_spaceIndexPosition">The index in the hand that this space is trying to be at.</param>
    public HandCardSpace(CardDisplay _cardDisplay, float _spaceIndexPosition)
    {
        Setup(_cardDisplay, _spaceIndexPosition);
    }

    /// <summary>
    /// Sets the initial goals of this hand card space.
    /// </summary>
    /// <param name="_cardDisplay">The card display that is tied to this position.</param>
    /// <param name="_spaceIndexPosition">The index in the hand that this space is trying to be at.</param>
    public void Setup(CardDisplay _cardDisplay, float _spaceIndexPosition)
    {
        cardDisplay = _cardDisplay;
        SetGoalSize(1);
        displayRemoved = false;
        spaceIndexPosition = _spaceIndexPosition;
        spaceIndexPositionGoal = _spaceIndexPosition;
        moveDirection = 0;
    }

    /// <summary>
    /// Update the position and size of this space.
    /// </summary>
    /// <param name="growSpeed">The speed that this space should grow at.</param>
    /// <param name="moveSpeed">The speed that this space should move at.</param>
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

    /// <summary>
    /// If the goal position is greater than a specified index, decrement it.
    /// </summary>
    /// <param name="index">The index threshold.</param>
    /// <returns>Whether the condition was passed.</returns>
    public bool DecrementGoalPositionIfGreaterThanIndex(int index)
    {
        bool passed = spaceIndexPositionGoal > index;

        if (passed)
            SetGoalPosition(spaceIndexPositionGoal - 1);

        return passed;
    }

    /// <summary>
    /// If the goal position is greater than a specified index, increment it.
    /// </summary>
    /// <param name="index">The index threshold.</param>
    /// <returns>Whether the condition was passed.</returns>
    public bool IncrementGoalPositionIfGreaterThanIndex(int index)
    {
        bool passed = spaceIndexPositionGoal > index;

        if (passed)
            SetGoalPosition(spaceIndexPositionGoal + 1);

        return passed;
    }

    /// <summary>
    /// Sets the goal index position and makes the space move towards that.
    /// </summary>
    /// <param name="newPosition">The new goal index position</param>
    public void SetGoalPosition(float newPosition)
    {
        spaceIndexPositionGoal = newPosition;
        if (spaceIndexPosition != spaceIndexPositionGoal)
            moveDirection = Math.Sign(spaceIndexPositionGoal - spaceIndexPosition);
    }

    /// <summary>
    /// Sets the goal size and makes the space grow/shrink towards that.
    /// </summary>
    /// <param name="newPosition">The new goal size</param>
    public void SetGoalSize(float newSize)
    {
        spaceSizeGoal = newSize;
        if (spaceSize != spaceSizeGoal)
            growDirection = Math.Sign(spaceSizeGoal - spaceSize);
    }
}


