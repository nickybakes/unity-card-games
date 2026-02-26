using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManagerBase : MonoBehaviour
{
    [SerializeField] private UnityEvent<List<GameStateChange>> finishTurnEvent;

    protected GameRulesBase gameRules;
    protected List<Deck> decks;
    protected List<Hand> hands;

    protected List<GameStateChange> changesThisTurn;

    protected PaytableDataBase paytable;


    protected bool gameStarted = false;



    public virtual void LoadGameRules(GameRulesBase _gameRules)
    {
        gameRules = _gameRules;
        changesThisTurn = new List<GameStateChange>();
        InitGame();
    }

    public virtual void InitGame()
    {
        SetupHands();
        BeginBetting();
        SubmitChanges();
    }

    public virtual void StartRound()
    {
        ResetDecks();
    }

    public virtual void SubmitChanges()
    {
        finishTurnEvent.Invoke(changesThisTurn);
        changesThisTurn = new List<GameStateChange>();
    }

    public void ResetDecks()
    {
        decks = new List<Deck>(gameRules.Decks.Count);
        foreach (DeckData deckData in gameRules.Decks)
        {
            Deck deck = new Deck(deckData);
            decks.Add(deck);
            deck.ShuffleDeck();
        }
    }

    public void SetupHands()
    {
        hands = new List<Hand>(gameRules.NumberOfHands);

        for (int i = 0; i < gameRules.NumberOfHands; i++)
        {
            hands.Add(new Hand());
        }
    }

    public void DrawCardsToHand(int deckIndex, int handIndex, int numCards)
    {
        for (int i = 0; i < numCards; i++)
        {
            if (decks[deckIndex].NumberOfCardsLeft() > 0)
            {
                Card card = decks[deckIndex].DrawCard();
                hands[handIndex].AddCard(card);
                changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardMove, GameBoardTarget.Deck, deckIndex, GameBoardTarget.Hand, handIndex, card));
            }
        }
    }

    public void DiscardCardsFromHand(List<Card> cardsToDiscard, int handIndex)
    {
        cardsToDiscard = new List<Card>(cardsToDiscard);

        for (int i = 0; i < cardsToDiscard.Count; i++)
        {
            changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardMove, GameBoardTarget.Hand, handIndex, GameBoardTarget.Discard, 0, cardsToDiscard[i]));
            hands[handIndex].RemoveCard(cardsToDiscard[i]);
        }
    }

    public void DiscardAllCardsFromHand(int handIndex)
    {
        DiscardCardsFromHand(hands[handIndex].Cards, handIndex);
    }

    public void UnholdAllCardsInHand(int handIndex)
    {
        List<Card> heldCards = hands[handIndex].GetHeldCards();

        for (int i = 0; i < heldCards.Count; i++)
        {
            heldCards[i].InvertHeld();
            GameStateChangeTime time = GameStateChangeTime.Instant;
            if (i == heldCards.Count - 1)
                time = GameStateChangeTime.Medium;
            changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardUpdate, heldCards[i], time));
        }
    }

    public void ReplaceCardsInHand(List<Card> cardsToReplace, int handIndex, int deckIndex)
    {
        List<int> cardIndices = new List<int>(cardsToReplace.Count);

        foreach (Card card in cardsToReplace)
        {
            cardIndices.Add(hands[handIndex].Cards.IndexOf(card));
        }

        List<GameStateChange> dicardChanges = new List<GameStateChange>();
        List<GameStateChange> drawChanges = new List<GameStateChange>();

        for (int i = 0; i < cardIndices.Count; i++)
        {
            if (decks[deckIndex].NumberOfCardsLeft() > 0)
            {
                Card card = decks[deckIndex].DrawCard();
                hands[handIndex].ReplaceCardAt(cardIndices[i], card);
                dicardChanges.Add(new GameStateChange(GameStateChangeType.CardMove, GameBoardTarget.Hand, handIndex, GameBoardTarget.Discard, 0, cardsToReplace[i]));
                drawChanges.Add(new GameStateChange(GameStateChangeType.CardMove, GameBoardTarget.Deck, deckIndex, GameBoardTarget.Hand, handIndex, card, GameStateChangeTime.Short, cardIndices[i]));
            }
            else
            {
                break;
            }
        }

        changesThisTurn.AddRange(dicardChanges);
        changesThisTurn.AddRange(drawChanges);
    }

    public void MoveCardsToAnotherHand(List<Card> cardsToMove, int fromHandIndex, int toHandIndex)
    {
        foreach (Card card in cardsToMove)
        {
            hands[fromHandIndex].RemoveCard(card);
            hands[toHandIndex].AddCard(card);
            changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardMove, GameBoardTarget.Hand, fromHandIndex, GameBoardTarget.Hand, toHandIndex, card));
        }
    }

    public void UpdateText(int textIndex, string text)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.TextUpdate, textIndex, text, GameStateChangeTime.Instant));
    }

    public void HideButton(int buttonIndex, GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.HideButton, buttonIndex, changeTime));
    }

    public void ShowButton(int buttonIndex, GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.ShowButton, buttonIndex, changeTime));
    }

    public void HideAllButtons(GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.HideButton, -1, changeTime));
    }

    public void ShowAllButtons(GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.ShowButton, -1, changeTime));
    }

    public void HideText(int textIndex, GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.HideText, textIndex, changeTime));
    }

    public void ShowText(int textIndex, GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.ShowText, textIndex, changeTime));
    }

    public void HideAllTexts(GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.HideText, -1, changeTime));
    }

    public void ShowAllTexts(GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.ShowText, -1, changeTime));
    }

    public virtual void PlayerSelectCard(Card card)
    {
        card.InvertFlipped();
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardUpdate, card));
        SubmitChanges();
    }

    public virtual void PlayerSelectButton(int index)
    {
    }

    public void EndRound(int paytableWinningRowIndex)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.ResultsPresentation, paytableWinningRowIndex));
        for (int i = 0; i < hands.Count; i++)
        {
            DiscardAllCardsFromHand(i);
        }
        BeginBetting();
    }

    public void BeginBetting()
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.BeginBetting));
    }

    public virtual PaytableDataBase GetPaytable()
    {
        return paytable;
    }
}
