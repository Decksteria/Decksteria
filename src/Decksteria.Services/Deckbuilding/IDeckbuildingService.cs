namespace Decksteria.Services.Deckbuilding;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Models;
using Decksteria.Services.Deckbuilding.Models;
using Decksteria.Services.FileService.Models;

public interface IDeckbuildingService
{
    IEnumerable<DecksteriaDeck> DeckInformation { get; }

    string GameTitle { get; }

    string FormatTitle { get; }

    Task<bool> AddCardAsync(CardArt card, string? deckName = null, CancellationToken cancellationToken = default);

    Task<bool> CanAddCardAsync(long cardId, string? deckName = null, CancellationToken cancellationToken = default);

    Task ClearCardsAsync(CancellationToken cancellationToken = default);

    Decklist CreateDecklist();

    int GetCardCountFromDeck(long cardId, string deckName);

    Task<CardArt> GetCardAsync(CardArtId cardArtId, CancellationToken cancellationToken = default);

    Task<IEnumerable<CardArt>> GetCardsAsync(string searchText, IEnumerable<ISearchFieldFilter>? filters = null, CancellationToken cancellationToken = default);

    IEnumerable<CardArt>? GetDeckCards(string deckName);

    Task LoadDecklistAsync(Decklist newDecklist, CancellationToken cancellationToken = default);

    Task<IDictionary<string, List<CardArt>>> ReInitializeAsync(CancellationToken cancellationToken = default);

    Task<bool> RemoveCardAsync(CardArt card, string deckName, CancellationToken cancellationToken = default);

    Task<bool> RemoveCardAtAsync(int index, string deckName, CancellationToken cancellationToken = default);
}