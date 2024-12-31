namespace Decksteria.Ui.Maui.Pages.DeckStatistics;

internal sealed class StatisticInfo
{
    public required int Count { get; set; }

    public required string Label { get; set; }

    public string Text => $"{Label}: {Count}";
}
