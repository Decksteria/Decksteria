﻿namespace Decksteria.Ui.Maui.Pages.Search.Model;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Maui.Core.Extensions;
using Decksteria.Core.Models;
using Decksteria.Services.Deckbuilding.Models;
using Microsoft.Maui.Controls;
using UraniumUI.Material.Controls;

internal class MultiSelectionSearchFilter : IMauiSearchFilter
{
    private const int DefaultValue = 0;

    private readonly SearchField _searchField;

    private MultiplePickerField? _multiPickerField;

    public MultiSelectionSearchFilter(SearchField searchField)
    {
        if (searchField.FieldType is not FieldType.MultiSelect)
        {
            throw new InvalidCastException($"Only {FieldType.MultiSelect} can be a {nameof(MultiSelectionSearchFilter)}.");
        }

        if (searchField.OptionMapping is null)
        {
            throw new UnreachableException($"{nameof(_searchField.OptionMapping)} is null for a Multi-Select Field.");
        }

        _searchField = searchField;
        Values = searchField.Options.Cast<object>().ToObservableCollection();
        SelectableItems = searchField.Options.ToArray();
    }

    public string[] SelectableItems { get; init; }

    public string Title => _searchField.FieldName;

    public ObservableCollection<object> Values { get; set; }

    private bool IsChanged => Values.Count != SelectableItems.Length;

    public ISearchFieldFilter[] AsSearchFieldFilterArray() => IsChanged ? [(MultiSelectFieldFilter) this] : [];

    public VisualElement GetVisualElement()
    {
        var multiPickerField = new MultiplePickerField
        {
            Title = this.Title
        };
        multiPickerField.SetBinding(MultiplePickerField.ItemsSourceProperty, new Binding(nameof(MultiSelectionSearchFilter.SelectableItems), BindingMode.OneWay, source: this));
        multiPickerField.SetBinding(MultiplePickerField.SelectedItemsProperty, new Binding(nameof(MultiSelectionSearchFilter.Values), BindingMode.TwoWay, source: this));
        _multiPickerField = multiPickerField;
        return multiPickerField;
    }

    public static implicit operator MultiSelectFieldFilter(MultiSelectionSearchFilter textSearchField)
    {
        if (textSearchField._searchField.OptionMapping is null)
        {
            throw new UnreachableException($"{nameof(_searchField.OptionMapping)} is null for a Multi-Select Field.");
        }

        uint orSum = 0;
        foreach (var item in textSearchField.Values)
        {
            if (!textSearchField._searchField.OptionMapping.TryGetValue(item.ToString() ?? string.Empty, out var value))
            {
                throw new UnreachableException($"The user selected a item that does not exist in .");
            }

            orSum |= value;
        }

        return new MultiSelectFieldFilter(ComparisonType.Contains, textSearchField._searchField, orSum);
    }
}
