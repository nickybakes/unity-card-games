using System.Collections.Generic;
using UnityEngine;

public class HandAnalysis
{

    public static bool AnalizeForPokerHand(List<Card> hand, PokerHand pokerHand)
    {
        List<Card> trimmedHand = new List<Card>(hand);

        foreach (PokerHandRule rule in pokerHand.Rules)
        {
            List<Card> allowedCards = TrimHandToAllowedCards(trimmedHand, rule.AllowedValues, rule.AllowedSuits);
            List<Card> scoringCards = GetCardsThatPassCondition(allowedCards, rule.Type, rule.AmountNeeded, rule.StopCountingAtAmount);

            if (scoringCards.Count == 0)
                return false;

            switch (rule.AfterRuleOperation)
            {
                case HandAfterRuleOperation.TrimScoringCards:
                    foreach (Card scoringCard in scoringCards)
                    {
                        trimmedHand.Remove(scoringCard);
                    }
                    break;
                case HandAfterRuleOperation.TrimNonscoringCards:
                    for (int i = 0; i < trimmedHand.Count; i++)
                    {
                        if (!scoringCards.Contains(trimmedHand[i]))
                        {
                            trimmedHand.Remove(trimmedHand[i]);
                            i--;
                        }
                    }
                    break;
            }
        }

        return true;
    }

    public static List<Card> TrimHandToAllowedCards(List<Card> hand, bool[] allowedValues, bool[] allowedSuits)
    {
        List<Card> trimmedHand = new List<Card>(hand);

        for (int i = 0; i < trimmedHand.Count; i++)
        {
            for (int j = 0; j < allowedValues.Length; j++)
            {
                if (!allowedValues[j] && (int)trimmedHand[i].Value == j)
                {
                    trimmedHand.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }

        for (int i = 0; i < trimmedHand.Count; i++)
        {
            for (int j = 0; j < allowedSuits.Length; j++)
            {
                if (!allowedSuits[j] && (int)trimmedHand[i].Suit == j)
                {
                    trimmedHand.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }

        return trimmedHand;
    }

    public static List<Card> GetCardsThatPassCondition(List<Card> hand, HandComponent conditionToCheck, int amountNeeded, bool stopCountingAtAmount)
    {
        List<Card> mostPassingCards = new List<Card>();

        for (int i = 0; i < hand.Count; i++)
        {
            Card currentCard = hand[i];
            List<Card> currentPassingCards = new List<Card> { currentCard };

            List<Card> remainingCards = new List<Card>(hand);
            remainingCards.RemoveAt(i);

            for (int j = 0; j < remainingCards.Count; j++)
            {
                Card nextCard = remainingCards[j];
                if (DoCardsPassCondition(currentCard, nextCard, conditionToCheck))
                {
                    currentPassingCards.Add(nextCard);
                    currentCard = nextCard;
                    remainingCards.RemoveAt(j);
                    j = -1;

                    if (stopCountingAtAmount && currentPassingCards.Count == amountNeeded)
                    {
                        return currentPassingCards;
                    }
                }
            }

            if (currentPassingCards.Count > mostPassingCards.Count && currentPassingCards.Count >= amountNeeded)
            {
                mostPassingCards = currentPassingCards;
            }
        }

        return mostPassingCards;
    }

    private static bool DoCardsPassCondition(Card a, Card b, HandComponent conditionToCheck)
    {
        switch (conditionToCheck)
        {
            case HandComponent.ValueCollection:
                return a.Value == b.Value;
            case HandComponent.SuitCollection:
                return a.Suit == b.Suit;
            case HandComponent.ConsecutiveValues:
                return (int)b.Value - (int)a.Value == 1 || (a.Value == CardValue.Ace && b.Value == CardValue.Two);
        }

        return false;
    }
}
