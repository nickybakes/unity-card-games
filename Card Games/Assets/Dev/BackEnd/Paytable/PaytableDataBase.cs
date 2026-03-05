using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Paytable has rows, and those rows have names and bet multipliers.
/// </summary>
public abstract class PaytableDataBase : ScriptableObject
{
    /// <summary>
    /// Get the number of rows in the Paytable.
    /// </summary>
    /// <returns></returns>
    public abstract int GetRowCount();

    /// <summary>
    /// Get the name of a Paytable row.
    /// </summary>
    /// <param name="index">The index of the row.</param>
    /// <returns>The name of the row.</returns>
    public abstract string GetRowName(int index);

    /// <summary>
    /// Get the bet multiplier of a Paytable row.
    /// </summary>
    /// <param name="index">The index of the row.</param>
    /// <returns>The bet multiplier of the row.</returns>
    public abstract float GetBetMultiplier(int index);


    /// <summary>
    /// Get whether a row is considered a win or not.
    /// </summary>
    /// <param name="index">The index of the row.</param>
    /// <returns>Whether the row is considered a win or not.</returns>
    public abstract bool IsAWin(int index);

}
