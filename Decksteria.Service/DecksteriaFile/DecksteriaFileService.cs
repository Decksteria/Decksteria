namespace Decksteria.Service.DecksteriaFile;

using Decksteria.Core.Models;
using Decksteria.Service.DecksteriaFile.Models;
using Decksteria.Service.DecksteriaPluginService;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

internal sealed class DecksteriaFileService(IPlugInManagerService plugInManagerService) : IDecksteriaFileService
{
    private readonly IPlugInManagerService plugInManagerService = plugInManagerService;

    public async Task<Decklist> LoadDecksteriaFileAsync(Stream stream)
    {
        var deck = JsonSerializer.Deserialize<DeckFile>(stream) ?? throw new InvalidDataException();
        var game = plugInManagerService.ChangePlugIn(deck.Game) ?? throw new KeyNotFoundException($"Game Plug-In {deck.Game} could not be found.");
        var format = plugInManagerService.ChangeFormat(deck.Format) ?? throw new KeyNotFoundException($"Format {deck.Format} is undefined in {game.DisplayName} Plug-In.");
        
        return await ToDecklistAsync(deck);

        async Task<Decklist> ToDecklistAsync(DeckFile decksteriaFile)
        {
            var mappedDecks = new Dictionary<string, IEnumerable<CardArt>>();

            foreach (var decklist in decksteriaFile.Decks)
            {
                var tasks = decklist.Value.Select(ToCardArtAsync);
                var list = await Task.WhenAll(tasks);
                mappedDecks.Add(decklist.Key, list);
            }

            return new Decklist(decksteriaFile.Game, decksteriaFile.Format, mappedDecks);
        }

        async Task<CardArt> ToCardArtAsync(FileCard fileCard)
        {
            var card = await format.GetCardAsync(fileCard.CardId);
            var art = card.Arts.First(art => art.ArtId == fileCard.ArtId);
            return new CardArt(fileCard.CardId, fileCard.ArtId, art.DownloadUrl, art.FileName, card.Details);
        }
    }

    public async Task SaveDecksteriaFileAsync(Decklist decklist, Stream stream)
    {
        var decksteriaFile = new DeckFile(decklist.Game, decklist.Format, decklist.Decks.ToDictionary(kv => kv.Key, kv => kv.Value.Select(ToFileCard)));
        var decksteriaFileJson = JsonSerializer.SerializeToUtf8Bytes(decksteriaFile);
        await stream.WriteAsync(decksteriaFileJson);

        static FileCard ToFileCard(CardArt cardArt)
        {
            return new FileCard(cardArt.CardId, cardArt.ArtId);
        }
    }
}
