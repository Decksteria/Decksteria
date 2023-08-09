namespace Decksteria.Service.Deckbuilding.Models;

using Decksteria.Core.Models;

internal record Deck
{
    public string Name { get; init; } = string.Empty;

    public string Label { get; init; } = string.Empty;

    public IEnumerable<CardArt>? Cards { get; init; }
}
