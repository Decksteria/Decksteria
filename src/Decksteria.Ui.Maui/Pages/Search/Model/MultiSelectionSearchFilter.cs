﻿namespace Decksteria.Ui.Maui.Pages.Search.Model;

using Decksteria.Core.Models;
using Decksteria.Services.Deckbuilding.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

internal class MultiSelectionSearchFilter : ISearchFilter
{
    private const int DefaultValue = 0;

    private readonly SearchField _searchField;

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
        Values = searchField.Options.ToList();
        SelectableItems = searchField.Options.ToArray();
    }

    public string[] SelectableItems { get; init; }

    public IList<string> Values { get; set; }

    private bool IsChanged => Values.Count == SelectableItems.Length;

    public SearchFieldFilter[] AsSearchFieldFilterArray() => IsChanged ? [this] : [];

    public static implicit operator SearchFieldFilter(MultiSelectionSearchFilter textSearchField)
    {
        if (textSearchField._searchField.OptionMapping is null)
        {
            throw new UnreachableException($"{nameof(_searchField.OptionMapping)} is null for a Multi-Select Field.");
        }

        uint orSum = 0;
        foreach (var item in textSearchField.Values)
        {
            if (!textSearchField._searchField.OptionMapping.TryGetValue(item, out var value))
            {
                throw new UnreachableException($"The user selected a item that does not exist in .");
            }

            orSum |= value;
        }

        return new SearchFieldFilter(textSearchField._searchField, ComparisonType.Contains, orSum);
    }
}
