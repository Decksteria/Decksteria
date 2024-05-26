namespace Decksteria.Core.Models;

public interface ISearchFieldFilter
{
    ComparisonType Comparison { get; init; }
    SearchField SearchField { get; init; }
    object? Value { get; init; }

    bool MatchesFilter(int cardProperty);
    bool MatchesFilter(int? cardProperty);
    bool MatchesFilter(string? cardProperty);
}