namespace Decksteria.Services.DeckFileService.Models;

using System;
using System.Collections.Generic;
using Decksteria.Core.Models;

[Serializable]
/// <summary>
/// The file format in which Decksteria Deck Files will be saved.
/// </summary>
/// <param name="Game">The unique name of the Game Plug-In.</param>
/// <param name="Format">The unique name of the Format inside the Game Plug-In.</param>
/// <param name="Decks">All the cards in the decklist sorted by deck type.</param>
internal record DeckFile(string Game, string Format, IReadOnlyDictionary<string, IEnumerable<CardArtId>> Decks);
