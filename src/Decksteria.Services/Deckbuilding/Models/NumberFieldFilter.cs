namespace Decksteria.Services.Deckbuilding.Models;

using Decksteria.Core.Models;

internal sealed class NumberFieldFilter : ISearchFieldFilter
{
    public ComparisonType Comparison { get; set; } = ComparisonType.Equals;

    public required SearchField SearchField { get; init; }

    public object? Value => IntValue;

    public int IntValue { get; set; }

    public bool MatchesFilter(int cardProperty) => IntMatching(cardProperty);

    public bool MatchesFilter(int? cardProperty) => cardProperty.HasValue && IntMatching(cardProperty.Value);

    public bool MatchesFilter(string? cardProperty)
    {
        if (cardProperty is null || int.TryParse(cardProperty, out var intProperty))
        {
            return false;
        }

        return IntMatching(intProperty);
    }

    public bool MatchesFilter(uint cardProperty) => IntMatching((int) cardProperty);

    private bool IntMatching(int cardProperty)
    {
        return Comparison switch
        {
            ComparisonType.Equals => cardProperty == IntValue,
            ComparisonType.NotEquals => cardProperty != IntValue,
            ComparisonType.GreaterThan => cardProperty > IntValue,
            ComparisonType.GreaterThanOrEqual => cardProperty >= IntValue,
            ComparisonType.LessThan => cardProperty < IntValue,
            ComparisonType.LessThanOrEqual => cardProperty <= IntValue,
            _ => false,
        };
    }
}
