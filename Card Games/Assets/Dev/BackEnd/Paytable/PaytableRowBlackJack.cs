using System;
using UnityEngine;

/// <summary>
/// A row of a Blackjack Paytable.
/// </summary>
[Serializable]
public class PaytableRowBlackJack
{
    /// <summary>
    /// The name of this row.
    /// </summary>
    [SerializeField] private string name;

    /// <summary>
    /// The bet multiplier of this row.
    /// </summary>
    [SerializeField] private float betMultiplier;

    /// <summary>
    /// Whether this row is a win or not.
    /// </summary>
    [SerializeField] private bool isAWin;

    /// <summary>
    /// The name of this row.
    /// </summary>
    public string Name { get => name; }

    /// <summary>
    /// The bet multiplier of this row.
    /// </summary>
    public float BetMultiplier { get => betMultiplier; }

    /// <summary>
    /// Whether this row is a win or not.
    /// </summary>
    public bool IsAWin { get => isAWin; }

}
