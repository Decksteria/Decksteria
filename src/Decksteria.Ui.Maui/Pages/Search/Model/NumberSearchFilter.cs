namespace Decksteria.Ui.Maui.Pages.Search.Model;

using System;
using System.ComponentModel;
using Decksteria.Core.Models;
using Decksteria.Services.Deckbuilding.Models;
using Decksteria.Ui.Maui.Shared.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

internal sealed class NumberSearchFilter : IMauiSearchFilter
{
    private const double MinimumFieldWidth = 60;

    private readonly SearchField _searchField;

    public NumberSearchFilter(SearchField searchField)
    {
        if (searchField.FieldType is not FieldType.Number)
        {
            throw new InvalidEnumArgumentException($"Only {FieldType.Number} can be a {nameof(NumberSearchFilter)}.");
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
    /// The lower limit that the user is allowed to select for <see cref="MinimumValue"/>
    /// </summary>
    public int Minimum => _searchField.MinValue;

    /// <summary>
    /// The upper limit that the user is allowed to select for <see cref="MaximumValue"/>
    /// </summary>
    public int Maximum => _searchField.MaxValue;

    /// <summary>
    /// The Title of the Field.
    /// </summary>
    public string Title => _searchField.FieldName;

    public double WidthRequest { get; }

    private bool MinimumIsChanged => Minimum != MinimumValue;

    private bool MaximumIsChanged => Maximum != MaximumValue;

    public ISearchFieldFilter[] AsSearchFieldFilterArray()
    {
        if (!MinimumIsChanged && !MaximumIsChanged)
        {
            return [];
        }

        if (MinimumIsChanged)
        {
            return [new NumberFieldFilter(ComparisonType.GreaterThanOrEqual, _searchField, MinimumValue),];
        }

        if (MaximumIsChanged)
        {
            return [new NumberFieldFilter(ComparisonType.LessThanOrEqual, _searchField, MaximumValue)];
        }

        return [
            new NumberFieldFilter(ComparisonType.GreaterThanOrEqual, _searchField, MinimumValue),
            new NumberFieldFilter(ComparisonType.LessThanOrEqual, _searchField, MaximumValue),
        ];
    }

    public VisualElement GetVisualElement()
    {
        var horizontalStack = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center
        };

        var stackControlMargins = new Thickness(5, 0);
        var lowerRangeField = new NumericField
        {
            Margin = stackControlMargins,
            //Value = _searchField.MinValue,
            Title = "Min"
        };
        lowerRangeField.SetBinding(NumericField.MinProperty, new Binding(nameof(NumberSearchFilter.Minimum), BindingMode.OneWay, source: this));
        lowerRangeField.SetBinding(NumericField.MaxProperty, new Binding(nameof(NumberSearchFilter.MaximumValue), BindingMode.OneWay, source: this));
        lowerRangeField.SetBinding(NumericField.WidthRequestProperty, new Binding(nameof(NumberSearchFilter.WidthRequest), BindingMode.OneWay, source: this));
        lowerRangeField.SetBinding(NumericField.MinimumWidthRequestProperty, new Binding(nameof(NumberSearchFilter.WidthRequest), BindingMode.OneWay, source: this));
        lowerRangeField.SetBinding(NumericField.ValueProperty, new Binding(nameof(NumberSearchFilter.MinimumValue), BindingMode.TwoWay, source: this));

        var lessThanSymbolLabel = new Label
        {
            Text = "\u2264",
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            FontAttributes = FontAttributes.Bold,
            Margin = stackControlMargins
        };

        var titleLabel = new Label
        {
            Margin = stackControlMargins,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            FontAttributes = FontAttributes.Bold,
        };

        titleLabel.SetValue(Label.FontSizeProperty, "Medium");
        titleLabel.SetBinding(Label.TextProperty, new Binding(nameof(NumberSearchFilter.Title), BindingMode.OneWay, source: this));

        var lessThanSymbolLabel2 = new Label
        {
            Text = "\u2264",
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            FontAttributes = FontAttributes.Bold,
            Margin = stackControlMargins
        };

        var upperRangeField = new NumericField
        {
            Margin = stackControlMargins,
            //Value = _searchField.MaxValue,
            Title = "Min"
        };
        upperRangeField.SetBinding(NumericField.MinProperty, new Binding(nameof(NumberSearchFilter.MinimumValue), BindingMode.OneWay, source: this));
        upperRangeField.SetBinding(NumericField.MaxProperty, new Binding(nameof(NumberSearchFilter.Maximum), BindingMode.OneWay, source: this));
        upperRangeField.SetBinding(NumericField.WidthRequestProperty, new Binding(nameof(NumberSearchFilter.WidthRequest), BindingMode.OneWay, source: this));
        upperRangeField.SetBinding(NumericField.MinimumWidthRequestProperty, new Binding(nameof(NumberSearchFilter.WidthRequest), BindingMode.OneWay, source: this));
        upperRangeField.SetBinding(NumericField.ValueProperty, new Binding(nameof(NumberSearchFilter.MaximumValue), BindingMode.TwoWay, source: this));

        horizontalStack.Add(lowerRangeField);
        horizontalStack.Add(lessThanSymbolLabel);
        horizontalStack.Add(titleLabel);
        horizontalStack.Add(lessThanSymbolLabel2);
        horizontalStack.Add(upperRangeField);

        return horizontalStack;
    }
}
