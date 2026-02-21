using System;
using UnityEngine;

[Serializable]
public class PaytableRowBlackJack
{
    [SerializeField] private string name;
    [SerializeField] private float betMultiplier;

    public string Name { get => name; }
    public float BetMultiplier { get => betMultiplier; }

}
