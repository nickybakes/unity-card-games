/// <summary>
/// Components of a Poker Hand. Every Poker Hand is made up of some combination of these components.
/// </summary>
public enum HandComponent
{
    MatchingValues,
    MatchingSuits,
    ConsecutiveValues,
}

/// <summary>
/// Operation to do after checking a Hand Rule. 
/// For example, trimming the scoring cards means they wont be counted again for later rules.
/// </summary>
public enum HandAfterRuleOperation
{
    None,
    TrimScoringCards,
    TrimNonscoringCards
}