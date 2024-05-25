﻿namespace Decksteria.Ui.Maui.Pages.Search.Model;

using System;
using Decksteria.Core.Models;

internal sealed class NumberSearchFilter
{
    private const double MinimumFieldWidth = 60;

    private readonly SearchField _searchField;

    public NumberSearchFilter(SearchField searchField)
    {
        if (searchField.FieldType is not FieldType.Number)
        {
            throw new InvalidCastException("Only Number Fields can be a NumberSearchFilter.");
        }

        _searchField = searchField;
        MinimumValue = _searchField.MinValue;
        MaximumValue = _searchField.MaxValue;
        var preferredWidth = _searchField.MaxValue.ToString().Length * (double) 20;
        WidthRequest = Math.Max(preferredWidth, MinimumFieldWidth);
    }

    /// <summary>
    /// The lower range selected by the user.
    /// </summary>
    public int MinimumValue { get; set; }

    /// <summary>
    /// The upper range selected by the user.
    /// </summary>
    public int MaximumValue { get; set; }

    /// <summary>
    /// The lower limit that the user is allowed to select for <see cref="Minimum"/>
    /// </summary>
    public int Minimum => _searchField.MinValue;

    /// <summary>
    /// The upper limit that the user is allowed to select for <see cref="Maximum"/>
    /// </summary>
    public int Maximum => _searchField.MaxValue;

    /// <summary>
    /// The Title of the Field.
    /// </summary>
    public string Title => _searchField.FieldName;

    public double WidthRequest { get; }

    private bool MinimumIsChanged => Minimum != MinimumValue;

    private bool MaximumIsChanged => Maximum != MaximumValue;

    public static implicit operator SearchFieldFilter[] (NumberSearchFilter numberSearchFilter)
    {
        if (!numberSearchFilter.MinimumIsChanged || !numberSearchFilter.MaximumIsChanged)
        {
            return [];
        }

        if (numberSearchFilter.MinimumIsChanged)
        {
            return [new SearchFieldFilter(numberSearchFilter._searchField, ComparisonType.GreaterThanOrEqual, numberSearchFilter.Minimum),];
        }

        if (numberSearchFilter.MaximumIsChanged)
        {
            return [new SearchFieldFilter(numberSearchFilter._searchField, ComparisonType.LessThanOrEqual, numberSearchFilter.Maximum)];
        }

        return [
            new SearchFieldFilter(numberSearchFilter._searchField, ComparisonType.GreaterThanOrEqual, numberSearchFilter.Minimum),
            new SearchFieldFilter(numberSearchFilter._searchField, ComparisonType.LessThanOrEqual, numberSearchFilter.Maximum),
        ];
    }
}
