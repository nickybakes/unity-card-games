using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game scripting for Blackjack games.
/// </summary>
public class GameManagerBlackJack : GameManagerBase
{

    /// <summary>
    /// A result for Blackjack a game, tied to row indices o nthe paytable.
    /// </summary>
    enum PaytableResult
    {
        PlayerBust,
        PlayerLoseNoBust,
        PlayerDealerTie,
        PlayerWin
    }

    /// <summary>
    /// Blackjack game rules.
    /// </summary>
    protected GameRulesBlackJack gameRulesBlackJack;

    /// <summary>
    /// The index of the dealer's hand.
    /// </summary>
    [Header("Game Elements")]
    [SerializeField] private int dealerHandIndex;

    /// <summary>
    /// The index of the Hit button.
    /// </summary>
    [Header("View Elements")]
    [SerializeField] private int hitButtonIndex;

    /// <summary>
    /// The index of the Stand button.
    /// </summary>
    [SerializeField] private int standButtonIndex;

    /// <summary>
    /// The index of the Bust text display.
    /// </summary>
    [SerializeField] private int bustOverNumberTextIndex;

    /// <summary>
    /// The index of the Player's Score text display.
    /// </summary>
    [SerializeField] private int yourScoreNumberTextIndex;

    /// <summary>
    /// The index of the Final Player's Score text display.
    /// </summary>
    [SerializeField] private int finalPlayerScoreTextIndex;

    /// <summary>
    /// The index of the Final Dealer's Score text display.
    /// </summary>
    [SerializeField] private int finalDealerScoreTextIndex;

    /// <summary>
    /// The index of the Dealer Hit text display.
    /// </summary>
    [SerializeField] private int dealerHitWhenUnderTextIndex;

    /// <summary>
    /// Override the player's hand. Only useable in Editor.
    /// </summary>
    [Header("Test Data/Cheats")]
    [SerializeField] private List<Card> testPlayerHand;

    /// <summary>
    /// The current score limit for busting.
    /// </summary>
    private int currentScoreLimit;

    /// <summary>
    /// The current dealer hit threshold.
    /// </summary>
    private int currentDealerHitWhenUnder;

    /// <summary>
    /// Loads a BlackJack GameRules scriptable object and starts the game with these rules.
    /// </summary>
    /// <param name="_gameRules">The BlackJack GameRules to load.</param>
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

    /// <summary>
    /// Initializes the basic data, hides all ingame text and buttons, and opens the betting menu.
    /// </summary>
    public override void InitGame()
    {
        SetupHands();
        HideAllTexts(GameStateChangeTime.Instant);
        HideAllButtons(GameStateChangeTime.Instant);
        BeginBetting();
        SubmitChanges();
    }

    /// <summary>
    /// Deals the player's and dealer's hands for the round. Then checks if the player has already hit the score limit.
    /// </summary>
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

    /// <summary>
    /// Will Hit or Stand and then check if the player's game is over.
    /// </summary>
    /// <param name="index">The index of the button that was submitted.</param>
    public override void PlayerSubmitButton(int index)
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

    /// <summary>
    /// Checks if the player has busted, if so, end the game and reveal the dealer's hand.
    /// </summary>
    /// <param name="forceGameOver">Force the game over, such as if the player Stands.</param>
    private void CheckScoresForGameOver(bool forceGameOver)
    {
        int playerScore = GetHandScore(0);

        // Game is over if the player busts, Stands, or score is the limit.
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

            // If the player didnt bust, the dealer now needs to draw until they are over the threshold.
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
            //if the player's game is not over, then show the buttons and keep playing.
            UpdateText(bustOverNumberTextIndex, currentScoreLimit.ToString());
            UpdateText(yourScoreNumberTextIndex, playerScore.ToString());

            ShowAllTexts(GameStateChangeTime.Instant);
            HideText(finalDealerScoreTextIndex);
            HideText(finalPlayerScoreTextIndex);
            ShowAllButtons(GameStateChangeTime.Short);
        }
    }

    /// <summary>
    /// Compares the Player's score to the Dealer's score and the bust limit to determine the final result.
    /// </summary>
    /// <param name="playerScore">The player's score.</param>
    /// <param name="dealerScore">The dealer's score.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Uses the game rules to calculate a hand's Blackjack score.
    /// </summary>
    /// <param name="handIndex">The index of the hand to check.</param>
    /// <returns>The calculated score of the hand.</returns>
    private int GetHandScore(int handIndex)
    {
        return HandAnalysis.GetHandScore(hands[handIndex].Cards, gameRulesBlackJack.JackScoreValue,
        gameRulesBlackJack.QueenScoreValue, gameRulesBlackJack.KingScoreValue,
        gameRulesBlackJack.AceScoreValueHigh, gameRulesBlackJack.AceScoreValueLow,
        currentScoreLimit);
    }

    /// <summary>
    /// Overrive, gets the Blackjack paytable.
    /// </summary>
    /// <returns>The Blackjack paytable.</returns>
    public override PaytableDataBase GetPaytable()
    {
        return gameRulesBlackJack.paytableData;
    }
}
