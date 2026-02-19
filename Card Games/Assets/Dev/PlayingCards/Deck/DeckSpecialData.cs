using System;
using UnityEngine;

[Serializable]
public class DeckSpecialData
{

    [SerializeField] private SpecialDeckDataType type;

    [SerializeField] private int amount;

    public SpecialDeckDataType Type { get => type; }
    public int Amount { get => amount; }

}

public enum SpecialDeckDataType
{
    RANDOM_BASIC,
    BOMB
}
