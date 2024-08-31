namespace Decksteria.Services.DeckFileService;

using System.Text.Json;
using Decksteria.Core.Models;
using Decksteria.Services.DeckFileService.Models;

internal sealed class DecksteriaDeckFileService : IDecksteriaDeckFileService
{
    public Decklist ReadDeckFileJson(string deckFileJson)
    {
        var deck = JsonSerializer.Deserialize<DeckFile>(deckFileJson);
        return new Decklist(deck.Game, deck.Format, deck.Decks);
    }

    public string CreateDeckFileJson(Decklist decklist)
    {
        var decksteriaFile = new DeckFile(decklist.Game, decklist.Format, decklist.Decks);
        var decksteriaFileJson = JsonSerializer.Serialize<DeckFile>(decksteriaFile);
        return decksteriaFileJson;
    }
}
