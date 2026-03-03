using UnityEngine;

[CreateAssetMenu(fileName = "GameRulesBlackJack", menuName = "Scriptable Objects/Game Rules/Game Rules Black Jack")]
public class GameRulesBlackJack : GameRulesBase
{
    [Header("Black Jack Game Rules")]

    [field: SerializeField] public PaytableDataBlackJack paytableData { get; private set; }

    [field: SerializeField] public int BaseScoreLimit { get; private set; }
    [field: SerializeField] public int PlayerBaseHandSize { get; private set; }
    [field: SerializeField] public int DealerUnflippedCards { get; private set; }
    [field: SerializeField] public int DealerFlippedCards { get; private set; }
    [field: SerializeField] public int JackScoreValue { get; private set; }
    [field: SerializeField] public int QueenScoreValue { get; private set; }
    [field: SerializeField] public int KingScoreValue { get; private set; }
    [field: SerializeField] public int AceScoreValueLow { get; private set; }
    [field: SerializeField] public int AceScoreValueHigh { get; private set; }

}
