namespace Decksteria.Core.Models;

using Decksteria.Core.Data;

/// <summary>
/// Represents a statistic being returned by <see cref="IDecksteriaFormat.GetDeckStatsAsync"/>.
/// </summary>
public sealed class DeckStatisticMetadata
{
    /// <summary>
    /// The image icon to use to represent the statistic.
    /// </summary>
    public DecksteriaImage? Icon { get; init; }
}
