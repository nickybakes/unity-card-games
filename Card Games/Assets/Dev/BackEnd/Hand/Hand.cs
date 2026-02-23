using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    private List<Card> cards;

    public List<Card> Cards { get => cards; }

    public Hand()
    {
        cards = new List<Card>();
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    public void RemoveCardAt(int index)
    {
        cards.RemoveAt(index);
    }

    public bool RemoveCard(Card card)
    {
        return cards.Remove(card);
    }

    public void ShuffleHand()
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

}
