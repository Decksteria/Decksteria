namespace Decksteria.Ui.Maui.Pages.Search.Model;

using Decksteria.Core.Models;
using System;

internal class MultiSelectionSearchFilter : ISearchFilter
{
    private const int DefaultValue = 0;

    private readonly SearchField _searchField;

    public MultiSelectionSearchFilter(SearchField searchField)
    {
        if (searchField.FieldType is not FieldType.SingleSelect)
        {
            throw new InvalidCastException("Only Selection Fields can be a SelectionSearchFilter.");
        }

        _searchField = searchField;
        Value = DefaultValue;
    }

    public int Value { get; set; }

    private bool IsChanged => Value != DefaultValue;

    public SearchFieldFilter[] AsSearchFieldFilterArray() => IsChanged ? [this] : [];

    public static implicit operator SearchFieldFilter(MultiSelectionSearchFilter textSearchField)
    {
        return new SearchFieldFilter(textSearchField._searchField, ComparisonType.Contains, textSearchField.Value);
    }
}
