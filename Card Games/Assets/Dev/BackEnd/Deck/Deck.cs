using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Deck has a list of cards that can be shuffled and drawn from.
/// </summary>
public class Deck
{
    /// <summary>
    /// The cards currently in this Deck.
    /// </summary>
    private List<Card> cards;

    /// <summary>
    /// Deck constructor. Resets the list of cards, then adds cards to it based on the provided Deck Data.
    /// </summary>
    /// <param name="deckData">The rules for what kind of cards this Deck should have.</param>
    public Deck(DeckData deckData)
    {
        cards = new List<Card>();

        for (int i = 0; i < deckData.DeckSuits.Count; i++)
        {
            for (int j = 0; j < deckData.DeckSuits[i].Values.Length; j++)
            {
                if (deckData.DeckSuits[i].Values[j])
                    cards.Add(new Card((CardValue)j, deckData.DeckSuits[i].Suit, CardSpecial.None));
            }
        }

        for (int i = 0; i < deckData.DeckSpecials.Count; i++)
        {
            for (int j = 0; j < deckData.DeckSpecials[i].Amount; j++)
            {
                switch (deckData.DeckSpecials[i].Type)
                {
                    case SpecialDeckDataType.RandomBasicCards:
                        cards.Add(new Card((CardValue)Random.Range(0, 13), (CardSuit)Random.Range(0, 4), CardSpecial.None));
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Randomizes the positions of all cards in the Deck.
    /// </summary>
    public void ShuffleDeck()
    {
        List<Card> shuffledCards = new List<Card>(cards.Capacity);

        while (cards.Count > 0)
        {
            int randomIndex = Random.Range(0, cards.Count);
            Card card = cards[randomIndex];
            cards.RemoveAt(randomIndex);
            shuffledCards.Add(card);
        }

        cards = shuffledCards;
    }

    /// <summary>
    /// Removes the top-most card from the list and returns it.
    /// </summary>
    /// <returns>The top-most card in the Deck.</returns>
    public Card DrawCard()
    {
        Card card = cards[cards.Count - 1];
        cards.RemoveAt(cards.Count - 1);
        return card;
    }

    /// <summary>
    /// Gets the number of undrawn cards left in the deck.
    /// </summary>
    /// <returns>The number of undrawn cards left in the deck.</returns>
    public int NumberOfCardsLeft()
    {
        return cards.Count;
    }

}
