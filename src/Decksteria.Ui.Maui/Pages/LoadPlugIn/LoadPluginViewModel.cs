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

    private SelectionScreen expandedSection = SelectionScreen.PlugIn;

    private ObservableCollection<PlugInTile> _gameTileSource = [];

    private ObservableCollection<FormatTile> _formatTileSource = [];

    private ObservableCollection<DeckTile> _deckTileSource = [];

    public ObservableCollection<PlugInTile> GameTiles
    {
        get => _gameTileSource;
        set
        {
            _gameTileSource = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<FormatTile> FormatTiles
    {
        get => _formatTileSource;
        set
        {
            _formatTileSource = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<DeckTile> DeckTiles
    {
        get => _deckTileSource;
        set
        {
            _deckTileSource = value;
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
            OnPropertyChanged(nameof(DecksExpanded));
            OnPropertyChanged(nameof(FormatsExpanded));
            OnPropertyChanged(nameof(PlugInsExpanded));
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
            OnPropertyChanged(nameof(DecksExpanded));
            OnPropertyChanged(nameof(FormatsExpanded));
            OnPropertyChanged(nameof(PlugInsExpanded));
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
            OnPropertyChanged(nameof(DecksExpanded));
            OnPropertyChanged(nameof(FormatsExpanded));
            OnPropertyChanged(nameof(PlugInsExpanded));
        }
    }

    public PlugInTile? SelectedPlugIn { get; set; }

    public FormatTile? SelectedFormat { get; set; }
}
