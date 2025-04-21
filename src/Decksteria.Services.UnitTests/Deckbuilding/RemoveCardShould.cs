namespace Decksteria.Services.UnitTests.Deckbuilding;

using Decksteria.Core.Data;
using Decksteria.Services.DeckFileService.Models;
using NSubstitute;

using DecksteriaDecklistDto = IReadOnlyDictionary<string, IEnumerable<long>>;

public sealed class RemoveCardShould
{
    private const long CardId = 1;

    private static readonly DecksteriaImage image = new(string.Empty, string.Empty);

    private static readonly CardArt CardToRemove = new(CardId, 0, image, string.Empty);

    [Fact]
    public async Task RemoveCard_ShouldRemoveAddedCard()
    {
        var serviceBuilder = new DeckbuildingServiceBuilder();
        serviceBuilder.DecksteriaDeck.IsCardCanBeAddedAsync(Arg.Any<long>(), Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns(true);
        serviceBuilder.DecksteriaFormat.CheckCardCountAsync(Arg.Any<long>(), Arg.Any<DecksteriaDecklistDto>(), Arg.Any<CancellationToken>()).Returns(true);
        var service = serviceBuilder.Build();

        await service.AddCardAsync(CardToRemove);

        var previousCount = service.GetCardCountFromDeck(CardId, serviceBuilder.DecksteriaDeck.Name);
        await service.RemoveCardAsync(CardToRemove, serviceBuilder.DecksteriaDeck.Name);

        var newCount = service.GetCardCountFromDeck(CardId, serviceBuilder.DecksteriaDeck.Name);
        Assert.Equal(previousCount - 1, newCount);
    }

    [Fact]
    public async Task RemoveCard_ShouldNotRemoveDifferentCard()
    {
        var serviceBuilder = new DeckbuildingServiceBuilder();
        serviceBuilder.DecksteriaDeck.IsCardCanBeAddedAsync(Arg.Any<long>(), Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns(true);
        serviceBuilder.DecksteriaFormat.CheckCardCountAsync(Arg.Any<long>(), Arg.Any<DecksteriaDecklistDto>(), Arg.Any<CancellationToken>()).Returns(true);
        var service = serviceBuilder.Build();

        await service.AddCardAsync(new CardArt(CardId + 1, 0, image, string.Empty));

        var previousCount = service.GetCardCountFromDeck(CardId, serviceBuilder.DecksteriaDeck.Name);
        await service.RemoveCardAsync(CardToRemove, serviceBuilder.DecksteriaDeck.Name);

        var newCount = service.GetCardCountFromDeck(CardId, serviceBuilder.DecksteriaDeck.Name);
        Assert.Equal(previousCount, newCount);
    }

    [Fact]
    public async Task RemoveCard_ShouldNotRemoveNonExistentCard()
    {
        var serviceBuilder = new DeckbuildingServiceBuilder();
        var service = serviceBuilder.Build();

        var previousCount = service.GetCardCountFromDeck(CardId, serviceBuilder.DecksteriaDeck.Name);

        await service.RemoveCardAsync(CardToRemove, serviceBuilder.DecksteriaDeck.Name);

        var newCount = service.GetCardCountFromDeck(CardId, serviceBuilder.DecksteriaDeck.Name);
        Assert.Equal(previousCount, newCount);
    }
}
