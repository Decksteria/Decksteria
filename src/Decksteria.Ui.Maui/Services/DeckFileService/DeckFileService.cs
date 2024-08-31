namespace Decksteria.Ui.Maui.Services.DeckFileService;

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Models;
using Decksteria.Services.DeckFileService;
using Decksteria.Services.PlugInFactory.Models;

internal sealed class DeckFileService : IDeckFileService
{
    private readonly string gameName;

    private readonly string format;

    private readonly IDecksteriaDeckFileService deckFileService;

    private string BaseDirectory => @$"{gameName}\{format}\";

    public DeckFileService(GameFormat gameFormat, IDecksteriaDeckFileService deckFileService)
    {
        gameName = gameFormat.GameName;
        format = gameFormat.Format.Name;
        this.deckFileService = deckFileService;
    }

    public async Task<Decklist> ReadDecklistAsync(string deckName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var jsonString = await File.ReadAllTextAsync(@$"{BaseDirectory}\{deckName}", cancellationToken);
        var decklist = deckFileService.ReadDeckFileJson(jsonString);
        return decklist;
    }

    public async Task SaveDecklistAsync(string deckName, IDictionary<string, IEnumerable<CardArtId>> decks, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var decklist = new Decklist(gameName, format, decks.AsReadOnly());
        var decklistJson = deckFileService.CreateDeckFileJson(decklist);
        using var streamWriter = File.CreateText(@$"{BaseDirectory}\{deckName}");
        await streamWriter.WriteLineAsync(decklistJson);
        return;
    }
}
