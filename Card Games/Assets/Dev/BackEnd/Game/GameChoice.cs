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

[CreateAssetMenu(fileName = "GameChoice", menuName = "Scriptable Objects/Game Choice")]
public class GameChoice : ScriptableObject
{
    [field: SerializeField] public GameChoiceIndex GameChoiceIndex { get; private set; }
}
