using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Blackjack Paytable has defined wins and loses such as "Player Wins" and "Player Busts".
/// </summary>
[CreateAssetMenu(fileName = "PaytableDataBlackJack", menuName = "Scriptable Objects/Paytable/Paytable Data Black Jack")]
public class PaytableDataBlackJack : PaytableDataBase
{
    /// <summary>
    /// The list of Rows in this Paytable.
    /// </summary>
    [field: SerializeField] public List<PaytableRowBlackJack> Rows { get; private set; }

    // Inherit docs.
    public override int GetRowCount()
    {
        return Rows.Count;
    }

    // Inherit docs.
    public override string GetRowName(int index)
    {
        return Rows[index].Name;
    }

    // Inherit docs.
    public override float GetBetMultiplier(int index)
    {
        return Rows[index].BetMultiplier;
    }

    // Inherit docs.
    public override bool IsAWin(int index)
    {
        return Rows[index].IsAWin;
    }
}
