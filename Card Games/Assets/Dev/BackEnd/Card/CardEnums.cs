/// <summary>
/// The Value of a Card, such as 2-10, Jack, Queen, King, or Ace.
/// </summary>
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

/// <summary>
/// The suit of the card, such as Spade, Heart, Club, or Diamond.
/// </summary>
public enum CardSuit
{
    Spade,
    Heart,
    Club,
    Diamond
}

/// <summary>
/// Any special properties of the card, such as if its Wild or not. Can be expanded upon.
/// </summary>
public enum CardSpecial
{
    None,
    Wild,
}