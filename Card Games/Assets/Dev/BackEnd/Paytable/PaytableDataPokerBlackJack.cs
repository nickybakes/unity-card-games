using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PaytableDataBlackJack", menuName = "Scriptable Objects/Paytable/Paytable Data Black Jack")]
public class PaytableDataBlackJack : PaytableDataBase
{
    [field: SerializeField] public List<PaytableRowBlackJack> Rows { get; private set; }

    public override int GetRowCount()
    {
        return Rows.Count;
    }

    public override string GetRowName(int index)
    {
        return Rows[index].Name;
    }

    public override float GetBetMultiplier(int index)
    {
        return Rows[index].BetMultiplier;
    }
}
