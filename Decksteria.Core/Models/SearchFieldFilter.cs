namespace Decksteria.Core.Models;

/// <summary>
/// Constructor for the UI Layer to create a SearchFieldFIlter. Not called by the plug-ins.
/// </summary>
/// <param name="searchField">The <see cref="Decksteria.Core.Models.SearchField"/> provided by the Plug-In.</param>
/// <param name="comparison">The Comparison type selected by the User.</param>
/// <param name="value">The Search Value provided by the User.</param>
public class SearchFieldFilter(SearchField searchField, ComparisonType comparison, object value)
{

    /// <summary>
    /// A <see cref="Decksteria.Core.Models.SearchField"/> provided by the Plug-In.
    /// </summary>
    public SearchField SearchField { get; } = searchField;

    /// <summary>
    /// The Comparison type selected by the User.
    /// </summary>
    public ComparisonType Comparison { get; set; } = comparison;

    /// <summary>
    /// The Search Value provided by the User.
    /// </summary>
    public object? Value { get; set; } = value;

    /// <summary>
    /// Default <see cref="string" /> filter matching, call this inside the GetCardsAsync if you don't need to do any special filter matching.
    /// </summary>
    /// <param name="cardProperty">The value you specifically want to match.</param>
    /// <returns>A boolean value indicating whether the <paramref name="cardProperty"/> matches the default filter criteria based on the value of the search field.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Does not improve code readability due to multiple if Statements.")]
    public bool MatchesFilter(string? cardProperty)
    {
        if (SearchField.FieldType == FieldType.Number)
        {
            return false;
        }

        var stringValue = Value?.ToString();
        if (string.IsNullOrEmpty(stringValue))
        {
            return true;
        }

        if (string.IsNullOrEmpty(cardProperty))
        {
            return false;
        }

        return Comparison switch
        {
            ComparisonType.Equals => cardProperty == stringValue,
            ComparisonType.NotEquals => cardProperty != stringValue,
            ComparisonType.Contains => stringValue != null && cardProperty.Contains(stringValue),
            ComparisonType.NotContains => stringValue != null && !cardProperty.Contains(stringValue),
            _ => false,
        };
    }

    /// <summary>
    /// Default <see cref="int" /> filter matching, call this inside the GetCardsAsync if you don't need to do any special filter matching.
    /// </summary>
    /// <param name="cardProperty">The value you specifically want to match.</param>
    /// <returns>A boolean value indicating whether the <paramref name="cardProperty"/> matches the default filter criteria based on the value of the search field.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Does not improve code readability due to multiple if Statements.")]
    public bool MatchesFilter(int? cardProperty)
    {
        if (SearchField.FieldType != FieldType.Number)
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

        return IntMatching(cardProperty.Value);
    }

    /// <summary>
    /// Default <see cref="int" /> filter matching, call this inside the GetCardsAsync if you don't need to do any special filter matching.
    /// </summary>
    /// <param name="cardProperty">The value you specifically want to match.</param>
    /// <returns>A boolean value indicating whether the <paramref name="cardProperty"/> matches the default filter criteria based on the value of the search field.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Does not improve code readability due to multiple if Statements.")]
    public bool MatchesFilter(int cardProperty)
    {
        if (SearchField.FieldType != FieldType.Number)
        {
            return false;
        }

        if (Value is null)
        {
            return true;
        }

        return IntMatching(cardProperty);
    }

    private bool IntMatching(int cardProperty)
    {
        return Comparison switch
        {
            ComparisonType.Equals => cardProperty == Convert.ToInt32(Value),
            ComparisonType.NotEquals => cardProperty != Convert.ToInt32(Value),
            ComparisonType.GreaterThan => cardProperty > Convert.ToInt32(Value),
            ComparisonType.GreaterThanOrEqual => cardProperty >= Convert.ToInt32(Value),
            ComparisonType.LessThan => cardProperty < Convert.ToInt32(Value),
            ComparisonType.LessThanOrEqual => cardProperty <= Convert.ToInt32(Value),
            _ => false,
        };
    }
}

