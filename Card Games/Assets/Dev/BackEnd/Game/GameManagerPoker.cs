using System.Collections.Generic;
using UnityEngine;

public class GameManagerPoker : GameManagerBase
{

    protected GameRulesPoker gameRulesPoker;

    public GameRulesPoker gameRulesOverrive;

    private int drawsLeft;

    private float timeInGame;

    private void Start()
    {
        LoadGameRules(gameRulesOverrive);
    }

    public override void LoadGameRules(GameRulesBase _gameRules)
    {
        gameRules = _gameRules;
        gameRulesPoker = (GameRulesPoker)_gameRules;
        SetupHands();
    }

    public override void StartGame()
    {
        Debug.Log("Start Game");
        StartRound();
    }

    public override void StartRound()
    {
        base.StartRound();
        DrawCardsToHand(0, 0, gameRulesPoker.HandSize);
        FinishTurn();
    }

    public void Update()
    {
        timeInGame += Time.deltaTime;
        if (timeInGame > 1 && !gameStarted)
        {
            gameStarted = true;
            StartGame();
        }
    }

    /// <summary>
    /// In Poker, when the player selected a card, it sets the card as "Held" (or Unheld if its already held)
    /// </summary>
    /// <param name="card">The card the player selected.</param>
    public override void PlayerSelectCard(Card card)
    {
        card.InvertHeld();
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardUpdate, card));
        FinishTurn();
    }

    public override void PlayerSelectButton(int index)
    {
        switch (index)
        {
            case 0:
                List<Card> unheldCards = hands[0].GetUnheldCards();
                ReplaceCardsInHand(unheldCards, 0, 0);
                // UnholdAllCardsInHand(0);
                // MoveCardsToAnotherHand()
                drawsLeft--;
                FinishTurn();
                break;
        }
    }
}
