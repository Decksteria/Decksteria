namespace Decksteria.Service.Deckbuilding;

using Decksteria.Core;
using Decksteria.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

internal sealed class DeckbuildingService(IDecksteriaGame game, IDecksteriaFormat format) : IDeckbuildingService
{
    private readonly IDecksteriaGame game = game;

    private readonly IDecksteriaFormat format = format;

    private ReadOnlyDictionary<IDecksteriaDeck, List<CardArt>> decklist = format.Decks.ToDictionary(deck => deck, _ => new List<CardArt>()).AsReadOnly();

    public Task ReInitializeAsync()
    {
        decklist = format.Decks.ToDictionary(deck => deck, _ => new List<CardArt>()).AsReadOnly();
        return Task.CompletedTask;
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

    public async Task<IEnumerable<CardArt>> GetCardsAsync(IEnumerable<SearchFieldFilter>? filters = null, CancellationToken cancellationToken = default)
    {
        var cards = await format.GetCardsAsync(filters, cancellationToken);
        return cards.SelectMany(ToCardArts);

        IEnumerable<CardArt> ToCardArts(IDecksteriaCard cardInfo)
        {
            return cardInfo.Arts.Select(art => new CardArt(cardInfo.CardId, art.ArtId, art.DownloadUrl, art.FileName, cardInfo.Details));
        }
    }

    public async Task<bool> AddCardAsync(CardArt card, string? deckName = null, CancellationToken cancellationToken = default)
    {
        var decks = SelectAsLong(decklist);
        if (await format.CheckCardCountAsync(card.CardId, decks, cancellationToken))
        {
            return false;
        }

        var deck = deckName != null ? format.GetDeckFromName(deckName) : await format.GetDefaultDeckAsync(card.CardId, cancellationToken);
        if (deck == null || await deck.IsCardCanBeAddedAsync(card.CardId, decks[deck.Name], cancellationToken))
        {
            return false;
        }

        var cards = decklist[deck];
        cards.Add(card);
        return true;
    }

    public Task<IEnumerable<CardArt>?> GetDeckCardsAsync(string deckName, CancellationToken cancellationToken = default)
    {
        var deck = format.GetDeckFromName(deckName);
        if (deck == null)
        {
            return Task.FromResult<IEnumerable<CardArt>?>(null);
        }

        cancellationToken.ThrowIfCancellationRequested();
        var cards = decklist.GetValueOrDefault(deck);
        return Task.FromResult(cards?.AsEnumerable());
    }

    public Decklist CreateDecklist() => new(game.Name, format.Name, decklist.ToDictionary(kv => kv.Key.Name, kv => kv.Value.AsEnumerable()));

    public async Task<bool> RemoveCardAsync(CardArt card, string deckName, CancellationToken cancellationToken = default)
    {
        var deck = await GetDecklistAsync(deckName, cancellationToken);
        return deck?.Remove(card) ?? false;
    }

    public async Task<bool> RemoveCardAtAsync(int index, string deckName, CancellationToken cancellationToken = default)
    {
        var deck = await GetDecklistAsync(deckName, cancellationToken);
        deck?.RemoveAt(index);
        return deck != null;
    }

    private static ReadOnlyDictionary<string, IEnumerable<long>> SelectAsLong(IReadOnlyDictionary<IDecksteriaDeck, List<CardArt>> decks)
    {
        return decks.ToDictionary(kv => kv.Key.Name, kv => kv.Value.Select(card => card.CardId)).AsReadOnly();
    }

    private Task<List<CardArt>?> GetDecklistAsync(string deckName, CancellationToken cancellationToken = default)
    {
        var deck = format.GetDeckFromName(deckName);
        if (deck == null)
        {
            return Task.FromResult<List<CardArt>?>(null);
        }

        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<List<CardArt>?>(decklist[deck]);
    }
}
