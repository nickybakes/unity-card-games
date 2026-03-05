using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Poker Paytable is defined by the hands that can be achieved.
/// </summary>
[CreateAssetMenu(fileName = "PaytableDataPoker", menuName = "Scriptable Objects/Paytable/Paytable Data Poker")]
public class PaytableDataPoker : PaytableDataBase
{
    /// <summary>
    /// The list of Rows in this Paytable.
    /// </summary>
    [field: SerializeField] public List<PaytableRowPoker> Rows { get; private set; }

    // Inherit docs.
    public override int GetRowCount()
    {
        return Rows.Count;
    }

    /// <summary>
    /// Get the name of the Poker Hand for this row.
    /// </summary>
    /// <param name="index">The index of the row.</param>
    /// <returns>The name of the Poker hand for this row.</returns>
    public override string GetRowName(int index)
    {
        return Rows[index].Hand.Name;
    }

    // Inherit docs.
    public override float GetBetMultiplier(int index)
    {
        return Rows[index].BetMultiplier;
    }

    /// <summary>
    /// In poker, getting any Poker Hand from the Paytable is a win.
    /// </summary>
    /// <param name="index">The index of the row.</param>
    /// <returns>Returns true.</returns>
    public override bool IsAWin(int index)
    {
        return true;
    }

    /// <summary>
    /// Gets the Poker Hand data from a row.
    /// </summary>
    /// <param name="index">The index of the row.</param>
    /// <returns>The Poker Hand data from the row.</returns>
    public PokerHand GetPokerHand(int index)
    {
        return Rows[index].Hand;
    }
}
