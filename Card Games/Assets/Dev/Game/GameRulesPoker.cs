using UnityEngine;

[CreateAssetMenu(fileName = "GameRulesPoker", menuName = "Scriptable Objects/Game Rules Poker")]
public class GameRulesPoker : GameRules
{
    [field: SerializeField] public PaytableData paytableData { get; private set; }
}
