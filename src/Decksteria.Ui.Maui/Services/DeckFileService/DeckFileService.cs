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
using Microsoft.Maui.Storage;

internal sealed class DeckFileService : IDeckFileService
{
    private readonly string gameName;

    private readonly IDecksteriaFormat format;

    private readonly string formatName;

    private readonly IDecksteriaDeckFileService deckFileService;

    private readonly IReadOnlyDictionary<string, IDecksteriaExport> exporters;

    private readonly IReadOnlyDictionary<string, IDecksteriaImport> importers;

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
        _ = CreateMissingDirectories(filePath);
        using var fileStream = new FileStream(filePath, FileMode.Create);
        await memoryStream.CopyToAsync(fileStream, cancellationToken);
    }

    public Task<IEnumerable<string>> GetSavedDecksAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var baseDirectory = GetDeckFilePath();
        var filePaths = Directory.EnumerateFiles(baseDirectory, "*.json", new EnumerationOptions
        {
            MatchType = MatchType.Simple,
            MatchCasing = MatchCasing.CaseInsensitive,
            RecurseSubdirectories = false
        });
        var deckNames = filePaths.Select(s => Path.GetFileNameWithoutExtension(s));
        return Task.FromResult(deckNames);
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
        var deckFilePath = GetDeckFilePath(deckName);
        var jsonString = await File.ReadAllTextAsync(deckFilePath, cancellationToken);
        var decklist = deckFileService.ReadDeckFileJson(jsonString) ?? throw new FileLoadException("File could not be read.", deckFilePath);
        return decklist;
    }

    public async Task SaveDecklistAsync(string deckName, IDictionary<string, IEnumerable<CardArtId>> decks, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var decklist = new Decklist(gameName, formatName, decks.AsReadOnly());
        var decklistJson = deckFileService.CreateDeckFileJson(decklist, true);
        var deckFilePath = GetDeckFilePath(deckName);

        // Create File
        _ = CreateMissingDirectories(deckFilePath);
        using var streamWriter = File.CreateText(deckFilePath);
        await streamWriter.WriteLineAsync(decklistJson);
        return;
    }

    private static string CreateMissingDirectories(string filePath)
    {
        var directoryPath = Path.GetDirectoryName(filePath);
        if (directoryPath is null)
        {
            return string.Empty;
        }

        if (!Path.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        return directoryPath;
    }

    private string GetDeckFilePath(string? deckName = null)
    {
        var baseDirectory = @$"{FileSystem.AppDataDirectory}\{gameName}\{formatName}\";
        if (deckName is null)
    {
            return baseDirectory;
        }

        return $@"{baseDirectory}\{deckName}.json";
    }
}
