namespace Decksteria.Ui.Maui.Pages.Search.Model;

using Decksteria.Core.Models;
using System;
using System.Linq;

internal class SingleSelectionSearchFilter : ISearchFilter
{
    private readonly SearchField _searchField;

    public SingleSelectionSearchFilter(SearchField searchField)
    {
        if (searchField.FieldType is not FieldType.SingleSelect)
        {
            throw new InvalidCastException("Only Selection Fields can be a SelectionSearchFilter.");
        }

        _searchField = searchField;
        Value = searchField.DefaultSelect ?? searchField.Options.First();
    }

    public string Value { get; set; }

    private bool IsChanged => Value.Length == _searchField.Options.Count();

    public SearchFieldFilter[] AsSearchFieldFilterArray() => IsChanged ? [this] : [];

    public static implicit operator SearchFieldFilter(SingleSelectionSearchFilter textSearchField)
    {
        return new SearchFieldFilter(textSearchField._searchField, ComparisonType.Equals, textSearchField.Value);
    }
}
