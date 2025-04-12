namespace Decksteria.Services.UnitTests.Deckbuilding;

using Decksteria.Services.Deckbuilding;
using Decksteria.Services.UnitTests.Deckbuilding.DefaultImplementation;

public sealed class AddCardShould
{
    private IDeckbuildingService service;

    public AddCardShould()
    {
        service = new DeckbuildingService<DefaultDecksteriaFormat>();
    }

    [Fact]
    public void AddCardShould_GetNewCard()
    {
        Assert.True(true);
    }

}