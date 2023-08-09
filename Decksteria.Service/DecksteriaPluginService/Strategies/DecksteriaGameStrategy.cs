namespace Decksteria.Service.DecksteriaPluginService.Strategies;

using Decksteria.Core;
using System.Collections.Generic;

internal sealed class DecksteriaGameStrategy : IDecksteriaGameStrategy
{
    private IDecksteriaGame? selectedGame;

    public void ChangePlugIn(IDecksteriaGame? newPlugIn)
    {
        selectedGame = newPlugIn;
    }

    public string Name => CheckAndThrowIfNotSelected(selectedGame?.Name);

    public string DisplayName => CheckAndThrowIfNotSelected(selectedGame?.DisplayName);

    public byte[]? Icon => selectedGame?.Icon;

    public IEnumerable<IDecksteriaFormat> Formats => CheckAndThrowIfNotSelected(selectedGame?.Formats);

    public IEnumerable<IDecksteriaImport> Importers => CheckAndThrowIfNotSelected(selectedGame?.Importers);

    public IEnumerable<IDecksteriaExport> Exporters => CheckAndThrowIfNotSelected(selectedGame?.Exporters);

    private T CheckAndThrowIfNotSelected<T>(T? value)
    {
        return value ?? throw new NotImplementedException("Game Plug-In has not been selected.");
    }
}
