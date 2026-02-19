using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokerHand", menuName = "Scriptable Objects/Poker Hand")]
public class PokerHand : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField, TextArea] public string Description { get; private set; }

    [field: SerializeField] public List<PokerHandRule> Rules { get; private set; }

}
