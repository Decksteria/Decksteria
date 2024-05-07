namespace Decksteria.Core;

/// <summary>
/// Represents an individual art for a particular card.
/// </summary>
public interface IDecksteriaCardArt
{
    /// <summary>
    /// The ID used by the Decksteria plug-ins and applications to determine a unique artwork.
    /// The ID must be unique to the card that consumes the art.
    /// </summary>
    public long ArtId { get; }

    /// <summary>
    /// The URL used by the Decksteria Application to download the Image File.
    /// </summary>
    public string DownloadUrl { get; }

    /// <summary>
    /// The image file to save to and read from. Do not include the path.
    /// </summary>
    public string FileName { get; }
}