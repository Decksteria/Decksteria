namespace Decksteria.Ui.Maui.Pages.Search.Model;

using System;
using System.ComponentModel;
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
            throw new InvalidEnumArgumentException($"Only {FieldType.Text} can be a {nameof(TextSearchFilter)}.");
        }

        _searchField = searchField;
        Value = string.Empty;
    }

    public string Title => _searchField.FieldName;

    public string Value { get; set; }

    public int MaxLength => _searchField.Length;

    private bool IsChanged => !string.IsNullOrWhiteSpace(Value);

    public ISearchFieldFilter[] AsSearchFieldFilterArray() => IsChanged ? [(TextFieldFilter) this] : [];

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

    public static implicit operator TextFieldFilter(TextSearchFilter textSearchField)
    {
        return new TextFieldFilter(ComparisonType.Contains, textSearchField._searchField, textSearchField.Value);
    }
}
