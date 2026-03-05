using System;
using UnityEngine;

/// <summary>
/// A special rule for adding specific types of cards.
/// </summary>
[Serializable]
public class DeckSpecialData
{
    /// <summary>
    /// What type of Special rule this is.
    /// </summary>
    [SerializeField] private SpecialDeckDataType type;

    /// <summary>
    /// The number of cards to add following this rule.
    /// </summary>
    [SerializeField] private int amount;

    /// <summary>
    /// What type of Special rule this is.
    /// </summary>
    public SpecialDeckDataType Type { get => type; }

    /// <summary>
    /// The number of cards to add following this rule.
    /// </summary>
    public int Amount { get => amount; }

}

/// <summary>
/// What type of Special rule this is. Can be expanded upon.
/// </summary>
public enum SpecialDeckDataType
{
    RandomBasicCards,
    Bomb
}
