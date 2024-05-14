namespace Decksteria.Ui.Maui.Pages.Search.Fields;

using System.Collections.Generic;
using Decksteria.Core.Models;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using UraniumUI.Material.Controls;

internal sealed class FilterTextField : TextField
{
    private readonly List<ComparisonOption> comparisonOptions =
    [
        new() { ComparisonType = ComparisonType.Contains, Symbol = '\u2282', Detail = "Contains" },
        new() { ComparisonType = ComparisonType.Equals, Symbol = '=', Detail = "Equals" },
        new() { ComparisonType = ComparisonType.NotContains, Symbol = '\u2284', Detail = "Not Contains" },
        new() { ComparisonType = ComparisonType.NotEquals, Symbol = '\u2260', Detail = "Not Equal" },
        new() { ComparisonType = ComparisonType.StartsWith, Symbol = '\u21D0', Detail = "Starts with" },
        new() { ComparisonType = ComparisonType.EndsWith, Symbol = '\u21D2', Detail = "Ends with" }
    ];

    private Picker ComparisonPicker = new();

    public FilterTextField() : base()
    {
        rootGrid.AddColumnDefinition(new ColumnDefinition(GridLength.Auto));
        rootGrid.Add(ComparisonPicker, column: 3);

        ComparisonPicker.WidthRequest = 63;
        ComparisonPicker.ItemsSource = comparisonOptions;
        ComparisonPicker.ItemDisplayBinding = new Binding(nameof(ComparisonOption.Label));
        ComparisonPicker.SelectedIndex = 0;
    }
}
