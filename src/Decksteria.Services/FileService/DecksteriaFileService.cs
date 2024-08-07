﻿namespace Decksteria.Services.FileService;

using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Decksteria.Core.Models;
using Decksteria.Services.FileService.Models;

internal sealed class DecksteriaFileService : IDecksteriaFileService
{
    public async Task<Decklist> LoadDecksteriaFileAsync(Stream stream)
    {
        var deck = await JsonSerializer.DeserializeAsync<DeckFile>(stream) ?? throw new InvalidDataException();
        return new Decklist(deck.Game, deck.Format, deck.Decks);
    }

    public async Task SaveDecksteriaFileAsync(Decklist decklist, Stream stream)
    {
        var decksteriaFile = new DeckFile(decklist.Game, decklist.Format, decklist.Decks);
        var decksteriaFileJson = JsonSerializer.SerializeToUtf8Bytes(decksteriaFile);
        await stream.WriteAsync(decksteriaFileJson);
    }
}
