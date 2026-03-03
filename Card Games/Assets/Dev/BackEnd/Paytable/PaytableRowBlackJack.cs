using System;
using UnityEngine;

[Serializable]
public class PaytableRowBlackJack
{
    [SerializeField] private string name;
    [SerializeField] private float betMultiplier;
    [SerializeField] private bool isAWin;

    public string Name { get => name; }
    public float BetMultiplier { get => betMultiplier; }
    public bool IsAWin { get => isAWin; }

}
