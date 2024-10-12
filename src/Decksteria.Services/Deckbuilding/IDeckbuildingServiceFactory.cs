namespace Decksteria.Services.Deckbuilding;

public interface IDeckbuildingServiceFactory
{
    void ChangeFormat(string formatName);

    IDeckbuildingService GetCurrentDeckbuildingService();
}