namespace Decksteria.Ui.Maui.Services.PlugInFactory;

using System.Collections.Generic;
using Decksteria.Core;
using Decksteria.Ui.Maui.Shared.Models;

public interface IDecksteriaPlugInFactory
{
    IDecksteriaGame CreatePlugInInstance();

    IEnumerable<DecksteriaPlugIn> GetOrInitializePlugIns();

    void SelectGame(string gameName);
    bool TryAddGame(string dllFilePath);
}