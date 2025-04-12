namespace Decksteria.Services.UnitTests.Deckbuilding.DefaultImplementation;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core;
using Decksteria.Core.Models;

internal sealed class DefaultDecksteriaFormat : IDecksteriaFormat
{
    public string Name => "TestFormat";

    public IEnumerable<IDecksteriaDeck> Decks => [new DefaultDecksteriaDeck()];

    public IEnumerable<SearchField> SearchFields => [];

    public string DisplayName => "Test Format";

    public byte[]? Icon => throw new NotImplementedException();

    public string Description => throw new NotImplementedException();

    internal int DefaultMaximumCardCount {get;set;} = 2;

    public Task<bool> CheckCardCountAsync(long cardId, IReadOnlyDictionary<string, IEnumerable<long>> decklist, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("This method needs to be mocked.");
    }

    public Task<int> CompareCardsAsync(long cardId1, long cardId2, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("This method needs to be mocked.");
    }

    public Task<IDecksteriaCard> GetCardAsync(long cardId, CancellationToken cancellationToken = default)
    {
        var card = new DefaultDecksteriaCard
        {
            CardId = cardId
        };

        return Task.FromResult<IDecksteriaCard>(card);
    }

    public Task<IQueryable<IDecksteriaCard>> GetCardsAsync(IEnumerable<ISearchFieldFilter>? filters = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("This method needs to be mocked.");
    }

    public Task<IEnumerable<DeckStatisticSection>> GetDeckStatsAsync(IReadOnlyDictionary<string, IEnumerable<long>> decklist, bool isDetailed, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("This method needs to be mocked.");
    }

    public Task<IDecksteriaDeck> GetDefaultDeckAsync(long cardId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Decks.First());
    }

    public Task<bool> IsDecklistLegalAsync(IReadOnlyDictionary<string, IEnumerable<long>> decklist, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}