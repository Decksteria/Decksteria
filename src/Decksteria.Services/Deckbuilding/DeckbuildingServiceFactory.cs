namespace Decksteria.Services.Deckbuilding;

using Decksteria.Core;
using Decksteria.Services.PlugInFactory;
using Decksteria.Services.PlugInFactory.Models;

internal sealed class DeckbuildingServiceFactory : IDeckbuildingServiceFactory
{
    private readonly IDecksteriaPlugInFactory plugInFactory;

    private GameFormat currentGameFormat;

    public DeckbuildingServiceFactory(IDecksteriaPlugInFactory plugInFactory)
    {
        this.plugInFactory = plugInFactory;
        currentGameFormat = plugInFactory.GetSelectedFormat();
    }

    public void ChangeFormat(string formatName)
    {
        plugInFactory.SelectGame(currentGameFormat.GameName, formatName);
        currentGameFormat = plugInFactory.GetSelectedFormat();
    }

    public IDeckbuildingService GetCurrentDeckbuildingService()
    {
        return new DeckbuildingService<IDecksteriaFormat>(currentGameFormat.Game, currentGameFormat.Format);
    }
}
