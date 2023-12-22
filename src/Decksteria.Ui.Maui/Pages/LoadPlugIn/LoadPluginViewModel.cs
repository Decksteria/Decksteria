namespace Decksteria.Ui.Maui.Pages.LoadPlugIn;

using System.Collections.ObjectModel;
using Decksteria.Ui.Maui.Shared;
using Decksteria.Ui.Maui.Shared.Models;

public sealed class LoadPluginViewModel : BaseViewModel
{
    private enum SelectionScreen
    {
        PlugIn,
        Format,
        Decks
    }

    private ObservableCollection<PlugInDetails> _gameTileSource = [];

    public ObservableCollection<PlugInDetails> GameTiles
    {
        get => _gameTileSource;
        set
        {
            _gameTileSource = value;
            OnPropertyChanged();
        }
    }
}
