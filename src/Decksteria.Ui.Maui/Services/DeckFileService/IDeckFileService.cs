namespace Decksteria.Ui.Maui.Services.DeckFileService;

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Models;

public interface IDeckFileService
{
    public Task<MemoryStream> ExportDecklistAsync(string exportFormat, Decklist decklist, CancellationToken cancellationToken = default);

    public IDictionary<string, string> GetExportFileTypes();

    public IDictionary<string, string> GetImportFileTypes();

    public Task<IEnumerable<string>> GetSavedDecksAsync(CancellationToken cancellationToken = default);

    public Task<Decklist> ImportDecklistAsync(string filePath, string importFormat, CancellationToken cancellationToken = default);

    public Task<Decklist> ReadDecklistAsync(string deckName, CancellationToken cancellationToken = default);

    public Task SaveDecklistAsync(string deckName, IDictionary<string, IEnumerable<CardArtId>> decks, CancellationToken cancellationToken = default);
}