using System;
using UnityEngine;

[Serializable]
public class Card
{
    public static string[] CARD_VALUE_STRINGS = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
    public static string[] CARD_SUIT_STRINGS = { "Spades", "Hearts", "Clubs", "Diamonds" };

    public CardValue Value { get; private set; }

    public CardSuit Suit { get; private set; }

    public CardSpecial Special { get; private set; }

    public Card(CardValue value, CardSuit suit, CardSpecial special)
    {
        Value = value;
        Suit = suit;
        Special = special;
    }
}
