using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object of rules for what cards are in a Deck.
/// </summary>
[CreateAssetMenu(fileName = "DeckData", menuName = "Scriptable Objects/Deck Data")]
public class DeckData : ScriptableObject
{
    /// <summary>
    /// Rules for adding cards of various values from specific suits.
    /// </summary>
    [field: SerializeField] public List<DeckSuitData> DeckSuits { get; private set; }

    /// <summary>
    /// Special rules for adding specific types of cards.
    /// </summary>
    [field: SerializeField] public List<DeckSpecialData> DeckSpecials { get; private set; }
}
