namespace Decksteria.Ui.Maui.Pages.Search.Model;

using System;
using Decksteria.Core.Models;
using Decksteria.Services.Deckbuilding.Models;

internal class TextSearchFilter : ISearchFilter
{
    private const double MinimumFieldWidth = 60;

    private readonly SearchField _searchField;

    public TextSearchFilter(SearchField searchField)
    {
        if (searchField.FieldType is not FieldType.Text)
        {
            throw new InvalidCastException($"Only {FieldType.Text} can be a {nameof(TextSearchFilter)}.");
        }

        _searchField = searchField;
        var preferredWidth = _searchField.Length * (double) 20;
        WidthRequest = Math.Max(preferredWidth, MinimumFieldWidth);
        Value = string.Empty;
    }

    public string Value { get; set; }

    public double WidthRequest { get; }

    public int MaxLength => _searchField.Length;

    private bool IsChanged => !string.IsNullOrWhiteSpace(Value);

    public SearchFieldFilter[] AsSearchFieldFilterArray() => IsChanged ? [this] : [];

    public static implicit operator SearchFieldFilter(TextSearchFilter textSearchField)
    {
        return new SearchFieldFilter(textSearchField._searchField, ComparisonType.Contains, textSearchField.Value);
    }
}
