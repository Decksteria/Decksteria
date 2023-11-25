namespace Decksteria.Services.Deckbuilding;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Models;
using Decksteria.Services.FileService.Models;

public interface IDeckbuildingService
{
    Task<bool> AddCardAsync(CardArt card, string? deckName = null, CancellationToken cancellationToken = default);

    Task ClearCardsAsync(CancellationToken cancellationToken = default);

    Decklist CreateDecklist();

    Task<IEnumerable<CardArt>> GetCardsAsync(IEnumerable<SearchFieldFilter>? filters = null, CancellationToken cancellationToken = default);

    Task<IEnumerable<CardArt>?> GetDeckCardsAsync(string deckName, CancellationToken cancellationToken = default);

    Task ReInitializeAsync();

    Task<bool> RemoveCardAsync(CardArt card, string deckName, CancellationToken cancellationToken = default);

    Task<bool> RemoveCardAtAsync(int index, string deckName, CancellationToken cancellationToken = default);
}