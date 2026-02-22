using UnityEngine;

[CreateAssetMenu(fileName = "GameRulesPoker", menuName = "Scriptable Objects/Game Rules/Game Rules Poker")]
public class GameRulesPoker : GameRulesBase
{

    [Header("Poker Game Rules")]
    [field: SerializeField] public PaytableDataPoker paytableData { get; private set; }

    [field: SerializeField] public int HandSize { get; private set; }

}
