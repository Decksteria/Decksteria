namespace Decksteria.Core;

using System.Collections.Generic;

/// <summary>
/// Represents a card for the Decksteria Application.
/// </summary>
public interface IDecksteriaCard
{
    /// <summary>
    /// The ID used by the Decksteria Plug-Ins and Applications to determine a unique card.
    /// </summary>
    public long CardId { get; }

    /// <summary>
    /// All of the different Artworks available to a unique card.
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
