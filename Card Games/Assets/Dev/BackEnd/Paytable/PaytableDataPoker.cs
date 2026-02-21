using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PaytableDataPoker", menuName = "Scriptable Objects/Paytable/Paytable Data Poker")]
public class PaytableDataPoker : PaytableDataBase
{
    [field: SerializeField] public List<PaytableRowPoker> Rows { get; private set; }

    public override int GetRowCount()
    {
        return Rows.Count;
    }

    public override string GetRowName(int index)
    {
        return Rows[index].Hand.Name;
    }

    public override float GetBetMultiplier(int index)
    {
        return Rows[index].BetMultiplier;
    }
}
