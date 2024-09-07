namespace Decksteria.Services.Deckbuilding.Models;

using System;
using System.ComponentModel;
using Decksteria.Core.Models;

public sealed class TextFieldFilter : ISearchFieldFilter
{
    public TextFieldFilter(ComparisonType comparison, SearchField searchField, string? stringValue = null)
    {
        if (searchField.FieldType is not FieldType.Text or FieldType.SingleSelect)
        {
            throw new InvalidEnumArgumentException(nameof(searchField.FieldType), (int) searchField.FieldType, searchField.FieldType.GetType());
        }

        Comparison = comparison;
        SearchField = searchField;
        StringValue = stringValue;
    }

    public ComparisonType Comparison { get; set; }

    public SearchField SearchField { get; init; }

    public object? Value => StringValue;

    public string? StringValue { get; set; }

    public bool MatchesFilter(int cardProperty) => StringMatching(cardProperty.ToString());

    public bool MatchesFilter(int? cardProperty)
    {
        if (!cardProperty.HasValue)
        {
            return false;
        }

        return StringMatching(cardProperty.Value.ToString());
    }

    public bool MatchesFilter(string? cardProperty)
    {
        if (cardProperty is null)
        {
            return false;
        }

        return StringMatching(cardProperty);
    }

    public bool MatchesFilter(uint cardProperty) => StringMatching(cardProperty.ToString());

    /// <summary>
    /// Default implementation for matching <see cref="string"/> against its own value.
    /// </summary>
    /// <param name="cardProperty">The value from the card.</param>
    /// <returns>The property successfully fulfils the conditions.</returns>
    private bool StringMatching(string cardProperty)
    {
        if (StringValue is null)
        {
            return true;
        }

        return Comparison switch
        {
            ComparisonType.Equals => string.Equals(cardProperty, StringValue, StringComparison.InvariantCultureIgnoreCase),
            ComparisonType.NotEquals => !string.Equals(cardProperty, StringValue, StringComparison.InvariantCultureIgnoreCase),
            ComparisonType.Contains => cardProperty.Contains(StringValue, StringComparison.InvariantCultureIgnoreCase),
            ComparisonType.NotContains => !cardProperty.Contains(StringValue, StringComparison.InvariantCultureIgnoreCase),
            ComparisonType.StartsWith => cardProperty.StartsWith(StringValue, StringComparison.InvariantCultureIgnoreCase),
            ComparisonType.EndsWith => cardProperty.EndsWith(StringValue, StringComparison.InvariantCultureIgnoreCase),
            _ => false,
        };
    }
}
