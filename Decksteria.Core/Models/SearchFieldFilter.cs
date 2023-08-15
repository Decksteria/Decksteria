namespace Decksteria.Core.Models;

public class SearchFieldFilter
{
    /// <summary>
    /// Constructor for the UI Layer to create a SearchFieldFIlter. Not called by the plug-ins.
    /// </summary>
    /// <param name="searchField">The <see cref="Decksteria.Core.Models.SearchField"/> provided by the Plug-In.</param>
    /// <param name="comparison">The Comparison type selected by the User.</param>
    /// <param name="value">The Search Value provided by the User.</param>
    public SearchFieldFilter(SearchField searchField, ComparisonType comparison, object value)
    {
        SearchField = searchField;
        Comparison = comparison;
        Value = value;
    }

    /// <summary>
    /// A <see cref="Decksteria.Core.Models.SearchField"/> provided by the Plug-In.
    /// </summary>
    public SearchField SearchField { get; }

    /// <summary>
    /// The Comparison type selected by the User.
    /// </summary>
    public ComparisonType Comparison { get; set; }

    /// <summary>
    /// The Search Value provided by the User.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Default <see cref="string" /> filter matching, call this inside the GetCardsAsync if you don't need to do any special filter matching.
    /// </summary>
    /// <param name="cardProperty">The value you specifically want to match.</param>
    /// <returns>A boolean value indicating whether the <paramref name="cardProperty"/> matches the default filter criteria based on the value of the search field.</returns>
    public bool MatchesFilter(string? cardProperty)
    {
        if (SearchField.FieldType == FieldType.Number)
        {
            return false;
        }

        var stringValue = Value?.ToString();
        if (stringValue == null)
        {
            return true;
        }

        if (cardProperty == null)
        {
            return false;
        }

        switch (Comparison)
        {
            case ComparisonType.Equals:
                return cardProperty == stringValue;
            case ComparisonType.NotEquals:
                return cardProperty != stringValue;
            case ComparisonType.Contains:
                return stringValue != null && cardProperty.Contains(stringValue);
            case ComparisonType.NotContains:
                return stringValue != null && !cardProperty.Contains(stringValue);
        }

        return false;
    }

    /// <summary>
    /// Default <see cref="int" /> filter matching, call this inside the GetCardsAsync if you don't need to do any special filter matching.
    /// </summary>
    /// <param name="cardProperty">The value you specifically want to match.</param>
    /// <returns>A boolean value indicating whether the <paramref name="cardProperty"/> matches the default filter criteria based on the value of the search field.</returns>
    public bool MatchesFilter(int? cardProperty)
    {
        if (SearchField.FieldType != FieldType.Number)
        {
            return false;
        }

        if (Value == null)
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
    public bool MatchesFilter(int cardProperty)
    {
        if (SearchField.FieldType != FieldType.Number)
        {
            return false;
        }

        if (Value == null)
        {
            return true;
        }

        return IntMatching(cardProperty);
    }

    private bool IntMatching(int cardProperty)
    {
        switch (Comparison)
        {
            case ComparisonType.Equals:
                return cardProperty == Convert.ToInt32(Value);
            case ComparisonType.NotEquals:
                return cardProperty != Convert.ToInt32(Value);
            case ComparisonType.GreaterThan:
                return cardProperty > Convert.ToInt32(Value);
            case ComparisonType.GreaterThanOrEqual:
                return cardProperty >= Convert.ToInt32(Value);
            case ComparisonType.LessThan:
                return cardProperty < Convert.ToInt32(Value);
            case ComparisonType.LessThanOrEqual:
                return cardProperty <= Convert.ToInt32(Value);
        }

        return false;
    }
}

