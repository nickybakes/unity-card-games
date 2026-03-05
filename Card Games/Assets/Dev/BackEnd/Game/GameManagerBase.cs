using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A base class for managing the data of a game.
/// </summary>
public class GameManagerBase : MonoBehaviour
{
    /// <summary>
    /// Sends out the current list of game changes in this event.
    /// </summary>
    [SerializeField] private UnityEvent<List<GameStateChange>> submitChangesEvent;

    /// <summary>
    /// The base game rules that are loaded.
    /// </summary>
    protected GameRulesBase gameRules;

    /// <summary>
    /// Current Decks in play.
    /// </summary>
    protected List<Deck> decks;

    /// <summary>
    /// Currently hands in play.
    /// </summary>
    protected List<Hand> hands;

    /// <summary>
    /// List of changes to the game that have happened in this turn/interval.
    /// </summary>
    protected List<GameStateChange> changesThisTurn;

    /// <summary>
    /// The base paytable in play.
    /// </summary>
    protected PaytableDataBase paytable;

    /// <summary>
    /// Invokes the Submit Changes Event and starts a new list of changes.
    /// </summary>
    public virtual void SubmitChanges()
    {
        submitChangesEvent.Invoke(changesThisTurn);
        changesThisTurn = new List<GameStateChange>();
    }

    #region Game Initialization

    /// <summary>
    /// Loads a GameRules scriptable object and starts the game with these rules.
    /// </summary>
    /// <param name="_gameRules">The GameRules to load.</param>
    public virtual void LoadGameRules(GameRulesBase _gameRules)
    {
        gameRules = _gameRules;
        changesThisTurn = new List<GameStateChange>
        {
            new GameStateChange(GameStateChangeType.GameRulesLoaded)
        };
        SubmitChanges();
        InitGame();
    }

    /// <summary>
    /// Initializes the basic data needed for the game to run and opens the betting menu.
    /// </summary>
    public virtual void InitGame()
    {
        SetupHands();
        BeginBetting();
        SubmitChanges();
    }

    /// <summary>
    /// Resets game values to whatever they need to be at the start of a round.
    /// </summary>
    public virtual void StartRound()
    {
        ResetDecks();
    }

    /// <summary>
    /// Resets all the decks the game needs to play following the game rules.
    /// </summary>
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

    /// <summary>
    /// Creates new Hand objects following the game rules.
    /// </summary>
    public void SetupHands()
    {
        hands = new List<Hand>(gameRules.NumberOfHands);

        for (int i = 0; i < gameRules.NumberOfHands; i++)
        {
            hands.Add(new Hand());
        }
    }

    #endregion

    #region Cards

    /// <summary>
    /// Draws cards fro ma specific deck to a specific hand.
    /// </summary>
    /// <param name="deckIndex">The index of the deck to draw from.</param>
    /// <param name="handIndex">The index of the hand to draw to.</param>
    /// <param name="numCards">The number of cards to draw.</param>
    /// <param name="flipped">Optional, whether the cards should be flipped over in hand.</param>
    /// <param name="changeTime">Optional override for how long in game this change should take.</param>
    public void DrawCardsToHand(int deckIndex, int handIndex, int numCards, bool flipped = false, GameStateChangeTime changeTime = GameStateChangeTime.Short)
    {
        for (int i = 0; i < numCards; i++)
        {
            if (decks[deckIndex].NumberOfCardsLeft() > 0)
            {
                Card card = decks[deckIndex].DrawCard();
                hands[handIndex].AddCard(card);
                if (flipped)
                    card.InvertFlipped();
                changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardMove, GameBoardTarget.Deck, deckIndex, GameBoardTarget.Hand, handIndex, card, changeTime));
            }
        }
    }

    /// <summary>
    /// Discards a list of cards from a specific hand.
    /// </summary>
    /// <param name="cardsToDiscard">The list of cards to discard.</param>
    /// <param name="handIndex">The index of the hand to discard from.</param>
    public void DiscardCardsFromHand(List<Card> cardsToDiscard, int handIndex)
    {
        cardsToDiscard = new List<Card>(cardsToDiscard);

        for (int i = 0; i < cardsToDiscard.Count; i++)
        {
            changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardMove, GameBoardTarget.Hand, handIndex, GameBoardTarget.Discard, 0, cardsToDiscard[i]));
            hands[handIndex].RemoveCard(cardsToDiscard[i]);
        }
    }

    /// <summary>
    /// Discards all cards from a specific hand.
    /// </summary>
    /// <param name="handIndex">The index of the hand to discard from.</param>
    public void DiscardAllCardsFromHand(int handIndex)
    {
        DiscardCardsFromHand(hands[handIndex].Cards, handIndex);
    }

    /// <summary>
    /// Makes all cards in a hand no longer be "held".
    /// </summary>
    /// <param name="handIndex">The index of the hand to make changes to.</param>
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

    /// <summary>
    /// Makes all cards in a hand no longer be flipped.
    /// </summary>
    /// <param name="handIndex">The index of the hand to make changes to.</param>
    public void UnflipAllCardsInHand(int handIndex)
    {
        List<Card> flippedCards = hands[handIndex].GetFlippedCards();

        for (int i = 0; i < flippedCards.Count; i++)
        {
            flippedCards[i].InvertFlipped();
            GameStateChangeTime time = GameStateChangeTime.Short;
            if (i == flippedCards.Count - 1)
                time = GameStateChangeTime.Medium;
            changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardUpdate, flippedCards[i], time));
        }
    }

    /// <summary>
    /// Discards a list of cards from a specific hand, then draws the same amount of cards to the hand from a specific deck.
    /// </summary>
    /// <param name="cardsToReplace">The list of cards to discard.</param>
    /// <param name="handIndex">The index of the hand to make changes to.</param>
    /// <param name="deckIndex">The index of the deck to draw from.</param>
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

    /// <summary>
    /// Moves a list of cards from one hand to another.
    /// </summary>
    /// <param name="cardsToMove">The list of cards to move.</param>
    /// <param name="fromHandIndex">The index of the hand to move from.</param>
    /// <param name="toHandIndex">The index of the hand to move to.</param>
    public void MoveCardsToAnotherHand(List<Card> cardsToMove, int fromHandIndex, int toHandIndex)
    {
        foreach (Card card in cardsToMove)
        {
            hands[fromHandIndex].RemoveCard(card);
            hands[toHandIndex].AddCard(card);
            changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardMove, GameBoardTarget.Hand, fromHandIndex, GameBoardTarget.Hand, toHandIndex, card));
        }
    }

    /// <summary>
    /// Immediately inverts the flipped status of a specific card and submits the changes.
    /// </summary>
    /// <param name="card">The card to change.</param>
    public void InvertCardFlip(Card card)
    {
        card.InvertFlipped();
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardUpdate, card));
        SubmitChanges();
    }

    /// <summary>
    /// Immediately inverts the held status of a specific card and submits the changes.
    /// </summary>
    /// <param name="card">The card to change.</param>
    public void InvertCardHeld(Card card)
    {
        card.InvertHeld();
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.CardUpdate, card));
        SubmitChanges();
    }

    #endregion

    #region Game View Elements

    /// <summary>
    /// Hides a specific button in the game view. Use index -1 to hide all game buttons at the same time.
    /// </summary>
    /// <param name="buttonIndex">The index of the button to hide. Use -1 to hide all game buttons at the same time.</param>
    /// <param name="changeTime">Optional override for how long in game this change should take.</param>
    public void HideButton(int buttonIndex, GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.HideButton, buttonIndex, changeTime));
    }

    /// <summary>
    /// Shows a specific button in the game view. Use index -1 to show all game buttons at the same time.
    /// </summary>
    /// <param name="buttonIndex">The index of the button to show. Use -1 to show all game buttons at the same time.</param>
    /// <param name="changeTime">Optional override for how long in game this change should take.</param>
    public void ShowButton(int buttonIndex, GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.ShowButton, buttonIndex, changeTime));
    }

    /// <summary>
    /// Hides all game buttons at the same time.
    /// </summary>
    /// <param name="changeTime">Optional override for how long in game this change should take.</param>
    public void HideAllButtons(GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.HideButton, -1, changeTime));
    }

    /// <summary>
    /// Shows all game buttons at the same time.
    /// </summary>
    /// <param name="changeTime">Optional override for how long in game this change should take.</param>
    public void ShowAllButtons(GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.ShowButton, -1, changeTime));
    }

    /// <summary>
    /// Changes the text of an in-game text display. Should mainly be used to show player scores and other number values.
    /// </summary>
    /// <param name="textIndex">The index of the text display.</param>
    /// <param name="text">The new text to show.</param>
    /// <param name="changeTime">Optional override for how long in game this change should take.</param>
    public void UpdateText(int textIndex, string text, GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.TextUpdate, textIndex, text, changeTime));
    }

    /// <summary>
    /// Hides a specific text display in the game view. Use index -1 to hide all game text displays at the same time.
    /// </summary>
    /// <param name="buttonIndex">The index of the text display to hide. Use -1 to hide all game text displays at the same time.</param>
    /// <param name="changeTime">Optional override for how long in game this change should take.</param>
    public void HideText(int textIndex, GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.HideText, textIndex, changeTime));
    }

    /// <summary>
    /// Shows a specific text display in the game view. Use index -1 to show all game text displays at the same time.
    /// </summary>
    /// <param name="buttonIndex">The index of the text display to show. Use -1 to show all game text displays at the same time.</param>
    /// <param name="changeTime">Optional override for how long in game this change should take.</param>
    public void ShowText(int textIndex, GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.ShowText, textIndex, changeTime));
    }

    /// <summary>
    /// Hides all game text displays at the same time.
    /// </summary>
    /// <param name="changeTime">Optional override for how long in game this change should take.</param>
    public void HideAllTexts(GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.HideText, -1, changeTime));
    }

    /// <summary>
    /// Shows all game text displays at the same time.
    /// </summary>
    /// <param name="changeTime">Optional override for how long in game this change should take.</param>
    public void ShowAllTexts(GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.ShowText, -1, changeTime));
    }

    #endregion

    #region Player Interaction

    /// <summary>
    /// Overridable function of what to do when a player submits (clicks) a card.
    /// </summary>
    /// <param name="card">The card that was submitted.</param>
    public virtual void PlayerSubmitCard(Card card)
    {
    }

    /// <summary>
    /// Overridable function of what to do when a player submits (clicks) a button.
    /// </summary>
    /// <param name="card">The index of the button that was submitted.</param>
    public virtual void PlayerSubmitButton(int index)
    {
    }

    #endregion

    #region Results, Awards, Betting

    /// <summary>
    /// Plays the Result Presentation based on the winnings paytable row, discards all hands, then opens the betting menu.
    /// </summary>
    /// <param name="paytableWinningRowIndex">The index of the winning paytable row. Use -1 if the player fully lost.</param>
    public void EndRound(int paytableWinningRowIndex)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.ResultPresentation, paytableWinningRowIndex));
        for (int i = 0; i < hands.Count; i++)
        {
            DiscardAllCardsFromHand(i);
        }
        BeginBetting();
    }

    /// <summary>
    /// Opens the betting menu.
    /// </summary>
    public void BeginBetting()
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.BeginBetting));
    }

    /// <summary>
    /// Awards the current user their winnings based on the Bet Multiplier of the winning paytable row, and the user's current bet.
    /// </summary>
    /// <param name="index"></param>
    protected void ApplyBetMultiplierFromPaytable(int index)
    {
        if (index == -1)
            return;

        UserManager.user.AwardWinnings(UserManager.user.CurrentBet * paytable.GetBetMultiplier(index));
    }

    /// <summary>
    /// Shows the in-game preview of the paytable winnings that player will get.
    /// </summary>
    /// <param name="index">The index of the currently winning paytable row.</param>
    public void SetAndShowResultPreview(int index)
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.SetAndShowResultPreview, index));
    }

    /// <summary>
    /// Hides the in-game result preview.
    /// </summary>
    public void HideResultPreview()
    {
        changesThisTurn.Add(new GameStateChange(GameStateChangeType.HideResultPreview));
    }

    /// <summary>
    /// Gets the current game's paytable.
    /// </summary>
    /// <returns>The current game's paytable.</returns>
    public virtual PaytableDataBase GetPaytable()
    {
        return paytable;
    }

    #endregion
}
