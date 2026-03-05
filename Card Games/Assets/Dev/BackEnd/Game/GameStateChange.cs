/// <summary>
/// An element on the game board/view that a state change can target.
/// </summary>
public enum GameBoardTarget
{
    None,
    Deck,
    Hand,
    Discard
}

/// <summary>
/// Type of game state change.
/// </summary>
public enum GameStateChangeType
{
    CardMove,
    CardUpdate,
    HideButton,
    ShowButton,
    TextUpdate,
    HideText,
    ShowText,
    ResultPresentation,
    BeginBetting,
    GameRulesLoaded,
    SetAndShowResultPreview,
    HideResultPreview,
}

/// <summary>
/// The time interval this state change should take up.
/// </summary>
public enum GameStateChangeTime
{
    Instant,
    Short,
    Medium,
    Long
}

/// <summary>
/// A game state change represents how an element in the game has changed.
/// </summary>
public class GameStateChange
{
    /// <summary>
    /// The type of game state change this is.
    /// </summary>
    public GameStateChangeType Type { get; private set; }

    /// <summary>
    /// The type of game element this change starts from.
    /// </summary>
    public GameBoardTarget From { get; private set; }

    /// <summary>
    /// The index of the game element this change starts from.
    /// </summary>
    public int FromIndex { get; private set; }

    /// <summary>
    /// The type of game element this change goes to.
    /// </summary>
    public GameBoardTarget To { get; private set; }

    /// <summary>
    /// The index of the game element this change goes to.
    /// </summary>
    public int ToIndex { get; private set; }

    /// <summary>
    /// The Card that is affected by this change.
    /// </summary>
    public Card Card { get; private set; }

    /// <summary>
    /// Position in hand to put Cards into or remove from.
    /// </summary>
    public int IndexInHand { get; private set; }

    /// <summary>
    /// The time interval this state change should take up.
    /// </summary>
    public GameStateChangeTime ChangeTime { get; private set; }

    /// <summary>
    /// The string text that may be needed for this change.
    /// </summary>
    public string Text { get; private set; }

    /// <summary>
    /// Constructor for a GameStateChange for moving a Card around the game board.
    /// </summary>
    /// <param name="type">The type of game state change this is.</param>
    /// <param name="from">The type of game element this change starts from.</param>
    /// <param name="fromIndex">The index of the game element this change starts from.</param>
    /// <param name="to">The type of game element this change goes to.</param>
    /// <param name="toIndex">The index of the game element this change goes to.</param>
    /// <param name="card">The Card that is affected by this change.</param>
    /// <param name="changeTime">Optional override of the time interval this state change should take up.</param>
    /// <param name="indexInHand">Optional override of the position in hand to put Cards into or remove from.</param>
    public GameStateChange(GameStateChangeType type, GameBoardTarget from, int fromIndex, GameBoardTarget to, int toIndex, Card card, GameStateChangeTime changeTime = GameStateChangeTime.Short, int indexInHand = -1)
    {
        Type = type;
        From = from;
        FromIndex = fromIndex;
        To = to;
        ToIndex = toIndex;
        Card = card;
        ChangeTime = changeTime;
        IndexInHand = indexInHand;
    }

    /// <summary>
    /// Constructor for a GameStateChange for updating status of a Card.
    /// </summary>
    /// <param name="type">The type of game state change this is.</param>
    /// <param name="card">The Card that is affected by this change.</param>
    /// <param name="changeTime">Optional override of the time interval this state change should take up.</param>
    public GameStateChange(GameStateChangeType type, Card card, GameStateChangeTime changeTime = GameStateChangeTime.Short)
    {
        Type = type;
        From = GameBoardTarget.None;
        FromIndex = 0;
        To = GameBoardTarget.None;
        ToIndex = 0;
        Card = card;
        ChangeTime = changeTime;
        IndexInHand = -1;
    }

    /// <summary>
    /// Constructor for a GameStateChange for updating the text of a text display.
    /// </summary>
    /// <param name="type">The type of game state change this is.</param>
    /// <param name="fromIndex">The index of the text display.</param>
    /// <param name="text">The new text to show.</param>
    /// <param name="changeTime">Optional override of the time interval this state change should take up.</param>
    public GameStateChange(GameStateChangeType type, int fromIndex, string text, GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        Type = type;
        FromIndex = fromIndex;
        Text = text;
        ChangeTime = changeTime;
    }

    /// <summary>
    /// Constructor for a GameStateChange for updating the status of a game view element.
    /// </summary>
    /// <param name="type">The type of game state change this is.</param>
    /// <param name="fromIndex">The index of the game view element. Use -1 for all of them.</param>
    /// <param name="changeTime">Optional override of the time interval this state change should take up.</param>
    public GameStateChange(GameStateChangeType type, int fromIndex, GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        Type = type;
        FromIndex = fromIndex;
        ChangeTime = changeTime;
    }

    /// <summary>
    /// Constructor for a GameStateChange that doesn't need any parameters.
    /// </summary>
    /// <param name="type">The type of game state change this is.</param>
    /// <param name="changeTime">Optional override of the time interval this state change should take up.</param>
    public GameStateChange(GameStateChangeType type, GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        Type = type;
        ChangeTime = changeTime;
    }


}

