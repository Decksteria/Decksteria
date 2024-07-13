namespace Decksteria.Services.Deckbuilding;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core;
using Decksteria.Core.Models;
using Decksteria.Services.Deckbuilding.Models;
using Decksteria.Services.FileService.Models;
using Decksteria.Services.PlugInFactory.Models;

internal sealed class DeckbuildingService : IDeckbuildingService
{
    private readonly IDecksteriaGame game;

    private readonly IDecksteriaFormat format;

    private ReadOnlyDictionary<string, List<CardArt>> decklist;

    public IEnumerable<DecksteriaDeck> DeckInformation => format.Decks.Select(f => new DecksteriaDeck(f));

    public string GameTitle => game.DisplayName;

    public string FormatTitle => format.DisplayName;

    public DeckbuildingService(GameFormat selectedFormat)
    {
        game = selectedFormat.Game;
        format = selectedFormat.Format;
        decklist = selectedFormat.Format.Decks.ToDictionary(deck => deck.Name, _ => new List<CardArt>()).AsReadOnly();
    }

    public async Task<bool> AddCardAsync(CardArt card, string? deckName = null, CancellationToken cancellationToken = default)
    {
        if (!await CanAddCardAsync(card.CardId, deckName, cancellationToken))
        {
            return false;
        }

        var deck = deckName != null ? format.GetDeckFromName(deckName) : await format.GetDefaultDeckAsync(card.CardId, cancellationToken);
        if (deck is null)
        {
            return false;
        }

        var cards = decklist[deck.Name];
        cards.Add(card);
        return true;
    }

    public async Task<bool> CanAddCardAsync(long cardId, string? deckName = null, CancellationToken cancellationToken = default)
    {
        var decks = SelectAsLong(decklist);
        if (!await format.CheckCardCountAsync(cardId, decks, cancellationToken))
        {
            return false;
        }

        var deck = deckName != null ? format.GetDeckFromName(deckName) : await format.GetDefaultDeckAsync(cardId, cancellationToken);
        if (deck is null)
        {
            return false;
        }

        return await deck.IsCardCanBeAddedAsync(cardId, decks[deck.Name], cancellationToken);
    }

    public Task ClearCardsAsync(CancellationToken cancellationToken = default)
    {
        foreach (var deck in decklist)
        {
            cancellationToken.ThrowIfCancellationRequested();
            deck.Value.Clear();
        }

        return Task.CompletedTask;
    }

    public async Task<IEnumerable<CardArt>> GetCardsAsync(string? searchText = null, IEnumerable<ISearchFieldFilter>? filters = null, CancellationToken cancellationToken = default)
    {
        var cards = await format.GetCardsAsync(filters, cancellationToken);
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            cards = cards.Where(c => c.Details.Contains(searchText));
        }

        return cards.SelectMany(ToCardArts);

        IEnumerable<CardArt> ToCardArts(IDecksteriaCard cardInfo)
        {
            return cardInfo.Arts.Select(art => new CardArt(cardInfo.CardId, art.ArtId, art.DownloadUrl, art.FileName, cardInfo.Details));
        }
    }

    public int GetCardCountFromDeck(long cardId, string deckName)
    {
        _ = decklist.TryGetValue(deckName, out var deck);
        return deck?.Count(card => card.CardId == cardId) ?? -1;
    }

    public IEnumerable<CardArt>? GetDeckCards(string deckName)
    {
        _ = decklist.TryGetValue(deckName, out var value);
        return value;
    }

    public Decklist CreateDecklist() => new(game.GetType().Name, format.Name, decklist.ToDictionary(kv => kv.Key, kv => kv.Value.Cast<CardArtId>()));

    public async Task<bool> RemoveCardAsync(CardArt card, string deckName, CancellationToken cancellationToken = default)
    {
        var deck = await GetDecklistAsync(deckName, cancellationToken);
        return deck?.Remove(card) ?? false;
    }

    public Task<IDictionary<string, List<CardArt>>> ReInitializeAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        decklist = format.Decks.ToDictionary(deck => deck.Name, _ => new List<CardArt>()).AsReadOnly();
        return Task.FromResult<IDictionary<string, List<CardArt>>>(decklist);
    }

    public async Task<bool> RemoveCardAtAsync(int index, string deckName, CancellationToken cancellationToken = default)
    {
        var deck = await GetDecklistAsync(deckName, cancellationToken);
        deck?.RemoveAt(index);
        return deck != null;
    }

    private static ReadOnlyDictionary<string, IEnumerable<long>> SelectAsLong(IReadOnlyDictionary<string, List<CardArt>> decks)
    {
        return decks.ToDictionary(kv => kv.Key, kv => kv.Value.Select(card => card.CardId)).AsReadOnly();
    }

    private Task<List<CardArt>?> GetDecklistAsync(string deckName, CancellationToken cancellationToken = default)
    {
        var deck = format.GetDeckFromName(deckName);
        if (deck == null)
        {
            return Task.FromResult<List<CardArt>?>(null);
        }

        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<List<CardArt>?>(decklist[deckName]);
    }
}
