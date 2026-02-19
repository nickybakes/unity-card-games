using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private List<Card> cards;

    public Deck(DeckData deckData)
    {
        cards = new List<Card>();

        for (int i = 0; i < deckData.DeckSuits.Count; i++)
        {
            for (int j = 0; j < deckData.DeckSuits[i].Values.Length; j++)
            {
                if (deckData.DeckSuits[i].Values[j])
                    cards.Add(new Card((CardValue)j, deckData.DeckSuits[i].Suit, CardSpecial.NONE));
            }
        }

        for (int i = 0; i < deckData.DeckSpecials.Count; i++)
        {
            for (int j = 0; j < deckData.DeckSpecials[i].Amount; j++)
            {
                switch (deckData.DeckSpecials[i].Type)
                {
                    case SpecialDeckDataType.RANDOM_BASIC:
                        cards.Add(new Card((CardValue)Random.Range(0, 13), (CardSuit)Random.Range(0, 4), CardSpecial.NONE));
                        break;
                }
            }
        }
    }

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

    public Card DrawCard()
    {
        Card card = cards[cards.Count - 1];
        cards.RemoveAt(cards.Count - 1);
        return card;
    }

}
