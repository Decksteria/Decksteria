namespace Decksteria.Core;

using Decksteria.Core.Models;

public interface IDecksteriaFormat
{
    public string Name { get; }

    public string DisplayName { get; }

    public byte[]? Icon { get; }

    public string Description { get; }

    public IEnumerable<IDecksteriaDeck> Decks { get; }

    public IEnumerable<SearchField> SearchFields { get; }

    public Task<bool> CheckCardCountAsync(long cardId, IReadOnlyDictionary<IDecksteriaDeck, IEnumerable<long>> decklist, CancellationToken cancellationToken = default);

    public int CompareCards(long cardId1, long cardId2);

    public Task<IEnumerable<IDecksteriaCard>> GetCardsAsync(IEnumerable<SearchField>? filters = null, CancellationToken cancellationToken = default);

    public Task<IDecksteriaCard> GetCardAsync(long cardId, CancellationToken? cancellationToken = null);

    public Task<Dictionary<string, int>> GetDeckStatsAsync(IReadOnlyDictionary<IDecksteriaDeck, IEnumerable<long>> decklist, bool isDetailed, CancellationToken cancellationToken = default);

    public Task<IDecksteriaDeck> GetDefaultDeckAsync(long cardId, CancellationToken cancellationToken = default);

    public IDecksteriaDeck? GetDeckFromName(string name);
}