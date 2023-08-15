namespace Decksteria.Core.Models;

/// <summary>
/// Represents what Comparison the user wants to perform
/// </summary>
public enum ComparisonType
{
    Equals,
    NotEquals,
    Contains,
    NotContains,
    StartsWith,
    EndsWith,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual
}