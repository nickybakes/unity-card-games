using UnityEngine;

/// <summary>
/// An enum that is tied to a game the player can select. Add onto this list when adding more games.
/// </summary>
public enum GameChoiceIndex
{
    JacksOrBetter,
    JacksOrBetterEasy,
    Blackjack,
    Blackjack2,
}

/// <summary>
/// Scriptable Object that represents a game the player can chose to play.
/// </summary>
[CreateAssetMenu(fileName = "GameChoice", menuName = "Scriptable Objects/Game Choice")]
public class GameChoice : ScriptableObject
{
    /// <summary>
    /// The index of the game this choice is tied to.
    /// </summary>
    [field: SerializeField] public GameChoiceIndex GameChoiceIndex { get; private set; }
}
