namespace Decksteria.Core;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Represents a Deck type inside a format. (e.g. Side Deck)
/// </summary>
public interface IDecksteriaDeck
{
    /// <summary>
    /// The name used by the Decksteria Application to save decklists.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The name of the Deck being displayed to the user.
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// Checks if a card can be added to this deck.
    /// </summary>
    /// <param name="cardId">The unique ID of the card that wants to be added to this deck.</param>
    /// <param name="cards">A list of Ids of cards currently in the deck.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the execution.</param>
    /// <returns>The card is a valid addition to this deck.</returns>
    public Task<bool> IsCardCanBeAddedAsync(long cardId, IEnumerable<long> cards, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if the cards inside the are legal
    /// </summary>
    /// <param name="cards">The list of IDs belonging to all cards in the deck.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the execution.</param>
    /// <returns></returns>
    public Task<bool> IsDeckValidAsync(IEnumerable<long> cards, CancellationToken cancellationToken = default);
}