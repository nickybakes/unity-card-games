using System;
using UnityEngine;

[Serializable]
public class PaytableRowPoker
{
    [SerializeField] private PokerHand hand;
    [SerializeField] private float betMultiplier;

    public PokerHand Hand { get => hand; }
    public float BetMultiplier { get => betMultiplier; }

}
