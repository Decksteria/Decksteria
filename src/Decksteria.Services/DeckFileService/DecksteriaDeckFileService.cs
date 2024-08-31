namespace Decksteria.Services.DeckFileService;

using System.IO;
using System.Text.Json;
using Decksteria.Core.Models;
using Decksteria.Services.DeckFileService.Models;

internal sealed class DecksteriaDeckFileService : IDecksteriaDeckFileService
{
    private readonly JsonSerializerOptions CreateJsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly JsonSerializerOptions ReadJsonOptions = new()
    {
        AllowTrailingCommas = true,
        IgnoreReadOnlyFields = true,
        IgnoreReadOnlyProperties = true
    };

    public string CreateDeckFileJson(Decklist decklist, bool deckIsValid)
    {
        var decksteriaFile = new DeckFile(decklist.Game, decklist.Format, deckIsValid, decklist.Decks);
        var decksteriaFileJson = JsonSerializer.Serialize<DeckFile>(decksteriaFile, CreateJsonOptions);
        return decksteriaFileJson;
    }

    public Decklist? ReadDeckFileJson(string deckFileJson)
    {
        var deck = JsonSerializer.Deserialize<DeckFile>(deckFileJson, ReadJsonOptions);
        if (deck is null)
        {
            return null;
        }

        return new Decklist(deck.Game, deck.Format, deck.Decks);
    }
}
