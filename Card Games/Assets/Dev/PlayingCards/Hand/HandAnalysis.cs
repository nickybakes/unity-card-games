using System.Collections.Generic;
using UnityEngine;

public class HandAnalysis
{

    public static List<Card> TrimHand(List<Card> hand, List<CardValue> allowedValues, List<CardSuit> allowedSuits)
    {
        List<Card> trimmedHand = new List<Card>(hand);

        for (int i = 0; i < hand.Count; i++)
        {
            if (!allowedValues.Contains(trimmedHand[i].Value))
            {
                trimmedHand.RemoveAt(i);
                i--;
            }
            else if (!allowedSuits.Contains(trimmedHand[i].Suit))
            {
                trimmedHand.RemoveAt(i);
                i--;
            }
        }

        return trimmedHand;
    }

    public static bool HasValueCollection(List<Card> hand, CardValue value, int amountNeeded, bool exactAmount)
    {
        int numMatchingCards = 0;

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].Value == value)
            {
                numMatchingCards++;
                if (numMatchingCards == amountNeeded && !exactAmount)
                {
                    return true;
                }
            }
        }

        return numMatchingCards == amountNeeded;
    }

    public static bool HasSuitCollection(List<Card> hand, CardSuit suit, int amountNeeded, bool exactAmount)
    {
        int numMatchingCards = 0;

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].Suit == suit)
            {
                numMatchingCards++;
                if (numMatchingCards == amountNeeded && !exactAmount)
                {
                    return true;
                }
            }
        }

        return numMatchingCards == amountNeeded;
    }

    public static bool HasConsecutiveValues(List<Card> hand, int amountNeeded, bool exactAmount, bool sameSuit)
    {
        int lengthOfConsecutiveCards = 0;

        for (int i = 0; i < hand.Count; i++)
        {
            List<Card> remainingCards = new List<Card>(hand);
            remainingCards.RemoveAt(i);
            int newLength = GetLengthOfConsecutiveCards(hand[i], remainingCards, sameSuit);

            if (newLength >= amountNeeded && !exactAmount)
            {
                return true;
            }

            if (newLength >= lengthOfConsecutiveCards)
            {
                lengthOfConsecutiveCards = newLength;
            }
        }

        return lengthOfConsecutiveCards == amountNeeded;
    }

    private static int GetLengthOfConsecutiveCards(Card currentCard, List<Card> remainingCards, bool sameSuit)
    {
        if (remainingCards.Count == 0)
        {
            return 1;
        }

        for (int i = 0; i < remainingCards.Count; i++)
        {
            Card nextCard = remainingCards[i];
            if ((int)nextCard.Value - (int)currentCard.Value == 1 || (currentCard.Value == CardValue.ACE && nextCard.Value == CardValue.TWO))
            {
                if (!sameSuit || currentCard.Suit == nextCard.Suit)
                {
                    List<Card> remainingCardsWithoutNextCard = new List<Card>(remainingCards);
                    remainingCardsWithoutNextCard.RemoveAt(i);
                    return 1 + GetLengthOfConsecutiveCards(nextCard, remainingCardsWithoutNextCard, sameSuit);
                }
            }
        }
        return 0;
    }

}
