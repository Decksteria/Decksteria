namespace Decksteria.Core;

public interface IDecksteriaDeck
{
    public string Name { get; }

    public string DisplayName { get; }

    public Task<bool> IsCardCanBeAddedAsync(long cardId, CancellationToken cancellationToken = default);
}