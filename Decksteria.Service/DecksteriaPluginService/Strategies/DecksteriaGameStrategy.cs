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

    public string Name => SelectedGame.Name;

    public string DisplayName => SelectedGame.DisplayName;

    public byte[]? Icon => selectedGame?.Icon;

    public IEnumerable<IDecksteriaFormat> Formats => SelectedGame.Formats;

    public IEnumerable<IDecksteriaImport> Importers => SelectedGame.Importers;

    public IEnumerable<IDecksteriaExport> Exporters => SelectedGame.Exporters;

    private IDecksteriaGame SelectedGame => selectedGame ?? throw new NotImplementedException("Game Plug-In has not been selected.");
}
