using System.Collections.Generic;
using UnityEngine;

public abstract class PaytableDataBase : ScriptableObject
{
    public abstract int GetRowCount();

    public abstract string GetRowName(int index);

    public abstract float GetBetMultiplier(int index);
}
