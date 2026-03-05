using System;
using UnityEngine;

/// <summary>
/// A rules for adding cards of various values from a specific suit.
/// </summary>
[Serializable]
public class DeckSuitData
{
    /// <summary>
    /// What suit these cards should be.
    /// </summary>
    [SerializeField] private CardSuit suit;

    /// <summary>
    /// The values of cards to add.
    /// </summary>
    [SerializeField] private bool[] values;

    /// <summary>
    /// What suit these cards should be.
    /// </summary>
    public CardSuit Suit { get => suit; }

    /// <summary>
    /// The values of cards to add.
    /// </summary>
    public bool[] Values { get => values; }

}
