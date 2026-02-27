using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameViewManager : MonoBehaviour
{
    [SerializeField] private GameManagerBase gameManager;

    [SerializeField] private List<HandDisplay> handDisplays;
    [SerializeField] private List<DeckDisplay> deckDisplays;

    [SerializeField] private List<BetterButton> gameButtons;
    [SerializeField] private List<TextDisplay> textDisplays;

    [SerializeField] private DisplayPool cardDisplayPool;
    [SerializeField] private RectTransform discardPileTransform;
    [SerializeField] private ResultsDisplay resultsDisplay;
    [SerializeField] private BetPanel betPanel;
    [SerializeField] private PaytablePanel paytablePanel;

    [SerializeField] private GameObject uiRaycastShield;

    [SerializeField] private float timeBetweenShortGameChangeAction = .2f;
    [SerializeField] private float timeBetweenMediumGameChangeAction = .6f;

    [SerializeField] private float timeBetweenLongGameChangeAction = 1f;

    private float timeForCurrentChange;
    private float currentChangeTime;

    private List<GameStateChange> changesThisTurn;
    private int currentChangeIndex;

    private Dictionary<Card, CardDisplay> cardToCardDisplayReferences;

    private bool parsingGameChanges;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        changesThisTurn = new List<GameStateChange>();
        cardToCardDisplayReferences = new Dictionary<Card, CardDisplay>();
        parsingGameChanges = true;
        DisablePlayerInteractions();
    }

    public void ButtonSelected(int index)
    {
        EventSystem.current.SetSelectedGameObject(null);
        gameManager.PlayerSelectButton(index);
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

        DisablePlayerInteractions();

        currentChangeTime = timeForCurrentChange;
    }

    public void BetPlaced()
    {
        betPanel.MinimizePanel();
        paytablePanel.MinimizePanel();
        gameManager.StartRound();
    }

    public void UpdatePaytable()
    {
        paytablePanel.UpdatePaytableChosenBet();
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
        CardDisplay display = cardToCardDisplayReferences.GetValueOrDefault(card);
        handDisplays[fromHandIndex].RemoveCardDisplayFromHand(display);
        display.SetStartTransform(display.GetRect());
        handDisplays[toHandIndex].AddCardDisplayToHand(display);
        display.StartTraveling();
    }

    public void StartResultsPresentation(int paytableWinningRowIndex)
    {
        StopParsingGameChanges();
        if (paytableWinningRowIndex == -1)
            resultsDisplay.StartPresentation("", 0);
        else
        {
            paytablePanel.HighlightText(paytableWinningRowIndex);
            resultsDisplay.StartPresentation(gameManager.GetPaytable().GetRowName(paytableWinningRowIndex), gameManager.GetPaytable().GetBetMultiplier(paytableWinningRowIndex));
            betPanel.ShowUserWinnings();
        }
    }

    public void EndResultsPresentation()
    {
        paytablePanel.UnhighlightText();
        StartParsingGameChanges();
    }

    public void StartParsingGameChanges()
    {
        parsingGameChanges = true;
    }

    public void StopParsingGameChanges()
    {
        parsingGameChanges = false;
    }

    public void DisablePlayerInteractions()
    {
        uiRaycastShield.SetActive(true);
    }

    public void EnablePlayerInteractions()
    {
        uiRaycastShield.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (parsingGameChanges)
        {
            currentChangeTime += Time.deltaTime;
            while (currentChangeTime >= timeForCurrentChange && currentChangeIndex < changesThisTurn.Count && parsingGameChanges)
            {
                ParseGameStateChange(changesThisTurn[currentChangeIndex]);
                currentChangeTime = 0;

                switch (changesThisTurn[currentChangeIndex].ChangeTime)
                {
                    case GameStateChangeTime.Instant:
                        timeForCurrentChange = 0;
                        break;
                    case GameStateChangeTime.Short:
                        timeForCurrentChange = timeBetweenShortGameChangeAction;
                        break;
                    case GameStateChangeTime.Medium:
                        timeForCurrentChange = timeBetweenMediumGameChangeAction;
                        break;
                    case GameStateChangeTime.Long:
                        timeForCurrentChange = timeBetweenLongGameChangeAction;
                        break;
                }

                currentChangeIndex++;
                if (currentChangeIndex >= changesThisTurn.Count)
                {
                    EnablePlayerInteractions();
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

            case GameStateChangeType.HideButton:
                if (change.FromIndex == -1)
                    foreach (BetterButton button in gameButtons)
                        button.Hide();
                else
                    gameButtons[change.FromIndex].Hide();
                break;

            case GameStateChangeType.ShowButton:
                if (change.FromIndex == -1)
                    foreach (BetterButton button in gameButtons)
                        button.Show();
                else
                    gameButtons[change.FromIndex].Show();
                break;

            case GameStateChangeType.HideText:
                if (change.FromIndex == -1)
                    foreach (TextDisplay text in textDisplays)
                        text.Hide();
                else
                    textDisplays[change.FromIndex].Hide();
                break;

            case GameStateChangeType.ShowText:
                if (change.FromIndex == -1)
                    foreach (TextDisplay text in textDisplays)
                        text.Show();
                else
                    textDisplays[change.FromIndex].Show();
                break;

            case GameStateChangeType.TextUpdate:
                if (change.FromIndex == -1)
                    foreach (TextDisplay text in textDisplays)
                        text.SetText(change.Text);
                else
                    textDisplays[change.FromIndex].SetText(change.Text);
                break;

            case GameStateChangeType.CardUpdate:
                CardDisplay cardDisplay = cardToCardDisplayReferences.GetValueOrDefault(change.Card);
                cardDisplay.UpdateFlip();
                cardDisplay.UpdateHeld();
                break;

            case GameStateChangeType.ResultsPresentation:
                StartResultsPresentation(change.FromIndex);
                break;

            case GameStateChangeType.BeginBetting:
                paytablePanel.MaximizePanel();
                betPanel.MaximizePanel();
                break;

            case GameStateChangeType.GameRulesLoaded:
                paytablePanel.SetupPaytablePanel(gameManager.GetPaytable());
                break;
        }
    }
}
