namespace Decksteria.Core;

using Decksteria.Core.Data;

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
    /// The details used to retrieve the image for the card art.
    /// </summary>
    public DecksteriaImage Image { get; }
}