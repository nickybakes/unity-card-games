using System;
using UnityEngine;

/// <summary>
/// A row of a Poker Paytable.
/// </summary>
[Serializable]
public class PaytableRowPoker
{
    /// <summary>
    /// The Poker Hand of this row.
    /// </summary>
    [SerializeField] private PokerHand hand;

    /// <summary>
    /// The bet multiplier of this row.
    /// </summary>
    [SerializeField] private float betMultiplier;

    /// <summary>
    /// The Poker Hand of this row.
    /// </summary>
    public PokerHand Hand { get => hand; }

    /// <summary>
    /// The bet multiplier of this row.
    /// </summary>
    public float BetMultiplier { get => betMultiplier; }

}
