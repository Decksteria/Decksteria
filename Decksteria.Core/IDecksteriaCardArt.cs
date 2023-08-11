namespace Decksteria.Core;

public interface IDecksteriaCardArt
{
    public long ArtId { get; }

    public string DownloadUrl { get; }

    public string FileName { get; }
}