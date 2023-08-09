namespace Decksteria.Service.Deckbuilding;

using Decksteria.Core;
using Decksteria.Core.Models;

internal class DeckbuildingService
{
    private readonly IDecksteriaGame game;

    private readonly IDecksteriaFormat format;

    private Dictionary<string, List<CardArt>> decks;

    public DeckbuildingService(IDecksteriaGame game, IDecksteriaFormat format)
    {
        this.game = game;
        this.format = format;

        decks = format.Decks.ToDictionary(deck => deck.Name, _ => new List<CardArt>());
    }

    public async Task<IEnumerable<CardArt>> GetCardsAsync(IEnumerable<SearchField>? filters = null)
    {
        var cards = await format.GetCardsAsync(filters);
        return cards.SelectMany(ToCardArts);

        IEnumerable<CardArt> ToCardArts(IDecksteriaCard<IDecksteriaCardArt> cardInfo)
        {
            return cardInfo.Arts.Select(art => new CardArt(cardInfo.CardId, art.ArtId, art.Image, cardInfo.Details));
        }
    }

    public async Task AddCardAsync(CardArt card, string? deckName = null)
    {
        deckName = deckName ?? format.GetDefaultDeck(card.CardId).Name;
        var cards = decks[deckName];
        cards.Add(card);
    }
}
