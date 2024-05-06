namespace Decksteria.Core.Models;

/// <summary>
/// Represents what comparison the user wants to perform on a
/// particular advanced filter field.
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