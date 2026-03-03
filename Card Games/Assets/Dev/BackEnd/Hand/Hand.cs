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

    public void InsertCard(int index, Card card)
    {
        cards.Insert(index, card);
    }

    public Card ReplaceCardAt(int index, Card cardToInsert)
    {
        Card card = cards[index];
        cards[index] = cardToInsert;
        return card;
    }

    public void RemoveCardAt(int index)
    {
        cards.RemoveAt(index);
    }

    public bool RemoveCard(Card card)
    {
        return cards.Remove(card);
    }

    public List<Card> GetUnheldCards()
    {
        List<Card> unheldCards = new List<Card>();

        foreach (Card card in cards)
        {
            if (!card.Held)
            {
                unheldCards.Add(card);
            }
        }

        return unheldCards;
    }

    public List<Card> GetHeldCards()
    {
        List<Card> heldCards = new List<Card>();

        foreach (Card card in cards)
        {
            if (card.Held)
            {
                heldCards.Add(card);
            }
        }

        return heldCards;
    }

    public List<Card> GetUnflippedCards()
    {
        List<Card> unflippedCards = new List<Card>();

        foreach (Card card in cards)
        {
            if (!card.Flipped)
            {
                unflippedCards.Add(card);
            }
        }

        return unflippedCards;
    }

    public List<Card> GetFlippedCards()
    {
        List<Card> flippedCards = new List<Card>();

        foreach (Card card in cards)
        {
            if (card.Flipped)
            {
                flippedCards.Add(card);
            }
        }

        return flippedCards;
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
