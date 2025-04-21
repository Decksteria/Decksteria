namespace Decksteria.Core;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Models;

using DecklistDto = System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<long>>;

/// <summary>
/// Represents a game's format and its individual deck-building rules. (e.g. Standard vs. Commander)
/// </summary>
public interface IDecksteriaFormat : IDecksteriaTile
{
    /// <summary>
    /// The name stored inside the saved deck file.
    /// The name should be unique among all the formats.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// A list of all the different deck sections, a decklist in the format consists of.
    /// (e.g. Main Deck, Side Deck, Extra Deck)
    /// </summary>
    public IEnumerable<IDecksteriaDeck> Decks { get; }

    /// <summary>
    /// A list of all the <see cref="SearchField"/> that the user can use to perform an advanced search.
    /// </summary>
    public IEnumerable<SearchField> SearchFields { get; }

    /// <summary>
    /// Checks whether a card has reached its maximum number of copies in the decklist.
    /// </summary>
    /// <param name="cardId">The ID of the unique <see cref="IDecksteriaCard"/>.</param>
    /// <param name="decklist">The decklist created by the user.</param>
    /// <param name = "cancellationToken" > The cancellation token used to cancel the execution.</param>
    /// <returns>Returns <see cref="true"/> if it hasn't reached its maximum count yet.</returns>
    public Task<bool> CheckCardCountAsync(long cardId, DecklistDto decklist, CancellationToken cancellationToken = default);

    /// <summary>
    /// Compares two <see cref="IDecksteriaCard"/> together and determines the order.
    /// </summary>
    /// <param name="cardId1">The Id of the first <see cref="IDecksteriaCard"/> being compared.</param>
    /// <param name="cardId2">The Id second <see cref="IDecksteriaCard"/> being compared.</param>
    /// <returns>An integer determining the order of <paramref name="cardId1"/> compared to <paramref name="cardId2"/>.
    /// It returns a negative value if <paramref name="cardId1"/> precedes <paramref name="cardId2"/>.
    /// It returns a positive value if <paramref name="cardId1"/> succeeds <paramref name="cardId2"/>.
    /// It returns 0 if <paramref name="cardId1"/> and <paramref name="cardId2"/> have the same sort order.</returns>
    public Task<int> CompareCardsAsync(long cardId1, long cardId2, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all cards available in the format that fulfil the filter criteria as
    /// specified in <see cref="SearchFields"/>.
    /// </summary>
    /// <param name="filters">A set of filters used to determine if a card should be returned. All cards will be returned if no filters are defined.</param>
    /// <param name = "cancellationToken" > The cancellation token used to cancel the execution.</param>
    /// <returns>An implementation of the IQueryable that allows additional filtering based on IDecksteriaCard.</returns>
    public Task<IQueryable<IDecksteriaCard>> GetCardsAsync(IEnumerable<ISearchFieldFilter>? filters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a single card based on its ID.
    /// </summary>
    /// <param name="cardId">The ID of the card the Decksteria Application is looking for provided by the plug-in.</param>
    /// <param name = "cancellationToken" > The cancellation token used to cancel the execution.</param>
    /// <returns></returns>
    public Task<IDecksteriaCard> GetCardAsync(long cardId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the counts of each particular important type of card that the user may want to keep track of.
    /// </summary>
    /// <param name="decklist">The decklist provided by the user. The list only provides the Card ID as the Art ID should not be used for the counting.</param>
    /// <param name="isDetailed">Determines whether a more detailed count should be returned.</param>
    /// <param name = "cancellationToken" > The cancellation token used to cancel the execution.</param>
    /// <returns>A list of sections that provide the user important card types should count for the user to keep track of.</returns>
    public Task<IEnumerable<DeckStatisticSection>> GetDeckStatsAsync(DecklistDto decklist, bool isDetailed, CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines which deck a card should be added to by default, if not defined by the user.
    /// </summary>
    /// <param name="cardId">The ID of the card that is being checked.</param>
    /// <param name="cancellationToken"> The cancellation token used to cancel the execution.</param>
    /// <returns>The interface of the deck that the card should be added to by default.</returns>
    public Task<IDecksteriaDeck> GetDefaultDeckAsync(long cardId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the <see cref="IDecksteriaDeck"/> from <see cref="Decks"/> based on the name.
    /// </summary>
    /// <param name="name">The name of the deck that needs to be returned.</param>
    /// <returns>A <see cref="IDecksteriaDeck"/> that matches the deck's name.</returns>
    public IDecksteriaDeck? GetDeckFromName(string name) => Decks.FirstOrDefault(deck => deck.Name == name);

    /// <summary>
    /// Verifies whether an entire decklist is legal.
    /// </summary>
    /// <param name="decklist">The decklist to be verified.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the execution.</param>
    /// <returns></returns>
    public Task<bool> IsDecklistLegalAsync(DecklistDto decklist, CancellationToken cancellationToken = default);

    /// <summary>
    /// Used by the Plug-In to free up any intensive memory. Use this to free up any large Lists.
    /// </summary>
    public void Uninitialize()
    {

    }
}