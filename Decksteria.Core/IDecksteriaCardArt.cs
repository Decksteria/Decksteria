namespace Decksteria.Core;

public interface IDecksteriaCardArt
{
    /// <summary>
    /// The ID used by the Decksteria Plug-Ins and Applications to determine a unique artwork.
    /// </summary>
    public long ArtId { get; }

    /// <summary>
    /// The URL used by the Decksteria Application to download the Image File.
    /// </summary>
    public string DownloadUrl { get; }

    /// <summary>
    /// The Image File to save to and read from. Do not include the path.
    /// </summary>
    public string FileName { get; }
}