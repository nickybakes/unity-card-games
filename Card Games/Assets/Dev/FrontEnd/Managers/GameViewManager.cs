using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The Game View Manager uses data from the Game Manager to control game objects on screen.
/// </summary>
public class GameViewManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the Game Manager in the scene.
    /// </summary>
    [SerializeField] private GameManagerBase gameManager;

    /// <summary>
    /// References to Hand Displays in the scene.
    /// </summary>
    [SerializeField] private List<HandDisplay> handDisplays;

    /// <summary>
    /// References to Deck Displays in the scene.
    /// </summary>
    [SerializeField] private List<DeckDisplay> deckDisplays;

    /// <summary>
    /// References to game buttons in the scene.
    /// </summary>
    [SerializeField] private List<BetterButton> gameButtons;

    /// <summary>
    /// References ot game text displays in the scene.
    /// </summary>
    [SerializeField] private List<TextDisplay> textDisplays;

    /// <summary>
    /// The Visual Profile settings for how cards should be displayed.
    /// </summary>
    [SerializeField] private CardVisualProfile cardVisualProfile;

    /// <summary>
    /// Reference to the Card Display Pool in the scene.
    /// </summary>
    [SerializeField] private DisplayPool cardDisplayPool;

    /// <summary>
    /// Transform of where to discard cards to.
    /// </summary>
    [SerializeField] private RectTransform discardPileTransform;

    /// <summary>
    /// Reference to the text display to use as a Result Preview.
    /// </summary>
    [SerializeField] private TextDisplay resultPreviewDisplay;

    /// <summary>
    /// Reference to the Result Panel in the scene.
    /// </summary>
    [SerializeField] private ResultPanel resultPanel;

    /// <summary>
    /// Reference to the Betting Panel in the scene.
    /// </summary>
    [SerializeField] private BetPanel betPanel;

    /// <summary>
    /// Reference to the Paytable Panel in the scene.
    /// </summary>
    [SerializeField] private PaytablePanel paytablePanel;

    /// <summary>
    /// Reference to a full screen raycast target to block player interactions.
    /// </summary>
    [SerializeField] private GameObject uiRaycastShield;

    /// <summary>
    /// How long a "short" game action takes, in seconds.
    /// </summary>
    [SerializeField] private float timeBetweenShortGameChangeAction = .2f;

    /// <summary>
    /// How long a "medium" game action takes, in seconds.
    /// </summary>
    [SerializeField] private float timeBetweenMediumGameChangeAction = .6f;

    /// <summary>
    /// How long a "long" game action takes, in seconds.
    /// </summary>
    [SerializeField] private float timeBetweenLongGameChangeAction = 1f;

    /// <summary>
    /// How long the current game action should take, in seconds.
    /// </summary>
    private float timeForCurrentChange;

    /// <summary>
    /// How long the current game action has taken, in seconds.
    /// </summary>
    private float currentChangeTime;

    /// <summary>
    /// A queue of game changes that the view manager should run through and display.
    /// </summary>
    private List<GameStateChange> changesThisTurn;

    /// <summary>
    /// The index of the current game change that the view manager is on.
    /// </summary>
    private int currentChangeIndex;

    /// <summary>
    /// A mapping of Card objects to the Card Displays that represent them.
    /// </summary>
    private Dictionary<Card, CardDisplay> cardToCardDisplayReferences;

    /// <summary>
    /// Whether the view manager should be working its way through displaying game changes.
    /// </summary>
    private bool parsingGameChanges;


    /// <summary>
    /// Initialize lists and disable player interactions on Awake
    /// </summary>
    void Awake()
    {
        changesThisTurn = new List<GameStateChange>();
        cardToCardDisplayReferences = new Dictionary<Card, CardDisplay>();
        parsingGameChanges = true;
        DisablePlayerInteractions();
    }

    /// <summary>
    /// When a button is clicked in-game, this is called with the button's index as a parameter.
    /// This tells the game manager what button the player clicked.
    /// </summary>
    /// <param name="index">The index of the button.</param>
    public void ButtonSubmitted(int index)
    {
        EventSystem.current.SetSelectedGameObject(null);
        gameManager.PlayerSubmitButton(index);
    }

    /// <summary>
    /// When a Card Display is clicked in-game, this is called with the Card Display's Card reference.
    /// This tells the game manager what Card the player clicked.
    /// </summary>
    /// <param name="card">The Card of the card display that was clicked.</param>
    public void CardSubmitted(Card card)
    {
        gameManager.PlayerSubmitCard(card);
    }

    /// <summary>
    /// Captures the current list of game changes and begins displaying them.
    /// </summary>
    /// <param name="changes">The changes to display.</param>
    public void CaptureAndDisplayGameChanges(List<GameStateChange> changes)
    {
        // If not currently rendering changes, capture the new list itself.
        if (currentChangeIndex >= changesThisTurn.Count)
        {
            changesThisTurn = changes;
            currentChangeIndex = 0;
        }
        else
        {
            // If currently rendering changes, add the new changes onto the end.
            changesThisTurn.AddRange(changes);
        }

        // Don't let the player interact while displaying changes.
        DisablePlayerInteractions();

        currentChangeTime = timeForCurrentChange;
    }

    /// <summary>
    /// Call this when the player has placed their bet and wants to begin the round.
    /// </summary>
    public void BetPlaced()
    {
        betPanel.MinimizePanel();
        paytablePanel.MinimizePanel();
        gameManager.StartRound();
    }

    /// <summary>
    /// When the player has changed their bet and the paytable needs to be updated.
    /// </summary>
    public void UpdatePaytable()
    {
        paytablePanel.UpdatePaytableChosenBet();
    }

    /// <summary>
    /// Grabs a new Card Display and travels it from deck to hand.
    /// </summary>
    /// <param name="card">The Card to draw.</param>
    /// <param name="deckIndex">The index of the deck to draw from.</param>
    /// <param name="handIndex">The index of the hand to draw from.</param>
    /// <param name="indexInHand">Optional index position in the hand to place the Card at.</param>
    public void DrawCardToHand(Card card, int deckIndex, int handIndex, int indexInHand = -1)
    {
        CardDisplay display = (CardDisplay)cardDisplayPool.GetDisplay();
        if (cardToCardDisplayReferences.ContainsKey(card))
        {
            cardToCardDisplayReferences.Remove(card);
        }
        cardToCardDisplayReferences.Add(card, display);
        display.DisplayCard(card, cardVisualProfile, deckDisplays[deckIndex].GetRect(), true);
        display.SetStartTransform(deckDisplays[deckIndex].GetRect());
        display.StartTraveling();
        handDisplays[handIndex].AddCardDisplayToHand(display, indexInHand);
    }

    /// <summary>
    /// Travel a Card from a hand to the discard pile.
    /// </summary>
    /// <param name="card">The Card to discard.</param>
    /// <param name="handIndex">Index of the hand to discard from.</param>
    public void DiscardCardFromHand(Card card, int handIndex)
    {
        CardDisplay display = cardToCardDisplayReferences.GetValueOrDefault(card);
        handDisplays[handIndex].RemoveCardDisplayFromHand(display);
        display.TravelToTransform(discardPileTransform, cardDisplayPool.RemoveDisplay);
        display.FlipOverride(true);
    }

    /// <summary>
    /// Travel a Card from one hand to another.
    /// </summary>
    /// <param name="card">The Card to move.</param>
    /// <param name="fromHandIndex">The index of the hand to move from.</param>
    /// <param name="toHandIndex">The index of the hand to move to.</param>
    public void MoveCardToAnotherHand(Card card, int fromHandIndex, int toHandIndex)
    {
        CardDisplay display = cardToCardDisplayReferences.GetValueOrDefault(card);
        handDisplays[fromHandIndex].RemoveCardDisplayFromHand(display);
        display.SetStartTransform(display.GetRect());
        handDisplays[toHandIndex].AddCardDisplayToHand(display);
        display.StartTraveling();
    }

    /// <summary>
    /// When the round is over, this will present the winning result and the amount the user has one.
    /// </summary>
    /// <param name="paytableWinningRowIndex">The index of the winning paytable row. Use -1 if the player didnt win anything.</param>
    public void StartResultPresentation(int paytableWinningRowIndex)
    {
        StopParsingGameChanges();
        if (paytableWinningRowIndex == -1)
            resultPanel.StartPresentation("", false);
        else
        {
            paytablePanel.HighlightText(paytableWinningRowIndex);
            resultPanel.StartPresentation(gameManager.GetPaytable().GetRowName(paytableWinningRowIndex), gameManager.GetPaytable().IsAWin(paytableWinningRowIndex));
            betPanel.ShowUserWinnings();
        }
    }

    /// <summary>
    /// When the Result Presentation is over, call this to return to the game.
    /// </summary>
    public void EndResultPresentation()
    {
        paytablePanel.UnhighlightText();
        StartParsingGameChanges();
    }

    /// <summary>
    /// Show the result preview.
    /// </summary>
    /// <param name="index">The index of the result paytable row.</param>
    public void SetAndShowResultPreview(int index)
    {
        resultPreviewDisplay.Show();
        resultPreviewDisplay.SetText(gameManager.GetPaytable().GetRowName(index), true);
        paytablePanel.HighlightText(index);
    }

    /// <summary>
    /// Hide the result preview.
    /// </summary>
    public void HideResultPreviewDisplay()
    {
        paytablePanel.UnhighlightText();
        resultPreviewDisplay.Hide();
    }

    /// <summary>
    /// Resumes parsing game changes.
    /// </summary>
    public void StartParsingGameChanges()
    {
        parsingGameChanges = true;
    }

    /// <summary>
    /// Stops parsing game changes
    /// </summary>
    public void StopParsingGameChanges()
    {
        parsingGameChanges = false;
    }

    /// <summary>
    /// Enables the full screen UI raycast object that blocks player interactions.
    /// </summary>
    public void DisablePlayerInteractions()
    {
        uiRaycastShield.SetActive(true);
    }

    /// <summary>
    /// Disables the full screen UI raycast object that blocks player interactions.
    /// </summary>
    public void EnablePlayerInteractions()
    {
        uiRaycastShield.SetActive(false);
    }

    /// <summary>
    /// Every frame try to parse game changes that still need to be parsed.
    /// </summary>
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

    /// <summary>
    /// Controls the objects displayed on screen depending on the data from a game state change.
    /// </summary>
    /// <param name="change">The game state change to parse.</param>
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

            case GameStateChangeType.ResultPresentation:
                StartResultPresentation(change.FromIndex);
                break;

            case GameStateChangeType.BeginBetting:
                paytablePanel.MaximizePanel();
                betPanel.MaximizePanel();
                break;

            case GameStateChangeType.GameRulesLoaded:
                paytablePanel.SetupPaytablePanel(gameManager.GetPaytable());
                break;

            case GameStateChangeType.SetAndShowResultPreview:
                SetAndShowResultPreview(change.FromIndex);
                break;

            case GameStateChangeType.HideResultPreview:
                HideResultPreviewDisplay();
                break;
        }
    }
}
