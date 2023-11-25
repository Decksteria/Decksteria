namespace Decksteria.Core.Models;

using System.Collections.Generic;
using Decksteria.Core;
using Decksteria.Service.DecksteriaFile.Models;

/// <summary>
/// Represents a decklist created by the user for a <see cref="IDecksteriaGame"/> and <see cref="IDecksteriaFormat"/>. Only used by the Plug-In for loading and saving a file.
/// </summary>
/// <param name="Game">The unique <see cref="IDecksteriaTile.Name"/> of the <see cref="IDecksteriaGame"/> Plug-In.</param>
/// <param name="Format">The unique <see cref="IDecksteriaTile.Name"/> of the <see cref="IDecksteriaFormat"/> Plug-In.</param>
/// <param name="Decks">All the cards in the decklist sorted by deck type.</param>
public record Decklist(string Game, string Format, IReadOnlyDictionary<string, IEnumerable<CardArtId>> Decks);
