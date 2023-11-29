namespace Decksteria.Services.PlugInFactory;

using System.Collections.Generic;
using Decksteria.Services.PlugInFactory.Models;

public interface IDecksteriaPlugInFactory
{
    GameFormat CreatePlugInInstance();

    IEnumerable<DecksteriaPlugIn> GetOrInitializePlugIns();

    void SelectGame(string gameName, string formatNaame);

    bool TryAddGame(string dllFilePath);
}