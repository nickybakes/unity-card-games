using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PaytableData", menuName = "Scriptable Objects/Paytable Data")]
public class PaytableData : ScriptableObject
{
    [field: SerializeField] public List<PaytableRow> Rows { get; private set; }
}
