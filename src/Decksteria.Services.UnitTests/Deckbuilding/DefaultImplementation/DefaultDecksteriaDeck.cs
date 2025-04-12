namespace Decksteria.Services.UnitTests.Deckbuilding.DefaultImplementation;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core;


internal sealed class DefaultDecksteriaDeck : IDecksteriaDeck
{
    public string Name => "DefaultDeck";

    public string DisplayName => "Default Deck";

    public Task<bool> IsCardCanBeAddedAsync(long cardId, IEnumerable<long> cards, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }

    public Task<bool> IsDeckValidAsync(IEnumerable<long> cards, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("This method needs to be mocked.");
    }
}