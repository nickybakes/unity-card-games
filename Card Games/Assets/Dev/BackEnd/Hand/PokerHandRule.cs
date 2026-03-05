using System;
using UnityEngine;

/// <summary>
/// An individual rule/condition of a Poker Hand that must be met for that Poker Hand to be found in a set of cards.
/// </summary>
[Serializable]
public class PokerHandRule
{
    /// <summary>
    /// The type of condition to check for.
    /// </summary>
    [SerializeField] private HandComponent type;

    /// <summary>
    /// The number of cards that need to pass this condition.
    /// </summary>
    [SerializeField] private int amountNeeded;

    /// <summary>
    /// Whether to stop checking cards if the amount needed is met.
    /// </summary>
    [SerializeField] private bool stopCountingAtAmount;

    /// <summary>
    /// The operation to do after this rule is checked. 
    /// For example, trimming the scoring cards means they wont be counted again for later rules.
    /// </summary>
    [SerializeField] private HandAfterRuleOperation afterRuleOperation;

    /// <summary>
    /// The Card Values allowed to pass this rule.
    /// </summary>
    [SerializeField] private bool[] allowedValues;

    /// <summary>
    /// The Card Suits allowed to pass this rule.
    /// </summary>
    [SerializeField] private bool[] allowedSuits;

    /// <summary>
    /// The type of condition to check for.
    /// </summary>
    public HandComponent Type { get => type; }

    /// <summary>
    /// The number of cards that need to pass this condition.
    /// </summary>
    public int AmountNeeded { get => amountNeeded; }

    /// <summary>
    /// Whether to stop checking cards if the amount needed is met.
    /// </summary>
    public bool StopCountingAtAmount { get => stopCountingAtAmount; }

    /// <summary>
    /// The operation to do after this rule is checked. 
    /// For example, trimming the scoring cards means they wont be counted again for later rules.
    /// </summary>
    public HandAfterRuleOperation AfterRuleOperation { get => afterRuleOperation; }

    /// <summary>
    /// The Card Values allowed to pass this rule.
    /// </summary>
    public bool[] AllowedValues { get => allowedValues; }

    /// <summary>
    /// The Card Suits allowed to pass this rule.
    /// </summary>
    public bool[] AllowedSuits { get => allowedSuits; }
}
