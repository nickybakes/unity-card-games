using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game scripting for Poker games.
/// </summary>
public class GameManagerPoker : GameManagerBase
{

    /// <summary>
    /// Poker Gamerules.
    /// </summary>
    protected GameRulesPoker gameRulesPoker;

    /// <summary>
    /// The index of the Draw button.
    /// </summary>
    [Header("View Elements")]
    [SerializeField] private int drawButtonIndex;

    /// <summary>
    /// The index of the Draws Left text display.
    /// </summary>
    [SerializeField] private int drawsLeftNumberTextIndex;

    /// <summary>
    /// Override the player's hand. Only useable in Editor.
    /// </summary>
    [Header("Test Data/Cheats")]
    [SerializeField] private List<Card> testPlayerHand;

    /// <summary>
    /// The current number of draws left for the player.
    /// </summary>
    private int drawsLeft;

    /// <summary>
    /// Loads a Poker GameRules scriptable object and starts the game with these rules.
    /// </summary>
    /// <param name="_gameRules">The Poker GameRules to load.</param>
    public override void LoadGameRules(GameRulesBase _gameRules)
    {
        gameRules = _gameRules;
        gameRulesPoker = (GameRulesPoker)_gameRules;
        paytable = gameRulesPoker.paytableData;
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
        HideResultPreview();
        HideAllTexts(GameStateChangeTime.Instant);
        HideAllButtons(GameStateChangeTime.Instant);
        BeginBetting();
        SubmitChanges();
    }

    /// <summary>
    /// Deals the player's hand for the round, shows in game text and buttons, and shows the result preview.
    /// </summary>
    public override void StartRound()
    {
        ResetDecks();
        drawsLeft = gameRulesPoker.NumberOfDraws;
        UpdateText(drawsLeftNumberTextIndex, drawsLeft.ToString());

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
            DrawCardsToHand(0, 0, gameRulesPoker.HandSize);
        }
#else
        DrawCardsToHand(0, 0, gameRulesPoker.HandSize);
#endif

        int winningPokerHandIndex = GetWinningPokerHandIndex(0);

        if (winningPokerHandIndex != -1)
        {
            SetAndShowResultPreview(winningPokerHandIndex);
        }
        ShowAllTexts(GameStateChangeTime.Instant);
        ShowAllButtons(GameStateChangeTime.Short);

        SubmitChanges();
    }

    /// <summary>
    /// In Poker, when the player selects a card, it sets the card as "Held" (or Unheld if its already held)
    /// </summary>
    /// <param name="card">The card the player selected.</param>
    public override void PlayerSubmitCard(Card card)
    {
        InvertCardHeld(card);
    }

    /// <summary>
    /// When the player submits the Draw button, decrement the number of draws and possibly end the round.
    /// </summary>
    /// <param name="index">The index of the button that was submitted.</param>
    public override void PlayerSubmitButton(int index)
    {
        if (index == drawButtonIndex)
        {
            HideResultPreview();
            HideAllTexts(GameStateChangeTime.Instant);
            HideAllButtons(GameStateChangeTime.Medium);
            drawsLeft--;
            UpdateText(drawsLeftNumberTextIndex, drawsLeft.ToString());

            List<Card> unheldCards = hands[0].GetUnheldCards();
            ReplaceCardsInHand(unheldCards, 0, 0);

            int winningPokerHandIndex = GetWinningPokerHandIndex(0);

            // If no more draws are left, end the round.
            if (drawsLeft == 0)
            {
                UnholdAllCardsInHand(0);
                ApplyBetMultiplierFromPaytable(winningPokerHandIndex);
                EndRound(winningPokerHandIndex);
            }
            else
            {
                //If there are draws left, let the player keep playing.
                if (winningPokerHandIndex != -1)
                {
                    SetAndShowResultPreview(winningPokerHandIndex);
                }
                ShowAllTexts(GameStateChangeTime.Instant);
                ShowAllButtons(GameStateChangeTime.Medium);
            }

            SubmitChanges();
        }
    }

    /// <summary>
    /// Uses the Poker paytable to determine the highest ranking poker hand that the player has.
    /// </summary>
    /// <param name="handIndex">The index of the hand to check.</param>
    /// <returns>The index of the paytable row, -1 if the player does not have a winning hand.</returns>
    public int GetWinningPokerHandIndex(int handIndex)
    {
        for (int i = paytable.GetRowCount() - 1; i >= 0; i--)
        {
            if (HandAnalysis.AnalizeForPokerHand(hands[handIndex].Cards, gameRulesPoker.paytableData.GetPokerHand(i)))
            {
                return i;
            }
        }

        return -1;
    }

    public override PaytableDataBase GetPaytable()
    {
        return gameRulesPoker.paytableData;
    }
}
