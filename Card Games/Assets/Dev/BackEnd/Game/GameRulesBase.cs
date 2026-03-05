using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Game Rules are the numbers/data that denote how a game is played. Should be extended for specific types of games.
/// </summary>
public class GameRulesBase : ScriptableObject
{
    /// <summary>
    /// The display name of this game.
    /// </summary>
    [Header("Base Game Rules")]
    [field: SerializeField] public string Name { get; private set; }

    /// <summary>
    /// The display description of this game.
    /// </summary>
    [field: SerializeField, TextArea] public string Description { get; private set; }

    /// <summary>
    /// The number of hands to set up in this game.
    /// </summary>
    [field: SerializeField] public int NumberOfHands { get; private set; }

    /// <summary>
    /// The list of Deck Datas to make decks from in this game.
    /// </summary>
    [field: SerializeField] public List<DeckData> Decks { get; private set; }

    /// <summary>
    /// The index of the scene this game is played in.
    /// </summary>
    [field: SerializeField] public SceneIndex SceneToLoad { get; private set; }
}
