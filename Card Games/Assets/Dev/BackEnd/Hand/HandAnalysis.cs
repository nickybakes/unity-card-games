using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains static methods for analyzing lists of cards.
/// </summary>
public class HandAnalysis
{

    /// <summary>
    /// Check if a list of Cards contains a Poker Hand.
    /// </summary>
    /// <param name="hand">The list of Cards to check.</param>
    /// <param name="pokerHand">The Poker Hand to check for.</param>
    /// <returns>True if all Poker Hand rules were passed, false otherwise.</returns>
    public static bool AnalizeForPokerHand(List<Card> hand, PokerHand pokerHand)
    {
        List<Card> trimmedHand = new List<Card>(hand);

        // Check if the list of Cards passes all rules for the Poker Hand.
        foreach (PokerHandRule rule in pokerHand.Rules)
        {
            List<Card> allowedCards = TrimHandToAllowedCards(trimmedHand, rule.AllowedValues, rule.AllowedSuits);
            List<Card> scoringCards = GetCardsThatPassCondition(allowedCards, rule.Type, rule.AmountNeeded, rule.StopCountingAtAmount);

            if (scoringCards.Count == 0)
                return false;

            // Do the after-rule operation, like trimming the scoring/non-scoring cards
            // from the analysis.
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

    /// <summary>
    /// Filters out a list of Cards based on the allowed values and suits.
    /// </summary>
    /// <param name="hand">The list of cards to filter.</param>
    /// <param name="allowedValues">The allowed values.</param>
    /// <param name="allowedSuits">The allowed suits.</param>
    /// <returns>Filtered list of Cards.</returns>
    public static List<Card> TrimHandToAllowedCards(List<Card> hand, bool[] allowedValues, bool[] allowedSuits)
    {
        List<Card> trimmedHand = new List<Card>(hand);

        // First filter for values.
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

        // Then filter for suits.
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

    /// <summary>
    /// Filters out a list of Cards based on the condition and amount needed.
    /// </summary>
    /// <param name="hand">The list of Cards to filter.</param>
    /// <param name="conditionToCheck">The hand component condition to check for.</param>
    /// <param name="amountNeeded">The number of cards necessary for the whole list to pass.</param>
    /// <param name="stopCountingAtAmount">Whether to stop checking cards ones the amount needed is met.</param>
    /// <returns>Filtered list of Cards.</returns>
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
                if (DoCardsPassCondition(currentCard, nextCard, conditionToCheck, currentCard == hand[i]))
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

    /// <summary>
    /// Calculates the score value of a list of cards based on the score limit.
    /// Aces will automatically be low if the score limit is reached.
    /// </summary>
    /// <param name="hand">The list of cards to calculate for.</param>
    /// <param name="jackValue">The score value of a Jack.</param>
    /// <param name="queenValue">The score value of a Queen.</param>
    /// <param name="kingValue">The score value of a King.</param>
    /// <param name="aceValueHigh">The high score value of an Ace.</param>
    /// <param name="aceValueLow">The low score value of an Ace.</param>
    /// <param name="scoreLimit">The score limit.</param>
    /// <returns>The calculates score.</returns>
    public static int GetHandScore(List<Card> hand, int jackValue, int queenValue, int kingValue, int aceValueHigh, int aceValueLow, int scoreLimit)
    {
        int numAces = 0;

        // Count up the score of the non-ace cards.
        int totalScore = 0;
        for (int i = 0; i < hand.Count; i++)
        {
            Card card = hand[i];

            if (card.Value == CardValue.Ace)
            {
                numAces++;
                continue;
            }

            int cardScore = Card.CARD_VALUE_SCORES[(int)card.Value];
            switch (card.Value)
            {
                case CardValue.Jack:
                    cardScore = jackValue;
                    break;
                case CardValue.Queen:
                    cardScore = queenValue;
                    break;
                case CardValue.King:
                    cardScore = kingValue;
                    break;
            }
            totalScore += cardScore;
        }

        // Work backwards from the score limit to find the highest score without going over, if possible.
        int acesScore = numAces * aceValueHigh;
        for (int i = 0; i < numAces; i++)
        {
            if (totalScore + acesScore > scoreLimit)
            {
                acesScore -= aceValueHigh;
                acesScore += aceValueLow;
            }
        }

        return totalScore + acesScore;
    }

    /// <summary>
    /// Checks if 2 cards pass a specific condition.
    /// </summary>
    /// <param name="a">The first card.</param>
    /// <param name="b">The second card to compare with the first card.</param>
    /// <param name="conditionToCheck">The hand component condition to check for.</param>
    /// <param name="aIsFirstCardInList">If the first card is the first in a list of scoring cards.</param>
    /// <returns>Returns whether the two cards pass the condition or not.</returns>
    private static bool DoCardsPassCondition(Card a, Card b, HandComponent conditionToCheck, bool aIsFirstCardInList)
    {
        switch (conditionToCheck)
        {
            case HandComponent.MatchingValues:
                return a.Value == b.Value;
            case HandComponent.MatchingSuits:
                return a.Suit == b.Suit;
            case HandComponent.ConsecutiveValues:
                return (int)b.Value - (int)a.Value == 1 || (a.Value == CardValue.Ace && b.Value == CardValue.Two && aIsFirstCardInList);
        }

        return false;
    }
}
