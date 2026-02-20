using System;
using UnityEngine;

[Serializable]
public class Card
{
    public static string[] CARD_VALUE_STRINGS = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
    public static string[] CARD_SUIT_STRINGS = { "Spades", "Hearts", "Clubs", "Diamonds" };

    [SerializeField] private CardValue value;
    [SerializeField] private CardSuit suit;
    [SerializeField] private CardSpecial special;

    public CardValue Value { get => value; private set => this.value = value; }

    public CardSuit Suit { get => suit; private set => suit = value; }

    public CardSpecial Special { get => special; private set => special = value; }

    public Card(CardValue _value, CardSuit _suit, CardSpecial _special)
    {
        Value = _value;
        Suit = _suit;
        Special = _special;
    }
}
