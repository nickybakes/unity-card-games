
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
    RoundEnd,
}

public enum GameStateChangeTime
{
    Instant,
    Standard,
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



    public GameStateChange(GameStateChangeType type, GameBoardTarget from, int fromIndex, GameBoardTarget to, int toIndex, Card card, GameStateChangeTime changeTime = GameStateChangeTime.Standard, int indexInHand = -1)
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

    public GameStateChange(GameStateChangeType type, Card card, GameStateChangeTime changeTime = GameStateChangeTime.Standard)
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


}

