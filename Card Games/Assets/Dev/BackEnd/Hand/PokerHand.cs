using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Poker Hand has a list of rules/conditions that must be met for a set of cards to contain that Poker Hand.
/// </summary>
[CreateAssetMenu(fileName = "PokerHand", menuName = "Scriptable Objects/Poker Hand")]
public class PokerHand : ScriptableObject
{
    /// <summary>
    /// The name of the Poker Hand.
    /// </summary>
    [field: SerializeField] public string Name { get; private set; }

    /// <summary>
    /// The description of the Poker Hand.
    /// </summary>
    [field: SerializeField, TextArea] public string Description { get; private set; }

    /// <summary>
    /// The rules/conditions that must be met for a set of cards to contain this Poker Hand.
    /// </summary>
    [field: SerializeField] public List<PokerHandRule> Rules { get; private set; }

}
