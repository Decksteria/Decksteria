namespace Decksteria.Ui.Maui.Services.FileLocator;

using System.Collections.Generic;
using System.IO;
using Decksteria.Services.PlugInFactory;
using Microsoft.Maui.Storage;

internal sealed class DecksteriaFileLocator : IDecksteriaFileLocator
{
    private const string cardImageFolder = "cards";

    private const string otherImageFolder = "images";

    private readonly IDecksteriaPlugInFactory decksteriaPlugInFactory;

    public DecksteriaFileLocator(IDecksteriaPlugInFactory decksteriaPlugInFactory)
    {
        this.decksteriaPlugInFactory = decksteriaPlugInFactory;
    }

    public IEnumerable<string> GetAllCardImageDirectories()
    {
        return Directory.GetDirectories(FileSystem.AppDataDirectory, cardImageFolder, SearchOption.AllDirectories);
    }

    public string GetExpectedFileLocation(string fileName)
    {
        var gameName = GetGameName();
        return Path.Combine(FileSystem.AppDataDirectory, gameName, fileName);
    }

    public string GetExpectedCardImageLocation(string fileName)
    {
        var gameName = GetGameName();
        return Path.Combine(FileSystem.AppDataDirectory, gameName, cardImageFolder, fileName);
    }

    public string GetExpectedImageLocation(string fileName)
    {
        var gameName = GetGameName();
        return Path.Combine(FileSystem.AppDataDirectory, gameName, otherImageFolder, fileName);
    }

    private string GetGameName()
    {
        var gameFormat = decksteriaPlugInFactory.GetSelectedFormat();
        return gameFormat.GameName;
    }
}
