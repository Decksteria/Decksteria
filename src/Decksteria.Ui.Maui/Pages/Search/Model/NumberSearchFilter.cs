namespace Decksteria.Ui.Maui.Pages.Search.Model;

using System;
using Decksteria.Core.Models;
using Decksteria.Services.Deckbuilding.Models;

internal sealed class NumberSearchFilter : ISearchFilter
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

    public SearchFieldFilter[] AsSearchFieldFilterArray()
    {
        if (!MinimumIsChanged || !MaximumIsChanged)
        {
            return [];
        }

        if (MinimumIsChanged)
        {
            return [new SearchFieldFilter(_searchField, ComparisonType.GreaterThanOrEqual, Minimum),];
        }

        if (MaximumIsChanged)
        {
            return [new SearchFieldFilter(_searchField, ComparisonType.LessThanOrEqual, Maximum)];
        }

        return [
            new SearchFieldFilter(_searchField, ComparisonType.GreaterThanOrEqual, Minimum),
            new SearchFieldFilter(_searchField, ComparisonType.LessThanOrEqual, Maximum),
        ];
    }
}
