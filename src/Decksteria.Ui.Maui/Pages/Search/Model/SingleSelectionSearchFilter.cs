﻿namespace Decksteria.Ui.Maui.Pages.Search.Model;

using Decksteria.Core.Models;
using Decksteria.Services.Deckbuilding.Models;
using Microsoft.Maui.Controls;
using System;
using System.Linq;
using UraniumUI.Material.Controls;

internal class SingleSelectionSearchFilter : IMauiSearchFilter
{
    private readonly SearchField _searchField;

    public SingleSelectionSearchFilter(SearchField searchField)
    {
        if (searchField.FieldType is not FieldType.SingleSelect)
        {
            throw new InvalidCastException($"Only {FieldType.SingleSelect} can be a {nameof(SingleSelectionSearchFilter)}.");
        }

        _searchField = searchField;
        Value = searchField.DefaultSelect ?? searchField.Options.First();
        SelectableItems = searchField.Options.ToArray();
    }

    public string[] SelectableItems { get; init; }

    public string Title => _searchField.FieldName;

    public string Value { get; set; }

    private bool IsChanged => Value.Length == _searchField.Options.Count();

    public SearchFieldFilter[] AsSearchFieldFilterArray() => IsChanged ? [this] : [];

    public VisualElement GetVisualElement()
    {
        var pickerField = new PickerField
        {
            Title = this.Title
        };
        pickerField.SetBinding(PickerField.ItemsSourceProperty, new Binding(nameof(SingleSelectionSearchFilter.SelectableItems), BindingMode.OneWay, source: this));
        pickerField.SetBinding(PickerField.SelectedItemProperty, new Binding(nameof(SingleSelectionSearchFilter.Value), BindingMode.TwoWay, source: this));
        return pickerField;
    }

    public static implicit operator SearchFieldFilter(SingleSelectionSearchFilter textSearchField)
    {
        return new SearchFieldFilter(textSearchField._searchField, ComparisonType.Equals, textSearchField.Value);
    }
}
