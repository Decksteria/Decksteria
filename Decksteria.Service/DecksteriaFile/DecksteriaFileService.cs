namespace Decksteria.Service.DecksteriaFile;

using Decksteria.Core;
using Decksteria.Core.Models;
using Decksteria.Service.DecksteriaFile.Models;
using Decksteria.Service.DecksteriaPluginService;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

internal class DecksteriaFileService : IDecksteriaFileService
{
    private readonly IPlugInManagerService plugInManagerService;
    private readonly DeckFileMapper decksteriaFileMapper;

    public DecksteriaFileService(IPlugInManagerService plugInManagerService, DeckFileMapper decksteriaFileMapper)
    {
        this.plugInManagerService = plugInManagerService;
        this.decksteriaFileMapper = decksteriaFileMapper;
    }

    public async Task<Decklist> LoadDecksteriaFileAsync(Stream stream)
    {
        var deck = JsonSerializer.Deserialize<DeckFile>(stream) ?? throw new InvalidDataException();
        var game = plugInManagerService.ChangePlugIn(deck.Game) ?? throw new KeyNotFoundException($"Game Plug-In {deck.Game} could not be found.");
        var format = plugInManagerService.ChangeFormat(deck.Format) ?? throw new KeyNotFoundException($"Format {deck.Format} is undefined in {game.DisplayName} Plug-In.");
        return await decksteriaFileMapper.ToDecklistAsync(deck);
    }

    public async Task SaveDecksteriaFileAsync(Decklist decklist, Stream stream)
    {
        var decksteriaFile = decksteriaFileMapper.ToDeckFile(decklist);
        var decksteriaFileJson = JsonSerializer.SerializeToUtf8Bytes(decksteriaFile);
        await stream.WriteAsync(decksteriaFileJson);
    }
}
