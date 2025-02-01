namespace Decksteria.Ui.Maui.Services.FileLocator;

using System.IO;
using Decksteria.Services.PlugInFactory;
using Microsoft.Maui.Storage;

internal sealed class DecksteriaFileLocator : IDecksteriaFileLocator
{
    private readonly IDecksteriaPlugInFactory decksteriaPlugInFactory;

    public DecksteriaFileLocator(IDecksteriaPlugInFactory decksteriaPlugInFactory)
    {
        this.decksteriaPlugInFactory = decksteriaPlugInFactory;
    }

    public string GetExpectedFileLocation(string fileName)
    {
        var gameName = GetGameName();
        return Path.Combine(FileSystem.AppDataDirectory, gameName, fileName);
    }

    public string GetExpectedCardImageLocation(string fileName)
    {
        var gameName = GetGameName();
        return Path.Combine(FileSystem.AppDataDirectory, gameName, "cards", fileName);
    }

    public string GetExpectedImageLocation(string fileName)
    {
        var gameName = GetGameName();
        return Path.Combine(FileSystem.AppDataDirectory, gameName, "images", fileName);
    }

    private string GetGameName()
    {
        var gameFormat = decksteriaPlugInFactory.GetSelectedFormat();
        return gameFormat.GameName;
    }
}
