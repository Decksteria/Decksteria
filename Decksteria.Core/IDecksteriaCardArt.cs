namespace Decksteria.Core;

public interface IDecksteriaCardArt
{
    public long ArtId { get; }

    public byte[] Image { get; }
}