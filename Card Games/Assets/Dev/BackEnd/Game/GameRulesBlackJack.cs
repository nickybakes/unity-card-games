using UnityEngine;

/// <summary>
/// Scriptable Object for the game rules for a Blackjack game.
/// </summary>
[CreateAssetMenu(fileName = "GameRulesBlackJack", menuName = "Scriptable Objects/Game Rules/Game Rules Black Jack")]
public class GameRulesBlackJack : GameRulesBase
{
    /// <summary>
    /// The Blackjack paytable.
    /// </summary>
    [Header("Black Jack Game Rules")]
    [field: SerializeField] public PaytableDataBlackJack paytableData { get; private set; }

    /// <summary>
    /// The starting limit for busting.
    /// </summary>
    [field: SerializeField] public int BaseScoreLimit { get; private set; }

    /// <summary>
    /// The number of cards the player is first dealt.
    /// </summary>
    [field: SerializeField] public int PlayerBaseHandSize { get; private set; }

    /// <summary>
    /// The number of unflipped cards dealt to the dealer.
    /// </summary>
    [field: SerializeField] public int DealerUnflippedCards { get; private set; }

    /// <summary>
    /// The number of flipped cards dealt to the dealer.
    /// </summary>
    [field: SerializeField] public int DealerFlippedCards { get; private set; }

    /// <summary>
    /// The starting threshold for when the dealer needs to hit.
    /// </summary>
    [field: SerializeField] public int DealerDrawIfUnder { get; private set; }

    /// <summary>
    /// The score value of a Jack card.
    /// </summary>
    [field: SerializeField] public int JackScoreValue { get; private set; }

    /// <summary>
    /// The score value of a Queen card.
    /// </summary>
    [field: SerializeField] public int QueenScoreValue { get; private set; }

    /// <summary>
    /// The score value of a King card.
    /// </summary>
    [field: SerializeField] public int KingScoreValue { get; private set; }

    /// <summary>
    /// The low score value of an Ace card.
    /// </summary>
    [field: SerializeField] public int AceScoreValueLow { get; private set; }

    /// <summary>
    /// The high score value of an Ace card.
    /// </summary>
    [field: SerializeField] public int AceScoreValueHigh { get; private set; }

}
