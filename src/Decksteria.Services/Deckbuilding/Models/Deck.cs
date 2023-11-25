namespace Decksteria.Service.Deckbuilding.Models;

using System.Collections.Generic;
using Decksteria.Services.DecksteriaFile.Models;

internal record Deck
{
    public string Name { get; init; } = string.Empty;

    public string Label { get; init; } = string.Empty;

    public IEnumerable<CardArt>? Cards { get; init; }
}
