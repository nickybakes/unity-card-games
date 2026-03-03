using System.Collections.Generic;
using UnityEngine;

public class TitleIntroManager : MonoBehaviour
{

    [SerializeField] private GameViewManager viewManager;
    [SerializeField] private DeckData deckData;
    [SerializeField] private int numberOfCardsToDraw = 30;

    private Deck deck;

    private List<GameStateChange> gameStateChanges;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetDeck();
        gameStateChanges = new List<GameStateChange>();
        DrawCardsToHand();
    }

    private void ResetDeck()
    {
        deck = new Deck(deckData);
        deck.ShuffleDeck();
    }

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

    public void SendGameChangesToViewManager()
    {
        viewManager.CaptureAndDisplayGameChanges(gameStateChanges);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TitleAnimationFinished()
    {
        AppManager.app.SwitchToScene(SceneIndex.BlackJack);
    }
}
