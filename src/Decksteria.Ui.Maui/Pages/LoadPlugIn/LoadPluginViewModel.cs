namespace Decksteria.Ui.Maui.Pages.LoadPlugIn;

using Decksteria.Ui.Maui.Shared;
using Decksteria.Ui.Maui.Shared.Models;
using System.Collections.ObjectModel;

public sealed class LoadPluginViewModel : BaseViewModel
{
    private enum SelectionScreen
    {
        PlugIn,
        Format,
        Decks
    }

    private SelectionScreen expandedSection = SelectionScreen.PlugIn;

    private ObservableCollection<PlugInTile> _gameTileSource = [];

    public ObservableCollection<PlugInTile> GameTiles
    {
        get => _gameTileSource;
        set
        {
            _gameTileSource = value;
            OnPropertyChanged();
        }
    }
    public bool DecksExpanded
    {
        get => expandedSection == SelectionScreen.Decks;
        set
        {
            if (!value || expandedSection is SelectionScreen.Decks)
            {
                return;
            }

            expandedSection = SelectionScreen.Decks;
            OnPropertyChanged( nameof( DecksExpanded ) );
            OnPropertyChanged( nameof( FormatsExpanded ) );
            OnPropertyChanged( nameof( PlugInsExpanded ) );
        }
    }

    public bool FormatsExpanded
    {
        get => expandedSection == SelectionScreen.Format;
        set
        {
            if (!value || expandedSection is SelectionScreen.Format)
            {
                return;
            }

            expandedSection = SelectionScreen.Format;
            OnPropertyChanged( nameof( DecksExpanded ) );
            OnPropertyChanged( nameof( FormatsExpanded ) );
            OnPropertyChanged( nameof( PlugInsExpanded ) );
        }
    }

    public bool PlugInsExpanded
    {
        get => expandedSection == SelectionScreen.PlugIn;
        set
        {
            if (!value || expandedSection is SelectionScreen.PlugIn)
            {
                return;
            }

            expandedSection = SelectionScreen.PlugIn;
            OnPropertyChanged( nameof( DecksExpanded ) );
            OnPropertyChanged( nameof( FormatsExpanded ) );
            OnPropertyChanged( nameof( PlugInsExpanded ) );
        }
    }
}
