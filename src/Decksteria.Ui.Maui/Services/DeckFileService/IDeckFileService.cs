namespace Decksteria.Ui.Maui.Services.DeckFileService;

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Models;

public interface IDeckFileService
{
    Task<MemoryStream> ExportDecklistAsync(string exportFormat, Decklist decklist, CancellationToken cancellationToken = default);

    IDictionary<string, string> GetExportFileTypes();
    
    IDictionary<string, string> GetImportFileTypes();

    Task<IEnumerable<string>> GetSavedDecksAsync(CancellationToken cancellationToken = default);

    Task<Decklist> ImportDecklistAsync(string filePath, string importFormat, CancellationToken cancellationToken = default);

    Task<Decklist> ReadDecklistAsync(string deckName, CancellationToken cancellationToken = default);

    Task SaveDecklistAsync(string deckName, IDictionary<string, IEnumerable<CardArtId>> decks, CancellationToken cancellationToken = default);
}