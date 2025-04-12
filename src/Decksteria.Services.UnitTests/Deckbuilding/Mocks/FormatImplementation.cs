namespace Decksteria.Services.UnitTests.Deckbuilding.Mocks;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core;
using Decksteria.Core.Models;


public sealed class FormatImplementation : IDecksteriaFormat
{
    public string Name => "TestFormat";

    public IEnumerable<IDecksteriaDeck> Decks => [];

    public IEnumerable<SearchField> SearchFields => [];

    public string DisplayName => "Test Format";

    public byte[]? Icon => throw new NotImplementedException();

    public string Description => throw new NotImplementedException();

    internal int DefaultMaximumCardCount {get;set;} = 2;

    public Task<bool> CheckCardCountAsync(long cardId, IReadOnlyDictionary<string, IEnumerable<long>> decklist, CancellationToken cancellationToken = default)
    {
        var cardCount = decklist.SelectMany(deck => deck.Value).Count(cId => cId == cardId);
        return Task.FromResult(cardCount < DefaultMaximumCardCount);
    }

    public Task<int> CompareCardsAsync(long cardId1, long cardId2, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IDecksteriaCard> GetCardAsync(long cardId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<IDecksteriaCard>> GetCardsAsync(IEnumerable<ISearchFieldFilter>? filters = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DeckStatisticSection>> GetDeckStatsAsync(IReadOnlyDictionary<string, IEnumerable<long>> decklist, bool isDetailed, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IDecksteriaDeck> GetDefaultDeckAsync(long cardId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsDecklistLegalAsync(IReadOnlyDictionary<string, IEnumerable<long>> decklist, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}