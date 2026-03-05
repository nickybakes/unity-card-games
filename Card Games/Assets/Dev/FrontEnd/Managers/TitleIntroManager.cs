using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The intro animation of the game actually uses the in-game elements for rendering cards.
/// This Manager tells the View Manager to display cards.
/// </summary>
public class TitleIntroManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the View Manager that will display the cards.
    /// </summary>
    [SerializeField] private GameViewManager viewManager;

    /// <summary>
    /// Whether to skip the title into in the Editor.
    /// </summary>
    [SerializeField] private bool skipTitleAnimationInEditor;

    /// <summary>
    /// The deck data to draw cards from.
    /// </summary>
    [SerializeField] private DeckData deckData;

    /// <summary>
    /// How many cards to draw.
    /// </summary>
    [SerializeField] private int numberOfCardsToDraw = 30;

    /// <summary>
    /// The deck that will be drawn from.
    /// </summary>
    private Deck deck;

    /// <summary>
    /// The list of game state changes for controlling the intro animation.
    /// </summary>
    private List<GameStateChange> gameStateChanges;

    /// <summary>
    /// On Start, skip the animation if necessary, otherwise initialize data for the animation.
    /// </summary>
    void Start()
    {
#if UNITY_EDITOR
        if (skipTitleAnimationInEditor)
        {

            TitleAnimationFinished();
            return;
        }
#endif

        ResetDeck();
        gameStateChanges = new List<GameStateChange>();
        DrawCardsToHand();
    }

    /// <summary>
    /// Makes a new deck and shuffles it.
    /// </summary>
    private void ResetDeck()
    {
        deck = new Deck(deckData);
        deck.ShuffleDeck();
    }

    /// <summary>
    /// Draws cards with a random game time interval and index position in the hand.
    /// </summary>
    private void DrawCardsToHand()
    {
        for (int i = 0; i < numberOfCardsToDraw; i++)
        {
            if (deck.NumberOfCardsLeft() > 0)
            {
                Card card = deck.DrawCard();
                int randomTime = Random.Range(1, 4);
                gameStateChanges.Add(new GameStateChange(GameStateChangeType.CardMove, GameBoardTarget.Deck, 0, GameBoardTarget.Hand, 0, card, (GameStateChangeTime)randomTime, Random.Range(0, 30)));
            }
        }
    }

    /// <summary>
    /// Send the compiled list of game state changes to the view manager.
    /// </summary>
    public void SendGameChangesToViewManager()
    {
        viewManager.CaptureAndDisplayGameChanges(gameStateChanges);
    }

    /// <summary>
    /// When the title animation is finished playing, transition in to the Menu.
    /// </summary>
    public void TitleAnimationFinished()
    {
        GameCollectionManager.collection.LoadGameMenuScene();
    }
}
