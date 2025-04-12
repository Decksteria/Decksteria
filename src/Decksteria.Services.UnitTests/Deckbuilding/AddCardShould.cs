namespace Decksteria.Services.UnitTests.Deckbuilding;

using Decksteria.Services.Deckbuilding;
using Decksteria.Services.UnitTests.Deckbuilding.Mocks;

internal sealed class AddCardShould
{
    private IDeckbuildingService service;


    public AddCardShould()
    {
        service = new DeckbuildingService<FormatImplementation>();
    }

    [Fact]
    public sealed void AddCardShould_GetNewCard()
    {

    }

}