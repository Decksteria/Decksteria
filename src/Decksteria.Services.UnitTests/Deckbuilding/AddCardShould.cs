namespace Decksteria.Services.UnitTests.Deckbuilding;

using Decksteria.Services.DeckFileService.Models;
using NSubstitute;

using DecksteriaDecklistDto = IReadOnlyDictionary<string, IEnumerable<long>>;

public sealed class AddCardShould
{
    private const long CardId = 1;

    private static readonly CardArt CardToAdd = new(CardId, 0, new(string.Empty, string.Empty), string.Empty);

    [Fact]
    public async Task AddCard_CanAddCard_ShouldIncreaseCount()
    {
        var serviceBuilder = new DeckbuildingServiceBuilder();
        serviceBuilder.DecksteriaDeck.IsCardCanBeAddedAsync(Arg.Any<long>(), Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns(true);
        serviceBuilder.DecksteriaFormat.CheckCardCountAsync(Arg.Any<long>(), Arg.Any<DecksteriaDecklistDto>(), Arg.Any<CancellationToken>()).Returns(true);
        var service = serviceBuilder.Build();

        var previousCount = service.GetCardCountFromDeck(CardId, serviceBuilder.DecksteriaDeck.Name);
        await service.AddCardAsync(CardToAdd);

        var newCount = service.GetCardCountFromDeck(CardId, serviceBuilder.DecksteriaDeck.Name);
        Assert.Equal(previousCount + 1, newCount);
    }

    [Fact]
    public async Task AddCard_CantAddToDeck_ShouldNotIncreaseCount()
    {
        var serviceBuilder = new DeckbuildingServiceBuilder();
        serviceBuilder.DecksteriaDeck.IsCardCanBeAddedAsync(Arg.Any<long>(), Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns(false);
        serviceBuilder.DecksteriaFormat.CheckCardCountAsync(Arg.Any<long>(), Arg.Any<DecksteriaDecklistDto>(), Arg.Any<CancellationToken>()).Returns(true);
        var service = serviceBuilder.Build();

        var previousCount = service.GetCardCountFromDeck(CardId, serviceBuilder.DecksteriaDeck.Name);
        await service.AddCardAsync(CardToAdd);

        var newCount = service.GetCardCountFromDeck(CardId, serviceBuilder.DecksteriaDeck.Name);
        Assert.Equal(previousCount, newCount);
    }

    [Fact]
    public async Task AddCard_MaximumCount_ShouldNotIncreaseCount()
    {
        var serviceBuilder = new DeckbuildingServiceBuilder();
        serviceBuilder.DecksteriaDeck.IsCardCanBeAddedAsync(Arg.Any<long>(), Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns(true);
        serviceBuilder.DecksteriaFormat.CheckCardCountAsync(Arg.Any<long>(), Arg.Any<DecksteriaDecklistDto>(), Arg.Any<CancellationToken>()).Returns(false);
        var service = serviceBuilder.Build();

        var previousCount = service.GetCardCountFromDeck(CardId, serviceBuilder.DecksteriaDeck.Name);
        await service.AddCardAsync(CardToAdd);

        var newCount = service.GetCardCountFromDeck(CardId, serviceBuilder.DecksteriaDeck.Name);
        Assert.Equal(previousCount, newCount);
    }

    [Fact]
    public async Task AddCard_MaxCountAndCantAddToDeck_ShouldNotIncreaseCount()
    {
        var serviceBuilder = new DeckbuildingServiceBuilder();
        serviceBuilder.DecksteriaDeck.IsCardCanBeAddedAsync(Arg.Any<long>(), Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns(false);
        serviceBuilder.DecksteriaFormat.CheckCardCountAsync(Arg.Any<long>(), Arg.Any<DecksteriaDecklistDto>(), Arg.Any<CancellationToken>()).Returns(false);
        var service = serviceBuilder.Build();

        var previousCount = service.GetCardCountFromDeck(CardId, serviceBuilder.DecksteriaDeck.Name);
        await service.AddCardAsync(CardToAdd);

        var newCount = service.GetCardCountFromDeck(CardId, serviceBuilder.DecksteriaDeck.Name);
        Assert.Equal(previousCount, newCount);
    }
}