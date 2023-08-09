namespace Decksteria.Service.DecksteriaPluginService;

using Decksteria.Core;
using Decksteria.Service.DecksteriaPluginService.Strategies;

internal sealed class PlugInManagerService : IPlugInManagerService
{
    private readonly IDecksteriaGameStrategy gameStrategy;

    private readonly IDecksteriaFormatStrategy formatStrategy;

    private readonly Dictionary<string, IDecksteriaGame> availablePlugIns;

    public PlugInManagerService(IDecksteriaGameStrategy gameStrategy, IDecksteriaFormatStrategy formatStrategy, IEnumerable<IDecksteriaGame> availablePlugIns)
    {
        this.gameStrategy = gameStrategy;
        this.formatStrategy = formatStrategy;
        this.availablePlugIns = availablePlugIns.ToDictionary(game => game.Name);
    }

    public void ChangePlugIn(IDecksteriaGame plugIn) => gameStrategy.ChangePlugIn(plugIn);

    public IDecksteriaGame? ChangePlugIn(string plugInName)
    {
        var newPlugIn = availablePlugIns.GetValueOrDefault(plugInName);
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
}
