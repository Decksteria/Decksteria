namespace Decksteria.Services.Deckbuilding.Models;

using System.ComponentModel;
using Decksteria.Core.Models;

public sealed class MultiSelectFieldFilter : ISearchFieldFilter
{
    public MultiSelectFieldFilter(ComparisonType comparison, SearchField searchField, uint? uintValue = null)
    {
        if (searchField.FieldType is not FieldType.MultiSelect)
        {
            throw new InvalidEnumArgumentException(nameof(searchField.FieldType), (int) searchField.FieldType, searchField.FieldType.GetType());
        }

        Comparison = comparison;
        SearchField = searchField;
        this.UintValue = uintValue ?? 0;
    }

    public ComparisonType Comparison { get; set; }

    public SearchField SearchField { get; }

    public object? Value => UintValue;

    public uint UintValue { get; set; }

    public bool MatchesFilter(int cardProperty)
    {
        if (cardProperty < 0)
        {
            return false;
        }

        return BitwiseMatching((uint) cardProperty);
    }

    public bool MatchesFilter(int? cardProperty)
    {
        if (!cardProperty.HasValue || cardProperty < 0)
        {
            return false;
        }

        return BitwiseMatching((uint) cardProperty);
    }

    public bool MatchesFilter(string? cardProperty)
    {
        if (cardProperty is null || uint.TryParse(cardProperty, out var uintProperty))
        {
            return false;
        }

        return BitwiseMatching(uintProperty);
    }

    public bool MatchesFilter(uint cardProperty) => BitwiseMatching(cardProperty);

    /// <summary>
    /// Default implementation for matching <see cref="uint"/> against its own value.
    /// </summary>
    /// <param name="cardProperty">The value from the card.</param>
    /// <returns>The property successfully fulfils the conditions.</returns>
    private bool BitwiseMatching(uint cardProperty)
    {
        if (UintValue <= 0)
        {
            return true;
        }

        if (cardProperty <= 0)
        {
            return false;
        }

        return Comparison switch
        {
            ComparisonType.Equals => cardProperty == UintValue,
            ComparisonType.NotEquals => cardProperty != UintValue,
            ComparisonType.Contains => (cardProperty & UintValue) > 0,
            ComparisonType.NotContains => (cardProperty & UintValue) > 0,
            ComparisonType.GreaterThanOrEqual => (cardProperty & UintValue) >= UintValue,
            ComparisonType.LessThan => (cardProperty & UintValue) == 0,
            _ => false
        };
    }
}
