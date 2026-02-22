using UnityEngine;

[CreateAssetMenu(fileName = "GameRulesBlackJack", menuName = "Scriptable Objects/Game Rules/Game Rules Black Jack")]
public class GameRulesBlackJack : GameRulesBase
{
    [Header("Black Jack Game Rules")]

    [field: SerializeField] public PaytableDataBlackJack paytableData { get; private set; }

    [field: SerializeField] public int BaseScoreLimit { get; private set; }
    [field: SerializeField] public int BaseHandSize { get; private set; }

}
