using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the data of a Card in game. Has a Value and Suit, can be flipped or held, and can be expanded to have other properties.
/// </summary>
[Serializable]
public class Card
{
    /// <summary>
    /// String representations of the basic Card Values.
    /// </summary>
    public static string[] CARD_VALUE_STRINGS = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

    /// <summary>
    /// String representations of the basic Card Suits.
    /// </summary>
    public static string[] CARD_SUIT_STRINGS = { "Spades", "Hearts", "Clubs", "Diamonds" };

    /// <summary>
    /// Mapping of Card Values to generic numeric values.
    /// </summary>
    public static int[] CARD_VALUE_SCORES = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

    /// <summary>
    /// The value of the card, such as 2-10, Jack, Queen, King, or Ace.
    /// </summary>
    [SerializeField] private CardValue value;

    /// <summary>
    /// The suit of the card, such as Spade, Heart, Club, or Diamond.
    /// </summary>
    [SerializeField] private CardSuit suit;

    /// <summary>
    /// Any special properties of the card, such as if its Wild or not. Can be expanded upon.
    /// </summary>
    [SerializeField] private CardSpecial special;

    /// <summary>
    /// If true, then the card is "flipped" to show the card's back. False by default.
    /// </summary>
    private bool flipped;

    /// <summary>
    /// If true, then the card is selected to be "held" which can be used by the Game Manager for gameplay logic. False by default.
    /// </summary>
    private bool held;

    /// <summary>
    /// The value of the card, such as 2-10, Jack, Queen, King, or Ace.
    /// </summary>
    public CardValue Value { get => value; private set => this.value = value; }

    /// <summary>
    /// The suit of the card, such as Spade, Heart, Club, or Diamond.
    /// </summary>
    public CardSuit Suit { get => suit; private set => suit = value; }

    /// <summary>
    /// The suit of the card, such as Spade, Heart, Club, or Diamond.
    /// </summary>
    public CardSpecial Special { get => special; private set => special = value; }

    /// <summary>
    /// If true, then the card is "flipped" to show the card's back. False by default.
    /// </summary>
    public bool Flipped { get => flipped; private set => flipped = value; }

    /// <summary>
    /// If true, then the card is selected to be "held" which can be used by the Game Manager for gameplay logic. False by default.
    /// </summary>
    public bool Held { get => held; private set => held = value; }

    /// <summary>
    /// Card constructor. Sets the basic values of the card.
    /// </summary>
    /// <param name="_value">The Value of the card.</param>
    /// <param name="_suit">The Suit of the card.</param>
    /// <param name="_special">Special properties of the card.</param>
    public Card(CardValue _value, CardSuit _suit, CardSpecial _special)
    {
        Value = _value;
        Suit = _suit;
        Special = _special;
    }

    /// <summary>
    /// Inverts the flipped status of the card.
    /// </summary>
    public void InvertFlipped()
    {
        flipped = !flipped;
    }

    /// <summary>
    /// Inverts the held status of the card.
    /// </summary>
    public void InvertHeld()
    {
        held = !held;
    }

    /// <summary>
    /// String formatting of the card, mainly to be used for Debugging.
    /// </summary>
    /// <returns>The value and suit of the card formatted to a string.</returns>
    public override string ToString()
    {
        return value + " of " + suit + "s";
    }

    /// <summary>
    /// Prints out a list of Cards.
    /// </summary>
    /// <param name="cardList">The list of Cards to print.</param>
    public static void DebugLogCardList(List<Card> cardList)
    {
        foreach (Card card in cardList)
        {
            Debug.Log(card);
        }
    }
}
