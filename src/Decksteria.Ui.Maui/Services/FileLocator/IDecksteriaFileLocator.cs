namespace Decksteria.Ui.Maui.Services.FileLocator;

internal interface IDecksteriaFileLocator
{
    string GetExpectedCardImageLocation(string fileName);
    string GetExpectedFileLocation(string fileName);
    string GetExpectedImageLocation(string fileName);
}