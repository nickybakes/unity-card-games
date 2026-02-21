using UnityEngine;

[CreateAssetMenu(fileName = "GameRulesBlackJack", menuName = "Scriptable Objects/Game Rules/Game Rules Black Jack")]
public class GameRulesBlackJack : GameRules
{
    [field: SerializeField] public PaytableDataBlackJack paytableData { get; private set; }
}
