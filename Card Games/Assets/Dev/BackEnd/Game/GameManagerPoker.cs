using System.Collections.Generic;
using UnityEngine;

public class GameManagerPoker : GameManagerBase
{

    protected GameRulesPoker gameRulesPoker;

    public GameRulesPoker gameRulesOverrive;

    [Header("View Elements")]
    [SerializeField] private int drawButtonIndex;
    [SerializeField] private int drawsLeftNumberTextIndex;

    [Header("Test Data/Cheats")]
    [SerializeField] private List<Card> testPlayerHand;


    private int drawsLeft;

    private void Start()
    {
        LoadGameRules(gameRulesOverrive);
    }

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

        ShowAllTexts(GameStateChangeTime.Instant);
        ShowAllButtons(GameStateChangeTime.Short);

        SubmitChanges();
    }

    /// <summary>
    /// In Poker, when the player selected a card, it sets the card as "Held" (or Unheld if its already held)
    /// </summary>
    /// <param name="card">The card the player selected.</param>
    public override void PlayerSelectCard(Card card)
    {
        card.InvertHeld();
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardUpdate, card));
        SubmitChanges();
    }

    public override void PlayerSelectButton(int index)
    {
        if (index == drawButtonIndex)
        {
            HideAllTexts(GameStateChangeTime.Instant);
            HideAllButtons(GameStateChangeTime.Medium);
            drawsLeft--;
            UpdateText(drawsLeftNumberTextIndex, drawsLeft.ToString());

            List<Card> unheldCards = hands[0].GetUnheldCards();
            ReplaceCardsInHand(unheldCards, 0, 0);

            if (drawsLeft == 0)
            {
                UnholdAllCardsInHand(0);
                int winningPokerHandIndex = GetWinningPokerHandIndex(0);
                ApplyBetMultiplierFromPaytable(winningPokerHandIndex);
                EndRound(winningPokerHandIndex);
            }
            else
            {
                ShowAllTexts(GameStateChangeTime.Instant);
                ShowAllButtons(GameStateChangeTime.Medium);
            }

            SubmitChanges();
        }
    }

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
