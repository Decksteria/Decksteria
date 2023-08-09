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

    public async Task<Decklist> LoadDecksteriaFilterAsync(MemoryStream memoryStream)
    {
        var deck = JsonSerializer.Deserialize<DeckFile>(memoryStream) ?? throw new InvalidDataException();
        var game = plugInManagerService.ChangePlugIn(deck.Game) ?? throw new KeyNotFoundException($"Game Plug-In {deck.Game} could not be found.");
        var format = plugInManagerService.ChangeFormat(deck.Format) ?? throw new KeyNotFoundException($"Format {deck.Format} is undefined in {game.DisplayName} Plug-In.");
        return await decksteriaFileMapper.ToDecklistAsync(deck);
    }

    public MemoryStream ReadDecksteriaFilter(Decklist decklist)
    {
        var decksteriaFile = decksteriaFileMapper.ToDeckFile(decklist);

        var stream = new MemoryStream();
        var decksteriaFileJson = JsonSerializer.SerializeToUtf8Bytes(decksteriaFile);
        stream.Write(decksteriaFileJson);
        return stream;
    }
}
