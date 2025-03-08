namespace Decksteria.Services.PlugInFactory;

using System.Collections.Generic;
using Decksteria.Services.PlugInFactory.Models;

public interface IDecksteriaPlugInFactory
{
    public IEnumerable<DecksteriaPlugIn> GetOrInitializePlugIns();

    public GameFormat GetSelectedFormat();

    public void SelectGame(string gameName, string formatName);

    public bool TryAddGame(string dllFilePath);
}