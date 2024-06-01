namespace Decksteria.Services.Deckbuilding.Models;

using System.ComponentModel;
using Decksteria.Core.Models;

public sealed class NumberFieldFilter : ISearchFieldFilter
{
    public NumberFieldFilter(ComparisonType comparison, SearchField searchField, int? intValue = null)
    {
        if (searchField.FieldType is not FieldType.Number)
        {
            throw new InvalidEnumArgumentException(nameof(searchField.FieldType), (int) searchField.FieldType, searchField.FieldType.GetType());
        }

        Comparison = comparison;
        SearchField = searchField;
        IntValue = intValue ?? 0;
    }

    public ComparisonType Comparison { get; set; }

    public SearchField SearchField { get; }

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

    /// <summary>
    /// Default implementation for matching <see cref="int"/> and <see cref="FieldType.Number"/> against its own value.
    /// </summary>
    /// <param name="cardProperty">The value from the card.</param>
    /// <returns>The property successfully fulfils the conditions.</returns>
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
