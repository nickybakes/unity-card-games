using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBlackJack : GameManagerBase
{

    enum PaytableResult
    {
        PlayerBust,
        PlayerLoseNoBust,
        PlayerDealerTie,
        PlayerWin
    }

    protected GameRulesBlackJack gameRulesBlackJack;

    [Header("Game Elements")]
    [SerializeField] private int dealerHandIndex;


    [Header("View Elements")]
    [SerializeField] private int hitButtonIndex;
    [SerializeField] private int standButtonIndex;
    [SerializeField] private int bustOverNumberTextIndex;
    [SerializeField] private int yourScoreNumberTextIndex;
    [SerializeField] private int finalPlayerScoreTextIndex;
    [SerializeField] private int finalDealerScoreTextIndex;
    [SerializeField] private int dealerHitWhenUnderTextIndex;

    [Header("Test Data/Cheats")]
    [SerializeField] private List<Card> testPlayerHand;

    private int currentScoreLimit;
    private int currentDealerHitWhenUnder;

    public override void LoadGameRules(GameRulesBase _gameRules)
    {
        gameRules = _gameRules;
        gameRulesBlackJack = (GameRulesBlackJack)_gameRules;
        paytable = gameRulesBlackJack.paytableData;
        changesThisTurn = new List<GameStateChange>
        {
            new GameStateChange(GameStateChangeType.GameRulesLoaded)
        };
        SubmitChanges();
        InitGame();
    }

    public override void InitGame()
    {
        SetupHands();
        HideAllTexts(GameStateChangeTime.Instant);
        HideAllButtons(GameStateChangeTime.Instant);
        BeginBetting();
        SubmitChanges();
    }

    public override void StartRound()
    {
        ResetDecks();
        currentScoreLimit = gameRulesBlackJack.BaseScoreLimit;
        currentDealerHitWhenUnder = gameRulesBlackJack.DealerDrawIfUnder;
        UpdateText(bustOverNumberTextIndex, currentScoreLimit.ToString());
        UpdateText(dealerHitWhenUnderTextIndex, currentDealerHitWhenUnder.ToString());

#if UNITY_EDITOR
        if (testPlayerHand != null && testPlayerHand.Count > 0)
        {
            foreach (Card card in testPlayerHand)
            {
                hands[0].AddCard(card);
                changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardMove, GameBoardTarget.Deck, 0, GameBoardTarget.Hand, 0, card));
            }

            for (int i = 0; i < gameRulesBlackJack.DealerUnflippedCards; i++)
            {
                DrawCardsToHand(0, dealerHandIndex, 1);
            }

            for (int i = 0; i < gameRulesBlackJack.DealerFlippedCards; i++)
            {
                DrawCardsToHand(0, dealerHandIndex, 1, true);
            }
        }
        else
        {
            DealHands();
        }
#else
            DealHands();
#endif


        CheckScoresForGameOver(false);
        SubmitChanges();
    }

    /// <summary>
    /// Deals cards to the player and dealer, alternating for each card. Then deals the flipped cards to the dealer.
    /// </summary>
    private void DealHands()
    {
        // Start with dealing unflipped cards to both the player and the dealer.
        int totalUnflippedDeals = Math.Max(gameRulesBlackJack.PlayerBaseHandSize, gameRulesBlackJack.DealerUnflippedCards);
        for (int i = 0; i < totalUnflippedDeals; i++)
        {
            for (int j = 0; j < gameRules.NumberOfHands; j++)
            {
                if ((j != dealerHandIndex && i < gameRulesBlackJack.PlayerBaseHandSize) || (j == dealerHandIndex && i < gameRulesBlackJack.DealerUnflippedCards))
                {
                    DrawCardsToHand(0, j, 1);
                }
            }
        }

        // Then deal the flipped cards to the dealer.
        for (int i = 0; i < gameRulesBlackJack.DealerFlippedCards; i++)
        {
            DrawCardsToHand(0, dealerHandIndex, 1, true);
        }
    }

    public override void PlayerSelectButton(int index)
    {
        if (index == hitButtonIndex)
        {
            HideAllButtons(GameStateChangeTime.Medium);
            DrawCardsToHand(0, 0, 1);

            CheckScoresForGameOver(false);
        }
        else if (index == standButtonIndex)
        {
            HideAllButtons(GameStateChangeTime.Instant);
            CheckScoresForGameOver(true);
        }

        SubmitChanges();

    }

    private void CheckScoresForGameOver(bool forceGameOver)
    {
        int playerScore = GetHandScore(0);

        if (forceGameOver || playerScore >= currentScoreLimit)
        {
            UpdateText(bustOverNumberTextIndex, currentScoreLimit.ToString());
            UpdateText(yourScoreNumberTextIndex, playerScore.ToString(), GameStateChangeTime.Medium);

            HideAllTexts(GameStateChangeTime.Instant);
            HideAllButtons(GameStateChangeTime.Instant);

            UnflipAllCardsInHand(dealerHandIndex);

            int dealerScore = GetHandScore(dealerHandIndex);

            UpdateText(finalDealerScoreTextIndex, "");
            UpdateText(finalPlayerScoreTextIndex, "");
            ShowText(finalDealerScoreTextIndex);
            ShowText(finalPlayerScoreTextIndex);

            while (playerScore <= currentScoreLimit && dealerScore < gameRulesBlackJack.DealerDrawIfUnder)
            {
                UpdateText(finalDealerScoreTextIndex, dealerScore.ToString());
                UpdateText(finalPlayerScoreTextIndex, playerScore.ToString(), GameStateChangeTime.Long);
                DrawCardsToHand(0, dealerHandIndex, 1, false, GameStateChangeTime.Medium);
                dealerScore = GetHandScore(dealerHandIndex);
            }

            UpdateText(finalDealerScoreTextIndex, dealerScore.ToString());
            UpdateText(finalPlayerScoreTextIndex, playerScore.ToString(), GameStateChangeTime.Long);


            HideAllTexts(GameStateChangeTime.Instant);

            PaytableResult result = GetRoundResult(playerScore, dealerScore);
            ApplyBetMultiplierFromPaytable((int)result);
            EndRound((int)result);
        }
        else
        {
            UpdateText(bustOverNumberTextIndex, currentScoreLimit.ToString());
            UpdateText(yourScoreNumberTextIndex, playerScore.ToString());

            ShowAllTexts(GameStateChangeTime.Instant);
            HideText(finalDealerScoreTextIndex);
            HideText(finalPlayerScoreTextIndex);
            ShowAllButtons(GameStateChangeTime.Short);
        }
    }


    private PaytableResult GetRoundResult(int playerScore, int dealerScore)
    {
        if (playerScore > currentScoreLimit)
        {
            return PaytableResult.PlayerBust;
        }

        if (dealerScore > currentScoreLimit)
        {
            return PaytableResult.PlayerWin;
        }

        if (playerScore < dealerScore)
        {
            return PaytableResult.PlayerLoseNoBust;
        }

        if (playerScore == dealerScore)
        {
            return PaytableResult.PlayerDealerTie;
        }

        return PaytableResult.PlayerWin;
    }

    private int GetHandScore(int handIndex)
    {
        return HandAnalysis.GetHandScore(hands[handIndex].Cards, gameRulesBlackJack.JackScoreValue,
        gameRulesBlackJack.QueenScoreValue, gameRulesBlackJack.KingScoreValue,
        gameRulesBlackJack.AceScoreValueHigh, gameRulesBlackJack.AceScoreValueLow,
        currentScoreLimit);
    }

    /// <summary>
    /// In Blackjack, when the player selects a card, it does nothing.
    /// </summary>
    /// <param name="card">The card the player selected.</param>
    public override void PlayerSelectCard(Card card)
    {

    }

    public override PaytableDataBase GetPaytable()
    {
        return gameRulesBlackJack.paytableData;
    }
}
