namespace Decksteria.Services.UnitTests.Deckbuilding;

using Decksteria.Core;
using Decksteria.Core.Models;
using Decksteria.Services.UnitTests.Deckbuilding.DefaultImplementation;
using NSubstitute;


public sealed class LoadDecklistShould
{
    private static readonly DefaultDecksteriaCard cardInfo = new()
    {
        CardId = 0,
        Arts =
        [
            new DefaultDecksteriaCardArt(0),
            new DefaultDecksteriaCardArt(1)
        ]
    };

    private static readonly IEnumerable<CardArtId> deck = [
        new(CardId: 0, ArtId: 0),
        new(CardId: 0, ArtId: 1),
        new(CardId: 1, ArtId: 0),
        new(CardId: 2, ArtId: 0),
        new(CardId: 3, ArtId: 0),
    ];

    [Fact]
    public async Task LoadDecklist_ShouldHaveCount()
    {
        var serviceBuilder = new DeckbuildingServiceBuilder();
        serviceBuilder.DecksteriaFormat.GetCardAsync(0, Arg.Any<CancellationToken>()).Returns(Task.FromResult<IDecksteriaCard>(cardInfo));
        var service = serviceBuilder.Build();
        
        var deckName = serviceBuilder.DecksteriaDeck.Name;

        var decklist = new Dictionary<string, IEnumerable<CardArtId>>()
        {
            {deckName, deck}
        };
        await service.LoadDecklistAsync(new(serviceBuilder.GameName, serviceBuilder.DecksteriaFormat.Name, decklist));

        var id0Count = service.GetCardCountFromDeck(0, deckName);
        Assert.Equal(2, id0Count);

        var id1Count = service.GetCardCountFromDeck(1, deckName);
        Assert.Equal(1, id1Count);

        var id2Count = service.GetCardCountFromDeck(2, deckName);
        Assert.Equal(1, id2Count);

        var id3Count = service.GetCardCountFromDeck(3, deckName);
        Assert.Equal(1, id3Count);
    }

    [Fact]
    public async Task LoadDecklist_ShouldHaveDeckCount()
    {
        var serviceBuilder = new DeckbuildingServiceBuilder();
        serviceBuilder.DecksteriaFormat.GetCardAsync(0, Arg.Any<CancellationToken>()).Returns(Task.FromResult<IDecksteriaCard>(cardInfo));
        var service = serviceBuilder.Build();
        
        var deckName = serviceBuilder.DecksteriaDeck.Name;

        var decklist = new Dictionary<string, IEnumerable<CardArtId>>()
        {
            {deckName, deck}
        };
        await service.LoadDecklistAsync(new(serviceBuilder.GameName, serviceBuilder.DecksteriaFormat.Name, decklist));

        var deckResult = service.GetDeckCards(deckName);
        var count = deckResult?.Count();

        Assert.Equal(deck.Count(), count);
    }

    [Fact]
    public async Task LoadDecklist_ShouldOverwrite()
    {
        var cardId10Info = new DefaultDecksteriaCard()
        {
            CardId = 10,
            Arts =
            [
                new DefaultDecksteriaCardArt(0),
                new DefaultDecksteriaCardArt(1)
            ]
        };
        
        var serviceBuilder = new DeckbuildingServiceBuilder();
        serviceBuilder.DecksteriaFormat.GetCardAsync(0, Arg.Any<CancellationToken>()).Returns(Task.FromResult<IDecksteriaCard>(cardInfo));
        serviceBuilder.DecksteriaFormat.GetCardAsync(10, Arg.Any<CancellationToken>()).Returns(Task.FromResult<IDecksteriaCard>(cardId10Info));
        var service = serviceBuilder.Build();
        
        var deckName = serviceBuilder.DecksteriaDeck.Name;

        var decklist = new Dictionary<string, IEnumerable<CardArtId>>()
        {
            {deckName, deck}
        };
        await service.LoadDecklistAsync(new(serviceBuilder.GameName, serviceBuilder.DecksteriaFormat.Name, decklist));

        var id0Count = service.GetCardCountFromDeck(0, deckName);
        Assert.Equal(2, id0Count);

        var id1Count = service.GetCardCountFromDeck(1, deckName);
        Assert.Equal(1, id1Count);

        var id2Count = service.GetCardCountFromDeck(2, deckName);
        Assert.Equal(1, id2Count);

        var id3Count = service.GetCardCountFromDeck(3, deckName);
        Assert.Equal(1, id3Count);

        var newDeck = new CardArtId[] {
            new(CardId: 10, ArtId: 0),
            new(CardId: 10, ArtId: 1),
            new(CardId: 11, ArtId: 0),
            new(CardId: 12, ArtId: 0),
            new(CardId: 13, ArtId: 0),
        };

        decklist = new Dictionary<string, IEnumerable<CardArtId>>()
        {
            {deckName, newDeck}
        };

        await service.LoadDecklistAsync(new(serviceBuilder.GameName, serviceBuilder.DecksteriaFormat.Name, decklist));

        id0Count = service.GetCardCountFromDeck(0, deckName);
        Assert.Equal(0, id0Count);

        id1Count = service.GetCardCountFromDeck(1, deckName);
        Assert.Equal(0, id1Count);

        id2Count = service.GetCardCountFromDeck(2, deckName);
        Assert.Equal(0, id2Count);

        id3Count = service.GetCardCountFromDeck(3, deckName);
        Assert.Equal(0, id3Count);

        var id10Count = service.GetCardCountFromDeck(10, deckName);
        Assert.Equal(2, id10Count);

        var id11Count = service.GetCardCountFromDeck(11, deckName);
        Assert.Equal(1, id11Count);

        var id12Count = service.GetCardCountFromDeck(12, deckName);
        Assert.Equal(1, id12Count);

        var id13Count = service.GetCardCountFromDeck(13, deckName);
        Assert.Equal(1, id13Count);
    }
}
