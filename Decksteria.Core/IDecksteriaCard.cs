namespace Decksteria.Core;

public interface IDecksteriaCard<out T> where T : IDecksteriaCardArt
{
    public long CardId { get; }

    public IEnumerable<T> Arts { get; }

    public string Details { get; }
}
