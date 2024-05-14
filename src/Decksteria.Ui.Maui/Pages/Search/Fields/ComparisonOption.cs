namespace Decksteria.Ui.Maui.Pages.Search.Fields;

using Decksteria.Core.Models;

internal class ComparisonOption
{
    public ComparisonType ComparisonType { get; init; }

    public string Detail { get; init; } = string.Empty;

    public char Symbol { get; init; } = char.MinValue;

    public string Label => $"{Symbol} - {Detail}";
}
