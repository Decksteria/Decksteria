namespace Decksteria.Service.Models;

using Decksteria.Core;

public record Decklist(IDecksteriaGame Game, IDecksteriaFormat Format, Dictionary<IDecksteriaDeck, IEnumerable<CardArt>> Decks);
