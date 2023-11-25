namespace Decksteria.Services.Deckbuilding.Models;

using System.Collections.Generic;
using Decksteria.Services.FileService.Models;

internal record Deck
{
    public string Name { get; init; } = string.Empty;

    public string Label { get; init; } = string.Empty;

    public IEnumerable<CardArt>? Cards { get; init; }
}
