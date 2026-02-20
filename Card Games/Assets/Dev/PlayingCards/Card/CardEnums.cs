public enum CardValue
{
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}

public enum CardSuit
{
    Spade,
    Heart,
    Club,
    Diamond
}

public enum HandComponent
{
    ValueCollection,
    SuitCollection,
    ConsecutiveValues,
}

public enum HandAfterRuleOperation
{
    None,
    TrimScoringCards,
    TrimNonscoringCards
}

public enum CardSpecial
{
    None,
    Wild,
}