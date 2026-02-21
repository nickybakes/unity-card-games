using UnityEngine;

[CreateAssetMenu(fileName = "GameRulesPoker", menuName = "Scriptable Objects/Game Rules/Game Rules Poker")]
public class GameRulesPoker : GameRules
{
    [field: SerializeField] public PaytableDataPoker paytableData { get; private set; }
}
