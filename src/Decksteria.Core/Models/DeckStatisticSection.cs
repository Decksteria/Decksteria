namespace Decksteria.Core.Models;

using Decksteria.Core.Data;

/// <summary>
/// Represents a statistic being returned by <see cref="IDecksteriaFormat.GetDeckStatsAsync"/>.
/// </summary>
public sealed record DeckStatisticSection
{
    /// <summary>
    /// The label for the section.
    /// </summary>
    public required string Label { get; init; }

    /// <summary>
    /// Whether to sort the labels in the dictionary by the count.
    /// If not set, it will sort by the order of insertion to <see cref="Statistics"/>.
    /// </summary>
    public bool OrderByCount { get; init; } = false;

    /// <summary>
    /// The card type counts belonging to this particular section.
    /// </summary>
    public required ReadOnlyDeckStatisticDictionary Statistics { get; init; }
}
