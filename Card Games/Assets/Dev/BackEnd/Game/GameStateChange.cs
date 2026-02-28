
public enum GameBoardTarget
{
    None,
    Deck,
    Hand,
    Discard
}

public enum GameStateChangeType
{
    CardMove,
    CardUpdate,
    HideButton,
    ShowButton,
    TextUpdate,
    HideText,
    ShowText,
    ResultsPresentation,
    BeginBetting,
    GameRulesLoaded,
    SetAndShowScorePaytablePreview,
    HideScorePreview,
}

public enum GameStateChangeTime
{
    Instant,
    Short,
    Medium,
    Long
}

public class GameStateChange
{
    public GameStateChangeType Type { get; private set; }

    public GameBoardTarget From { get; private set; }

    public int FromIndex { get; private set; }

    public GameBoardTarget To { get; private set; }

    public int ToIndex { get; private set; }

    public Card Card { get; private set; }

    public int IndexInHand { get; private set; }

    public GameStateChangeTime ChangeTime { get; private set; }

    public string Text { get; private set; }

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

    public GameStateChange(GameStateChangeType type, int fromIndex, string text, GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        Type = type;
        FromIndex = fromIndex;
        Text = text;
        ChangeTime = changeTime;
    }

    public GameStateChange(GameStateChangeType type, int fromIndex, GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        Type = type;
        FromIndex = fromIndex;
        ChangeTime = changeTime;
    }

    public GameStateChange(GameStateChangeType type, GameStateChangeTime changeTime = GameStateChangeTime.Instant)
    {
        Type = type;
        ChangeTime = changeTime;
    }


}

