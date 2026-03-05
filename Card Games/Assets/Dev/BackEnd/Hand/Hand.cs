using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Hand has a list of cards that can be edited and filtered by Card status.
/// </summary>
public class Hand
{
    /// <summary>
    /// The list of Dards in this Hand.
    /// </summary>
    private List<Card> cards;

    /// <summary>
    /// Public getter for the list of Dards in this Hand.
    /// </summary>
    public List<Card> Cards { get => cards; }

    /// <summary>
    /// Constructor for a Hand. Initializes the list of Cards.
    /// </summary>
    public Hand()
    {
        cards = new List<Card>();
    }

    /// <summary>
    /// Adds a Card to the Hand.
    /// </summary>
    /// <param name="card">The Card to add.</param>
    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    /// <summary>
    /// Inserts a Card at the index in the Hand.
    /// </summary>
    /// <param name="index">The index to insert at.</param>
    /// <param name="card">The Card to insert.</param>
    public void InsertCard(int index, Card card)
    {
        cards.Insert(index, card);
    }

    /// <summary>
    /// Replaces a Card at the index with a different Card.
    /// </summary>
    /// <param name="index">The index of the Card to replace.</param>
    /// <param name="cardToInsert">The Card to replace it with.</param>
    /// <returns></returns>
    public Card ReplaceCardAt(int index, Card cardToInsert)
    {
        Card card = cards[index];
        cards[index] = cardToInsert;
        return card;
    }

    /// <summary>
    /// Remove a Card at an index.
    /// </summary>
    /// <param name="index">The index to remove at.</param>
    public void RemoveCardAt(int index)
    {
        cards.RemoveAt(index);
    }

    /// <summary>
    /// Remove a Card by reference.
    /// </summary>
    /// <param name="card">The Card to remove.</param>
    /// <returns>Whether the Card was found and could be removed.</returns>
    public bool RemoveCard(Card card)
    {
        return cards.Remove(card);
    }

    /// <summary>
    /// Get a list of Cards in this Hand that are NOT held.
    /// </summary>
    /// <returns>A list of the NOT held cards in this hand.</returns>
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

    /// <summary>
    /// Get a list of Cards in this Hand that are held.
    /// </summary>
    /// <returns>A list of the held cards in this hand.</returns>
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

    /// <summary>
    /// Get a list of Cards in this Hand that are NOT flipped.
    /// </summary>
    /// <returns>A list of the NOT flipped cards in this hand.</returns>
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

    /// <summary>
    /// Get a list of Cards in this Hand that are flipped.
    /// </summary>
    /// <returns>A list of the flipped cards in this hand.</returns>
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

    /// <summary>
    /// Randomizes the positions of all Cards in the Hand.
    /// </summary>
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
