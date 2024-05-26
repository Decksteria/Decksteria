namespace Decksteria.Services.Deckbuilding.Models;

using System;
using System.Diagnostics;
using Decksteria.Core.Models;

/// <summary>
/// Constructor for the UI Layer to create a SearchFieldFilter. Not called by the plug-ins.
/// </summary>
public class SearchFieldFilter : ISearchFieldFilter
{
    public SearchField SearchField { get; init; }

    public ComparisonType Comparison { get; init; }

    public object? Value { get; init; }

    /// <summary>
    /// Constructor that takes in the value that will be used in comparison.
    /// </summary>
    /// <param name="searchField">The <see cref="SearchField"/> provided by the Plug-In.</param>
    /// <param name="comparison">The Comparison type selected by the User.</param>
    /// <param name="value">The Search Value provided by the User.</param>
    public SearchFieldFilter(SearchField searchField, ComparisonType comparison, object value)
    {
        SearchField = searchField;
        Comparison = comparison;
        Value = value;
    }

    public bool MatchesFilter(string? cardProperty)
    {
        if (Value is null)
        {
            return true;
        }

        switch (Value)
        {
            case null:
                return true;
            case string stringValue:
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    return true;
                }

                if (string.IsNullOrWhiteSpace(cardProperty))
                {
                    return false;
                }

                return StringMatching(cardProperty, stringValue);
            case int intValue:
                if (string.IsNullOrWhiteSpace(cardProperty))
                {
                    return false;
                }

                return int.TryParse(cardProperty, out var intProperty) && IntMatching(intProperty, intValue);
            case uint uintValue:
                if (string.IsNullOrWhiteSpace(cardProperty) || SearchField.OptionMapping is null)
                {
                    return false;
                }

                return SearchField.OptionMapping.TryGetValue(cardProperty, out var uintProperty) && BitwiseMatching(uintProperty, uintValue);
            default:
                return false;
        }
    }

    public bool MatchesFilter(int? cardProperty)
    {
        if (SearchField.FieldType is not FieldType.Number or FieldType.MultiSelect)
        {
            return false;
        }

        if (Value is null)
        {
            return true;
        }

        if (!cardProperty.HasValue)
        {
            return false;
        }

        return Value switch
        {
            int intValue => IntMatching(cardProperty.Value, intValue),
            uint uintValue => BitwiseMatching((uint) cardProperty.Value, uintValue),
            _ => false,
        };
    }

    public bool MatchesFilter(int cardProperty)
    {
        if (SearchField.FieldType is not FieldType.Number or FieldType.MultiSelect)
        {
            return false;
        }

        return Value switch
        {
            int intValue => IntMatching(cardProperty, intValue),
            uint uintValue => BitwiseMatching((uint) cardProperty, uintValue),
            _ => false,
        };
    }

    public bool MatchesFilter(uint cardProperty) => SearchField.FieldType is FieldType.MultiSelect && Value is uint uintValue && BitwiseMatching((uint) cardProperty, uintValue);

    /// <summary>
    /// Default implementation for matching <see cref="int"/> and <see cref="FieldType.Number"/> against its own value.
    /// </summary>
    /// <param name="cardProperty">The value from the card.</param>
    /// <param name="intValue">The value provided by the user.</param>
    /// <returns>The property successfully fulfils the conditions.</returns>
    private bool IntMatching(int cardProperty, int intValue)
    {
        return Comparison switch
        {
            ComparisonType.Equals => cardProperty == intValue,
            ComparisonType.NotEquals => cardProperty != intValue,
            ComparisonType.GreaterThan => cardProperty > intValue,
            ComparisonType.GreaterThanOrEqual => cardProperty >= intValue,
            ComparisonType.LessThan => cardProperty < intValue,
            ComparisonType.LessThanOrEqual => cardProperty <= intValue,
            _ => false,
        };
    }

    /// <summary>
    /// Default implementation for matching <see cref="uint"/> against its own value.
    /// </summary>
    /// <param name="cardProperty">The value from the card.</param>
    /// <param name="uintValue">The value provided by the user.</param>
    /// <returns>The property successfully fulfils the conditions.</returns>
    private bool BitwiseMatching(uint cardProperty, uint uintValue)
    {
        if (uintValue <= 0)
        {
            return true;
        }

        if (cardProperty <= 0)
        {
            return false;
        }

        return Comparison switch
        {
            ComparisonType.Equals => cardProperty == uintValue,
            ComparisonType.NotEquals => cardProperty != uintValue,
            ComparisonType.Contains => (cardProperty & uintValue) > 0,
            ComparisonType.NotContains => (cardProperty & uintValue) > 0,
            ComparisonType.GreaterThanOrEqual => (cardProperty & uintValue) >= uintValue,
            ComparisonType.LessThan => (cardProperty & uintValue) == 0,
            _ => false
        };
    }

    /// <summary>
    /// Default implementation for matching <see cref="string"/> against its own value.
    /// </summary>
    /// <param name="cardProperty">The value from the card.</param>
    /// <param name="matchValue">The value provided by the user.</param>
    /// <returns>The property successfully fulfils the conditions.</returns>
    private bool StringMatching(string cardProperty, string matchValue)
    {
        return Comparison switch
        {
            ComparisonType.Equals => cardProperty == matchValue,
            ComparisonType.NotEquals => cardProperty != matchValue,
            ComparisonType.Contains => cardProperty.Contains(matchValue),
            ComparisonType.NotContains => !cardProperty.Contains(matchValue),
            ComparisonType.StartsWith => cardProperty.StartsWith(matchValue),
            ComparisonType.EndsWith => cardProperty.EndsWith(matchValue),
            _ => false,
        };
    }
}

