using System;
using UnityEngine;

[Serializable]
public class PokerHandRule
{
    [SerializeField] private HandComponent type;
    [SerializeField] private int amountNeeded;
    [SerializeField] private bool stopCountingAtAmount;
    [SerializeField] private HandAfterRuleOperation afterRuleOperation;
    [SerializeField] private bool[] allowedValues;
    [SerializeField] private bool[] allowedSuits;

    public HandComponent Type { get => type; }
    public int AmountNeeded { get => amountNeeded; }
    public bool StopCountingAtAmount { get => stopCountingAtAmount; }
    public HandAfterRuleOperation AfterRuleOperation { get => afterRuleOperation; }
    public bool[] AllowedValues { get => allowedValues; }
    public bool[] AllowedSuits { get => allowedSuits; }
}
