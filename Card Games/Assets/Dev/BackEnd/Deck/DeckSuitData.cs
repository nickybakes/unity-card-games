using System;
using UnityEngine;

[Serializable]
public class DeckSuitData
{

    [SerializeField] private CardSuit suit;

    [SerializeField] private bool[] values;

    public CardSuit Suit { get => suit; }
    public bool[] Values { get => values; }

}
