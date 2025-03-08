namespace Decksteria.Ui.Maui.Services.FileLocator;

internal interface IDecksteriaFileLocator
{
    public string GetExpectedCardImageLocation(string fileName);
    public string GetExpectedFileLocation(string fileName);
    public string GetExpectedImageLocation(string fileName);
}