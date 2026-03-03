using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBlackJack : GameManagerBase
{

    protected GameRulesBlackJack gameRulesBlackJack;

    public GameRulesBlackJack gameRulesOverride;

    [Header("Game Elements")]
    [SerializeField] private int dealerHandIndex;


    [Header("View Elements")]
    [SerializeField] private int hitButtonIndex;
    [SerializeField] private int standButtonIndex;
    [SerializeField] private int bustOverNumberTextIndex;
    [SerializeField] private int yourScoreNumberTextIndex;

    [Header("Test Data/Cheats")]
    [SerializeField] private List<Card> testPlayerHand;


    private void Start()
    {
        LoadGameRules(gameRulesOverride);
    }

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
        UpdateText(bustOverNumberTextIndex, gameRulesBlackJack.BaseScoreLimit.ToString());

#if UNITY_EDITOR
        if (testPlayerHand != null && testPlayerHand.Count > 0)
        {
            foreach (Card card in testPlayerHand)
            {
                hands[0].AddCard(card);
                changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardMove, GameBoardTarget.Deck, 0, GameBoardTarget.Hand, 0, card));
            }
        }
        else
        {
            DealHands();
        }
#else
            DealHands();
#endif

        // int winningPokerHandIndex = GetWinningPokerHandIndex(0);

        // if (winningPokerHandIndex != -1)
        // {
        //     SetAndShowResultPreview(winningPokerHandIndex);
        // }
        ShowAllTexts(GameStateChangeTime.Instant);
        ShowAllButtons(GameStateChangeTime.Short);

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
            // HideResultPreview();
            // HideAllTexts(GameStateChangeTime.Instant);
            // HideAllButtons(GameStateChangeTime.Medium);

            // List<Card> unheldCards = hands[0].GetUnheldCards();
            // ReplaceCardsInHand(unheldCards, 0, 0);

            // int winningPokerHandIndex = GetWinningPokerHandIndex(0);
            // if (drawsLeft == 0)
            // {
            //     UnholdAllCardsInHand(0);
            //     ApplyBetMultiplierFromPaytable(winningPokerHandIndex);
            //     EndRound(winningPokerHandIndex);
            // }
            // else
            // {
            //     if (winningPokerHandIndex != -1)
            //     {
            //         SetAndShowResultPreview(winningPokerHandIndex);
            //     }
            //     ShowAllTexts(GameStateChangeTime.Instant);
            //     ShowAllButtons(GameStateChangeTime.Medium);
            // }

            SubmitChanges();
        }
        else if (index == standButtonIndex)
        {

        }
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
