using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckData", menuName = "Scriptable Objects/Deck Data")]
public class DeckData : ScriptableObject
{
    [field: SerializeField] public List<DeckSuitData> DeckSuits { get; private set; }
    [field: SerializeField] public List<DeckSpecialData> DeckSpecials { get; private set; }
}
