namespace Decksteria.Services.Deckbuilding;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core;
using Decksteria.Core.Models;
using Decksteria.Services.Deckbuilding.Models;
using Decksteria.Services.DeckFileService.Models;

public interface IDeckbuildingService
{
    public IEnumerable<DecksteriaDeck> DeckInformation { get; }

    public string GameTitle { get; }

    public string FormatName { get; }

    public string FormatTitle { get; }

    public Task<bool> AddCardAsync(CardArt card, string? deckName = null, CancellationToken cancellationToken = default);

    public Task<bool> CanAddCardAsync(long cardId, string? deckName = null, CancellationToken cancellationToken = default);

    public Task ClearCardsAsync(CancellationToken cancellationToken = default);

    public Decklist CreateDecklist();

    public int GetCardCountFromDeck(long cardId, string deckName);

    public Task<CardArt> GetCardAsync(CardArtId cardArtId, CancellationToken cancellationToken = default);

    public Task<IEnumerable<CardArt>> GetCardsAsync(string searchText, IEnumerable<ISearchFieldFilter>? filters = null, CancellationToken cancellationToken = default);

    public IEnumerable<CardArt>? GetDeckCards(string deckName);

    public Task<IEnumerable<DeckStatisticSection>> GetDeckStatsAsync(bool detailed = false, CancellationToken cancellationToken = default);

    public Task LoadDecklistAsync(Decklist newDecklist, CancellationToken cancellationToken = default);

    public Task<IDictionary<string, List<CardArt>>> ReInitializeAsync(CancellationToken cancellationToken = default);

    public Task<bool> RemoveCardAsync(CardArt card, string deckName, CancellationToken cancellationToken = default);

    public Task<bool> RemoveCardAtAsync(int index, string deckName, CancellationToken cancellationToken = default);

    public Task<bool> ValidDecklistAsync(CancellationToken cancellationToken = default);

    public Task<(IDictionary<IDecksteriaDeck, bool>, bool)> ValidDecksAsync(CancellationToken cancellationToken = default);
}