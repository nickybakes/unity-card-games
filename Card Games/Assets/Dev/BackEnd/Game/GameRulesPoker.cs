using UnityEngine;

/// <summary>
/// Scriptable Object for the game rules for a Poker game.
/// </summary>
[CreateAssetMenu(fileName = "GameRulesPoker", menuName = "Scriptable Objects/Game Rules/Game Rules Poker")]
public class GameRulesPoker : GameRulesBase
{

    /// <summary>
    /// The Poker paytable.
    /// </summary>
    [Header("Poker Game Rules")]
    [field: SerializeField] public PaytableDataPoker paytableData { get; private set; }

    /// <summary>
    /// The number of cards the player is dealt.
    /// </summary>
    [field: SerializeField] public int HandSize { get; private set; } = 5;

    /// <summary>
    /// The number of draws the player is given.
    /// </summary>
    [field: SerializeField] public int NumberOfDraws { get; private set; } = 1;

}
