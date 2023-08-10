namespace Decksteria.Service.DecksteriaPluginService;

using Decksteria.Core;

public interface IPlugInManagerService
{
    bool PlugInsLoaded { get; }

    void ChangeFormat(IDecksteriaFormat format);

    IDecksteriaFormat? ChangeFormat(string formatName);

    void ChangePlugIn(IDecksteriaGame plugIn);

    IDecksteriaGame? ChangePlugIn(string plugInName);
}