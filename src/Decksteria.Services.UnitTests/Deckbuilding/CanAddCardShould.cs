namespace Decksteria.Services.UnitTests.Deckbuilding;

using NSubstitute;

using DecksteriaDecklistDto = IReadOnlyDictionary<string, IEnumerable<long>>;

public sealed class CanAddCardShould
{
    private const long CardId = 1;

    [Fact]
    public async Task CanAddCard_CanAddAndBelowMaximumCount_ShouldReturnTrue()
    {
        var serviceBuilder = new DeckbuildingServiceBuilder();
        serviceBuilder.DecksteriaDeck.IsCardCanBeAddedAsync(Arg.Any<long>(), Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns(true);
        serviceBuilder.DecksteriaFormat.CheckCardCountAsync(Arg.Any<long>(), Arg.Any<DecksteriaDecklistDto>(), Arg.Any<CancellationToken>()).Returns(true);
        var service = serviceBuilder.Build();

        var result = await service.CanAddCardAsync(CardId);
        Assert.True(result);
    }

    [Fact]
    public async Task AddCard_CantAddToDeck_ShouldNotIncreaseCount()
    {
        var serviceBuilder = new DeckbuildingServiceBuilder();
        serviceBuilder.DecksteriaDeck.IsCardCanBeAddedAsync(Arg.Any<long>(), Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns(false);
        serviceBuilder.DecksteriaFormat.CheckCardCountAsync(Arg.Any<long>(), Arg.Any<DecksteriaDecklistDto>(), Arg.Any<CancellationToken>()).Returns(true);
        var service = serviceBuilder.Build();

        var result = await service.CanAddCardAsync(CardId);
        Assert.False(result);
    }

    [Fact]
    public async Task AddCard_MaximumCount_ShouldNotIncreaseCount()
    {
        var serviceBuilder = new DeckbuildingServiceBuilder();
        serviceBuilder.DecksteriaDeck.IsCardCanBeAddedAsync(Arg.Any<long>(), Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns(true);
        serviceBuilder.DecksteriaFormat.CheckCardCountAsync(Arg.Any<long>(), Arg.Any<DecksteriaDecklistDto>(), Arg.Any<CancellationToken>()).Returns(false);
        var service = serviceBuilder.Build();

        var result = await service.CanAddCardAsync(CardId);
        Assert.False(result);
    }

    [Fact]
    public async Task AddCard_MaxCountAndCantAddToDeck_ShouldNotIncreaseCount()
    {
        var serviceBuilder = new DeckbuildingServiceBuilder();
        serviceBuilder.DecksteriaDeck.IsCardCanBeAddedAsync(Arg.Any<long>(), Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns(false);
        serviceBuilder.DecksteriaFormat.CheckCardCountAsync(Arg.Any<long>(), Arg.Any<DecksteriaDecklistDto>(), Arg.Any<CancellationToken>()).Returns(false);
        var service = serviceBuilder.Build();

        var result = await service.CanAddCardAsync(CardId);
        Assert.False(result);
    }
}
