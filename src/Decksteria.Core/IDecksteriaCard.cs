namespace Decksteria.Core;

using System.Collections.Generic;

/// <summary>
/// Represents a card for the Decksteria Application.
/// </summary>
public interface IDecksteriaCard
{
    /// <summary>
    /// The unique identifier used by the Decksteria plug-in and applications to determine which card
    /// is being asked for by the application.
    /// </summary>
    public long CardId { get; }

    /// <summary>
    /// All of the different artworks available to a unique card.
    /// </summary>
    public IEnumerable<IDecksteriaCardArt> Arts { get; }

    /// <summary>
    /// The name of the card to be displayed to the user.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// THe extra details such as the statistics to be displayed to the user as text.
    /// </summary>
    public string Details { get; }
}
