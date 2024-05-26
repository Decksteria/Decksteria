namespace Decksteria.Services.Deckbuilding.Models;

using System;
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
        if (Value is string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(cardProperty))
            {
                return false;
            }

            return StringMatching(cardProperty, stringValue);
        }
        else if (Value is int intValue)
        {
            if (Value is null)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(cardProperty))
            {
                return false;
            }

            var intProperty = -1;
            if (SearchField.FieldType is FieldType.Number && !int.TryParse(cardProperty, out intProperty))
            {
                return false;
            }
            else if (SearchField.FieldType is FieldType.MultiSelect
                && !(SearchField.OptionMapping?.TryGetValue(cardProperty, out intProperty) ?? throw new NullReferenceException($"{nameof(SearchField.OptionMapping)} is null.")))
            {
                return false;
            }
            else if (intProperty == -1)
            {
                return false;
            }

            return IntMatching(intProperty, intValue);
        }

        return false;
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

        if (Value is not int intValue || !cardProperty.HasValue)
        {
            return false;
        }

        return IntMatching(cardProperty.Value, intValue);
    }

    public bool MatchesFilter(int cardProperty)
    {
        if (SearchField.FieldType is not FieldType.Number)
        {
            return false;
        }

        if (Value is null)
        {
            return true;
        }

        return IntMatching(cardProperty, Convert.ToInt32(Value));
    }

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
    /// Default implementation for matching <see cref="int"/> and <see cref="FieldType.MultiSelect"/> against its own value.
    /// </summary>
    /// <param name="cardProperty">The value from the card.</param>
    /// <param name="intValue">The value provided by the user.</param>
    /// <returns>The property successfully fulfils the conditions.</returns>
    private bool BitwiseMatching(int cardProperty, int intValue)
    {
        if (intValue <= 0)
        {
            return true;
        }

        if (cardProperty <= 0)
        {
            return false;
        }

        return Comparison switch
        {
            ComparisonType.Equals => cardProperty == intValue,
            ComparisonType.NotEquals => cardProperty != intValue,
            ComparisonType.Contains => (cardProperty & intValue) > 0,
            ComparisonType.NotContains => (cardProperty & intValue) > 0,
            ComparisonType.GreaterThanOrEqual => (cardProperty & intValue) >= intValue,
            ComparisonType.LessThan => (cardProperty & intValue) == 0,
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

