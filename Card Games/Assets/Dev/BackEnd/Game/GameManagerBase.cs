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

    protected bool gameStarted = false;


    public virtual void LoadGameRules(GameRulesBase _gameRules)
    {
        gameRules = _gameRules;
        SetupHands();
    }

    public virtual void StartGame()
    {
        Debug.Log("Start Game");
        StartRound();
    }

    public virtual void StartRound()
    {
        ResetDecks();
        StartNewTurn();
    }

    public virtual void StartNewTurn()
    {
        changesThisTurn = new List<GameStateChange>();
    }

    public virtual void FinishTurn()
    {
        finishTurnEvent.Invoke(changesThisTurn);
        StartNewTurn();
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
        Card[] newCards = new Card[numCards];
        for (int i = 0; i < numCards; i++)
        {
            Card card = decks[deckIndex].DrawCard();
            newCards[i] = card;
            hands[handIndex].AddCard(card);
            changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardMove, GameBoardTarget.Deck, deckIndex, GameBoardTarget.Hand, handIndex, card));
        }
    }

    public void DiscardCardsFromHand(List<Card> cardsToDiscard, int handIndex)
    {
        for (int i = 0; i < cardsToDiscard.Count; i++)
        {
            changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardMove, GameBoardTarget.Hand, handIndex, GameBoardTarget.Discard, 0, cardsToDiscard[i]));
            hands[handIndex].RemoveCard(cardsToDiscard[i]);
        }
    }

    public virtual void PlayerSelectCard(Card card)
    {
        card.Flip();
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardUpdate, card));
        FinishTurn();
    }

    public void DiscardAllCardsFromHand(int handIndex)
    {
        DiscardCardsFromHand(hands[handIndex].Cards, handIndex);
    }

    public void ReplaceCardsInHand(List<Card> cardsToReplace, int handIndex)
    {

    }

    public void MoveCardsToAnotherHand(List<Card> cardsToMove, int fromHandIndex, int tohandIndex)
    {

    }


    public virtual void DealHand()
    {

    }

    public virtual void DiscardHand()
    {

    }

    public virtual void AfterHandDealt()
    {

    }
}
