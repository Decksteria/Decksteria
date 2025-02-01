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

        // Use the labels as the key because .NET Maui's Display Action Sheet does not support classes
        exporters = gameFormat.Game.Exporters.ToDictionary(e => e.Label);
        importers = gameFormat.Game.Importers.ToDictionary(e => e.Label);
    }

    public async Task<MemoryStream> ExportDecklistAsync(string exportFormat, Decklist decklist, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Get Exporter
        var exporter = exporters.GetValueOrDefault(exportFormat) ?? throw new InvalidOperationException($"{exportFormat} is not a valid Exporter Option.");

        // Build Decklist Model
        var memoryStream = await exporter.SaveDecklistAsync(decklist, format, cancellationToken);

        // Reset memory stream for .NET Maui's FileSaver to use.
        memoryStream.Position = 0;
        return memoryStream;
    }

    public IDictionary<string, string> GetExportFileTypes()
    {
        return exporters.ToDictionary(e => e.Key, e => e.Value.FileType);
    }

    public IDictionary<string, string> GetImportFileTypes()
    {
        return exporters.ToDictionary(e => e.Key, e => e.Value.FileType);
    }

    public Task<IEnumerable<string>> GetSavedDecksAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var baseDirectory = GetDeckFilePath();
        var filePath = @$"{baseDirectory}/Gaius.json";
        if (!Directory.Exists(baseDirectory))
        {
            return Task.FromResult<IEnumerable<string>>(Array.Empty<string>());
        }

        var filePaths = Directory.GetFiles(baseDirectory, "*.json", new EnumerationOptions
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
        memoryStream.Position = 0;
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
        var baseDirectory = Path.Combine(FileSystem.AppDataDirectory, gameName, formatName);
        // @$"{FileSystem.AppDataDirectory}\{gameName}\{fileName}";
        if (deckName is null)
        {
            return baseDirectory;
        }

        return Path.Combine(baseDirectory, $"{deckName}.json");
    }
}
