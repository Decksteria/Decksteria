namespace Decksteria.Ui.Maui.Services.DeckFileService;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Models;

public interface IDeckFileService
{
    Task<IEnumerable<string>> GetSavedDecksAsync(CancellationToken cancellationToken = default);

    Task<Decklist> ReadDecklistAsync(string deckName, CancellationToken cancellationToken = default);

    Task SaveDecklistAsync(string deckName, IDictionary<string, IEnumerable<CardArtId>> decks, CancellationToken cancellationToken = default);
}