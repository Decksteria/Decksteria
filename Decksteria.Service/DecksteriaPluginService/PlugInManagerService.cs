namespace Decksteria.Service.DecksteriaPluginService;

using Decksteria.Core;
using Decksteria.Service.DecksteriaPluginService.Strategies;
using System.Collections.ObjectModel;

internal sealed class PlugInManagerService : IPlugInManagerService
{
    private readonly IDecksteriaGameStrategy gameStrategy;

    private readonly IDecksteriaFormatStrategy formatStrategy;

    private Dictionary<string, IDecksteriaGame>? availablePlugIns;

    public PlugInManagerService(IDecksteriaGameStrategy gameStrategy, IDecksteriaFormatStrategy formatStrategy)
    {
        this.gameStrategy = gameStrategy;
        this.formatStrategy = formatStrategy;
    }

    public bool PlugInsLoaded { get; private set; }

    public IEnumerable<IDecksteriaGame> AvailablePlugIns => availablePlugIns?.Select(kv => kv.Value) ?? throw new NotImplementedException("Game Plug-In has not been selected.");

    public IEnumerable<IDecksteriaFormat> AvailableFormats => gameStrategy.Formats;

    public void AddNewPlugIn(IDecksteriaGame plugIn)
    {
        availablePlugIns = availablePlugIns ?? new Dictionary<string, IDecksteriaGame>();
        availablePlugIns.Add(plugIn.Name, plugIn);
        PlugInsLoaded = true;
    }

    public void ChangePlugIn(IDecksteriaGame plugIn) => gameStrategy.ChangePlugIn(plugIn);

    public IDecksteriaGame? ChangePlugIn(string plugInName)
    {
        var newPlugIn = availablePlugIns?.GetValueOrDefault(plugInName);
        gameStrategy.ChangePlugIn(newPlugIn);
        return newPlugIn;
    }

    public void ChangeFormat(IDecksteriaFormat format) => formatStrategy.ChangeFormat(format);

    public IDecksteriaFormat? ChangeFormat(string formatName)
    {
        var newFormat = gameStrategy.Formats.FirstOrDefault(format => format.Name == formatName);
        formatStrategy.ChangeFormat(newFormat);
        return newFormat;
    }

    public void SetAvailablePlugIns(IEnumerable<IDecksteriaGame> plugIns)
    {
        availablePlugIns = plugIns.ToDictionary(plugin => plugin.Name);
        PlugInsLoaded = true;
    }
}
