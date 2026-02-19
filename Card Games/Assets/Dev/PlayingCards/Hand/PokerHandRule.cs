using System;
using UnityEngine;

[Serializable]
public class PokerHandRule
{
    [SerializeField] private HandComponent type;
    [SerializeField] private int amountNeeded;
    [SerializeField] private bool exactAmount;
    [SerializeField] private bool uniqueValue;
    [SerializeField] private bool[] allowedValues;
    [SerializeField] private bool[] allowedSuits;

    public HandComponent Type { get => type; }
    public int AmountNeeded { get => amountNeeded; }
    public bool ExactAmount { get => exactAmount; }
    public bool UniqueValue { get => uniqueValue; }
    public bool[] AllowedValues { get => allowedValues; }
    public bool[] AllowedSuits { get => allowedSuits; }
}
