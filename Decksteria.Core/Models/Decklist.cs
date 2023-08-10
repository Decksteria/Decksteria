namespace Decksteria.Core.Models;

using Decksteria.Core;

/// <summary>
/// Represents a decklist created by the user for a Game and Format.
/// </summary>
/// <param name="Game">The unique name of the Game Plug-In.</param>
/// <param name="Format">The unique name of the Format inside the Game Plug-In.</param>
/// <param name="Decks">All the cards in the decklist sorted by deck type.</param>
public record Decklist(string Game, string Format, IReadOnlyDictionary<IDecksteriaDeck, IEnumerable<CardArt>> Decks);
