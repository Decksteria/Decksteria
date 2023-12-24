namespace Decksteria.Services.Deckbuilding.Models;

using Decksteria.Core;

public record DecksteriaDeck(IDecksteriaDeck Deck)
{
    public string Name { get; init; } = Deck.Name;

    public string Label { get; init; } = Deck.DisplayName;
}
