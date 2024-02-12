namespace Decksteria.Services.PlugInFactory;

using System.Collections.Generic;
using Decksteria.Services.PlugInFactory.Models;

public interface IDecksteriaPlugInFactory
{
    IEnumerable<DecksteriaPlugIn> GetOrInitializePlugIns();

    GameFormat GetSelectedFormat();

    void SelectGame(string gameName, string formatNaame);

    bool TryAddGame(string dllFilePath);
}