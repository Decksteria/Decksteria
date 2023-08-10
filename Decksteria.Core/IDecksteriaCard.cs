namespace Decksteria.Core;

public interface IDecksteriaCard
{
    public long CardId { get; }

    public IEnumerable<IDecksteriaCardArt> Arts { get; }

    public string Details { get; }
}
