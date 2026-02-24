using System.Collections.Generic;
using UnityEngine;

public class GameViewManager : MonoBehaviour
{

    [SerializeField] private GameManagerBase gameManager;

    [SerializeField] private List<HandDisplay> handDisplays;
    [SerializeField] private List<DeckDisplay> deckDisplays;
    [SerializeField] private DisplayPool cardDisplayPool;
    [SerializeField] private RectTransform discardPileTransform;

    [SerializeField] private float timeBetweenStandardGameChangeAction = .2f;

    [SerializeField] private float timeBetweenLongGameChangeAction = 1f;

    private float timeForCurrentChange;
    private float currentChangeTime;


    private List<GameStateChange> changesThisTurn;
    private int currentChangeIndex;

    private Dictionary<Card, CardDisplay> cardToCardDisplayReferences;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        changesThisTurn = new List<GameStateChange>();
        cardToCardDisplayReferences = new Dictionary<Card, CardDisplay>();
    }

    public void ButtonSelected(int index)
    {
        if (currentChangeIndex >= changesThisTurn.Count)
        {
            Debug.Log("Button Clicked: " + index);
            gameManager.PlayerSelectButton(index);
        }
    }

    public void CardSelected(Card card)
    {
        if (currentChangeIndex >= changesThisTurn.Count)
        {
            gameManager.PlayerSelectCard(card);
        }
    }

    public void CaptureAndDisplayGameChanges(List<GameStateChange> changes)
    {
        if (currentChangeIndex >= changesThisTurn.Count)
        {
            changesThisTurn = changes;
            currentChangeIndex = 0;
        }
        else
        {
            changesThisTurn.AddRange(changes);
        }

        currentChangeTime = timeForCurrentChange;
    }

    public void DrawCardToHand(Card card, int deckIndex, int handIndex, int indexInHand = -1)
    {
        CardDisplay display = (CardDisplay)cardDisplayPool.GetDisplay();
        cardToCardDisplayReferences.Add(card, display);
        display.DisplayCard(card, deckDisplays[deckIndex].GetRect(), true);
        display.SetStartTransform(deckDisplays[deckIndex].GetRect());
        display.StartTraveling();
        handDisplays[handIndex].AddCardDisplayToHand(display, indexInHand);
    }

    public void DiscardCardFromHand(Card card, int handIndex)
    {
        CardDisplay display = cardToCardDisplayReferences.GetValueOrDefault(card);
        handDisplays[handIndex].RemoveCardDisplayFromHand(display);
        display.TravelToTransform(discardPileTransform, cardDisplayPool.RemoveDisplay);
        display.FlipOverride(true);
    }

    public void MoveCardToAnotherHand(Card card, int fromHandIndex, int toHandIndex)
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentChangeTime += Time.deltaTime;
        while (currentChangeTime >= timeForCurrentChange && currentChangeIndex < changesThisTurn.Count)
        {
            ParseGameStateChange(changesThisTurn[currentChangeIndex]);
            currentChangeTime = 0;
            currentChangeIndex++;

            if (currentChangeIndex < changesThisTurn.Count)
            {
                switch (changesThisTurn[currentChangeIndex].ChangeTime)
                {
                    case GameStateChangeTime.Instant:
                        timeForCurrentChange = 0;
                        break;
                    case GameStateChangeTime.Standard:
                        timeForCurrentChange = timeBetweenStandardGameChangeAction;
                        break;
                    case GameStateChangeTime.Long:
                        timeForCurrentChange = timeBetweenLongGameChangeAction;
                        break;
                }
            }
        }
    }

    private void ParseGameStateChange(GameStateChange change)
    {
        switch (change.Type)
        {
            case GameStateChangeType.CardMove:
                if (change.From == GameBoardTarget.Deck && change.To == GameBoardTarget.Hand)
                {
                    DrawCardToHand(change.Card, change.FromIndex, change.ToIndex, change.IndexInHand);
                }

                if (change.To == GameBoardTarget.Discard)
                {
                    DiscardCardFromHand(change.Card, change.FromIndex);
                }
                break;


            case GameStateChangeType.CardUpdate:
                CardDisplay cardDisplay = cardToCardDisplayReferences.GetValueOrDefault(change.Card);
                cardDisplay.UpdateFlip();
                cardDisplay.UpdateHeld();
                break;
        }
    }
}
