namespace Decksteria.Services.Deckbuilding;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core;
using Decksteria.Core.Models;
using Decksteria.Services.Deckbuilding.Models;
using Decksteria.Services.DeckFileService.Models;

internal sealed class DeckbuildingService<T> : IDeckbuildingService<T>
    where T : IDecksteriaFormat
{
    private readonly IDecksteriaGame game;

    private readonly T format;

    private ReadOnlyDictionary<string, List<CardArt>> decklist;

    public IEnumerable<DecksteriaDeck> DeckInformation => format.Decks.Select(f => new DecksteriaDeck(f));

    public string GameTitle => game.DisplayName;

    public string FormatName => format.Name;

    public string FormatTitle => format.DisplayName;

    public DeckbuildingService(IDecksteriaGame game, T format)
    {
        this.game = game;
        this.format = format;
        decklist = format.Decks.ToDictionary(deck => deck.Name, _ => new List<CardArt>()).AsReadOnly();
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

    public Decklist CreateDecklist()
    {
        return new(game.GetType().Name, format.Name, decklist.ToDictionary(kv => kv.Key, kv => kv.Value.Cast<CardArtId>()));
    }

    public async Task<CardArt> GetCardAsync(CardArtId cardArtId, CancellationToken cancellationToken = default)
    {
        var card = await format.GetCardAsync(cardArtId.CardId, cancellationToken);
        var art = card.Arts.FirstOrDefault(a => a.ArtId == cardArtId.ArtId) ?? throw new NullReferenceException("An artwork with the specified ID could not be found.");
        return GetCardArtFromCard(card, art);
    }

    public async Task<IEnumerable<CardArt>> GetCardsAsync(string? searchText = null, IEnumerable<ISearchFieldFilter>? filters = null, CancellationToken cancellationToken = default)
    {
        var cards = await format.GetCardsAsync(filters, cancellationToken);
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            cards = cards.Where(c => c.Details.Contains(searchText, StringComparison.InvariantCultureIgnoreCase));
        }

        return cards.SelectMany(ToCardArts);

        IEnumerable<CardArt> ToCardArts(IDecksteriaCard cardInfo)
        {
            return cardInfo.Arts.Select(art => GetCardArtFromCard(cardInfo, art));
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

    public async Task<IEnumerable<DeckStatisticSection>> GetDeckStatsAsync(bool detailed = false, CancellationToken cancellationToken = default)
    {
        var decklistIds = decklist.ToDictionary(d => d.Key, d => d.Value.Select(c => c.CardId)).AsReadOnly();
        var dictionaryCounts = await format.GetDeckStatsAsync(decklistIds, detailed, cancellationToken);
        return dictionaryCounts.Select(SortStatisticSection);

        static DeckStatisticSection SortStatisticSection(DeckStatisticSection section)
        {
            if (!section.OrderByCount)
            {
                return section;
            }

            return new DeckStatisticSection
            {
                Label = section.Label,
                OrderByCount = section.OrderByCount,
                Statistics = section.Statistics.SortValues()
            };
        }
    }

    public async Task LoadDecklistAsync(Decklist newDecklist, CancellationToken cancellationToken = default)
    {
        foreach (var deck in decklist)
        {
            var name = deck.Key;
            var cardList = deck.Value;

            // Clear the existing decklist because we're loading up the decklist from scratch.
            cardList.Clear();

            // Skip this deck if the loaded decklist doesn't contain any cards for this deck.
            newDecklist.Decks.TryGetValue(name, out var cards);
            if (cards is null || !cards.Any())
            {
                continue;
            }

            // Populate deck with new cards
            var convertTasks = cards.Select(GetCardArtFromId);
            var newCards = await Task.WhenAll(convertTasks);
            cardList.AddRange(newCards);
        }

        Task<CardArt> GetCardArtFromId(CardArtId cardArtId)
        {
            return GetCardAsync(cardArtId, cancellationToken);
        }
    }

    public Task<IDictionary<string, List<CardArt>>> ReInitializeAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        decklist = format.Decks.ToDictionary(deck => deck.Name, _ => new List<CardArt>()).AsReadOnly();
        return Task.FromResult<IDictionary<string, List<CardArt>>>(decklist);
    }

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

    public async Task<bool> ValidDecklistAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var decklistIds = decklist.ToDictionary(d => d.Key, d => d.Value.Select(c => c.CardId)).AsReadOnly();
        foreach (var deck in format.Decks)
        {
            if (!(await deck.IsDeckValidAsync(decklistIds[deck.Name], cancellationToken)))
            {
                return false;
            }
        }
        
        return await format.IsDecklistLegalAsync(decklistIds, cancellationToken);
    }

    public async Task<(IDictionary<IDecksteriaDeck, bool>, bool)> ValidDecksAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var decklistIds = decklist.ToDictionary(d => d.Key, d => d.Value.Select(c => c.CardId)).AsReadOnly();
        var deckValidationTasks = format.Decks.ToDictionary(d => d, async d => await d.IsDeckValidAsync(decklistIds[d.Name], cancellationToken));
        await Task.WhenAll(deckValidationTasks.Select(f => f.Value));
        var deckValidationList = deckValidationTasks.ToDictionary(d => d.Key, d => d.Value.Result);
        return (deckValidationList, await format.IsDecklistLegalAsync(decklistIds, cancellationToken));
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

    private CardArt GetCardArtFromCard(IDecksteriaCard cardInfo, IDecksteriaCardArt art)
    {
        return new CardArt(cardInfo.CardId, art.ArtId, art.Image, cardInfo.Details);
    }
}
