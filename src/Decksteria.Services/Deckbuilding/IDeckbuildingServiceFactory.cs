namespace Decksteria.Services.Deckbuilding;

public interface IDeckbuildingServiceFactory
{
    public void ChangeFormat(string formatName);

    public IDeckbuildingService GetCurrentDeckbuildingService();
}