namespace Decksteria.Service.DecksteriaPluginService;

using System.Collections.Generic;
using Decksteria.Core;

public interface IPlugInManagerService
{
    bool PlugInsLoaded { get; }

    IEnumerable<IDecksteriaGame> AvailablePlugIns { get; }

    IEnumerable<IDecksteriaFormat> AvailableFormats { get; }

    void AddNewPlugIn(IDecksteriaGame plugIn);
    
    void ChangeFormat(IDecksteriaFormat format);

    IDecksteriaFormat? ChangeFormat(string formatName);

    void ChangePlugIn(IDecksteriaGame plugIn);

    IDecksteriaGame? ChangePlugIn(string plugInName);

    void SetAvailablePlugIns(IEnumerable<IDecksteriaGame> plugIns);
}