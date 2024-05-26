namespace Decksteria.Ui.Maui.Pages.Search.Model;

using System;
using Decksteria.Core.Models;
using Decksteria.Services.Deckbuilding.Models;
using Microsoft.Maui.Controls;
using UraniumUI.Material.Controls;

internal class TextSearchFilter : IMauiSearchFilter
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
        Value = string.Empty;
    }

    public string Title => _searchField.FieldName;

    public string Value { get; set; }

    public int MaxLength => _searchField.Length;

    private bool IsChanged => !string.IsNullOrWhiteSpace(Value);

    public SearchFieldFilter[] AsSearchFieldFilterArray() => IsChanged ? [this] : [];

    public VisualElement GetVisualElement()
    {
        var textField = new TextField
        {
            Title = this.Title
        };
        textField.SetBinding(TextField.MaxLengthProperty, new Binding(nameof(TextSearchFilter.MaxLength), BindingMode.OneWay, source: this));
        textField.SetBinding(TextField.TextProperty, new Binding(nameof(TextSearchFilter.Value), BindingMode.TwoWay, source: this));
        return textField;
    }

    public static implicit operator SearchFieldFilter(TextSearchFilter textSearchField)
    {
        return new SearchFieldFilter(textSearchField._searchField, ComparisonType.Contains, textSearchField.Value);
    }
}
