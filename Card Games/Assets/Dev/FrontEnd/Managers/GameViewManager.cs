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

    }

    public void CardSelected(Card card)
    {
        gameManager.PlayerSelectCard(card);
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
        handDisplays[handIndex].AddCardDisplayToHand(display, indexInHand);
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
                break;


            case GameStateChangeType.CardUpdate:
                CardDisplay displayToUpdate = cardToCardDisplayReferences.GetValueOrDefault(change.Card);
                displayToUpdate.UpdateFlip();
                displayToUpdate.UpdateHeld();
                break;
        }
    }
}
