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

    public Task<IEnumerable<IDecksteriaCard<IDecksteriaCardArt>>> GetCardsAsync(IEnumerable<SearchField>? filters = null);

    public Dictionary<string, int> GetDeckStats(IDictionary<IDecksteriaDeck, IEnumerable<long>> decklist, bool isDetailed);

    public int CompareCards(long cardId1, long cardId2);

    public IDecksteriaDeck GetDefaultDeck(long card);
}