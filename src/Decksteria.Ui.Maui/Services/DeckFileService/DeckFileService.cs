namespace Decksteria.Ui.Maui.Services.DeckFileService;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core;
using Decksteria.Core.Models;
using Decksteria.Services.DeckFileService;
using Decksteria.Services.PlugInFactory.Models;

internal sealed class DeckFileService : IDeckFileService
{
    private readonly string gameName;

    private readonly IDecksteriaFormat format;

    private readonly string formatName;

    private readonly IDecksteriaDeckFileService deckFileService;

    private readonly IReadOnlyDictionary<string, IDecksteriaExport> exporters;

    private readonly IReadOnlyDictionary<string, IDecksteriaImport> importers;

    private string BaseDirectory => @$"{gameName}\{formatName}\";

    public DeckFileService(GameFormat gameFormat, IDecksteriaDeckFileService deckFileService)
    {
        gameName = gameFormat.GameName;
        format = gameFormat.Format;
        formatName = gameFormat.Format.Name;
        this.deckFileService = deckFileService;
        exporters = gameFormat.Game.Exporters.ToDictionary(e => e.Name);
        importers = gameFormat.Game.Importers.ToDictionary(e => e.Name);
    }

    public async Task ExportDecklistAsync(string filePath, string exportFormat, IDictionary<string, IEnumerable<CardArtId>> decks, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Get Exporter
        var exporter = exporters.GetValueOrDefault(exportFormat) ?? throw new InvalidOperationException($"{exportFormat} is not a valid Exporter Option.");

        // Build Decklist Model
        var decklist = new Decklist(gameName, formatName, decks.AsReadOnly());
        using var memoryStream = await exporter.SaveDecklistAsync(decklist, format, cancellationToken);

        // Save File
        memoryStream.Position = 0;
        using var fileStream = new FileStream(filePath, FileMode.Create);
        await memoryStream.CopyToAsync(fileStream, cancellationToken);
    }

    public async Task<Decklist> ImportDecklistAsync(string filePath, string importFormat, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Get Importer
        var importer = importers.GetValueOrDefault(importFormat) ?? throw new InvalidOperationException($"{importFormat} is not a valid Importer Option.");

        // Read File
        using var fileStream = new FileStream(filePath, FileMode.Open);
        using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream, cancellationToken);
        return await importer.LoadDecklistAsync(memoryStream, format, cancellationToken);
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
        var decklist = new Decklist(gameName, formatName, decks.AsReadOnly());
        var decklistJson = deckFileService.CreateDeckFileJson(decklist);
        using var streamWriter = File.CreateText(@$"{BaseDirectory}\{deckName}");
        await streamWriter.WriteLineAsync(decklistJson);
        return;
    }
}
