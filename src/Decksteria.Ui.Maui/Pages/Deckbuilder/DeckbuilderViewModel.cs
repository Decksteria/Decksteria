namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Decksteria.Services.FileService.Models;
using UraniumUI.Icons.FontAwesome;
using UraniumUI.Material.Controls;

internal class DeckbuilderViewModel : INotifyPropertyChanged
{
    internal enum SortDeckStatus
    {
        Unsorted,
        Ascending,
        Descending
    }

    public string ActiveDeckTab { get; set; } = string.Empty;

    private TabViewTabPlacement _tabPlacement = TabViewTabPlacement.Top;

    private bool _searching = false;

    private SortDeckStatus _sortStatus = SortDeckStatus.Unsorted;

    private string _windowTitle = string.Empty;

    public Dictionary<string, ObservableCollection<CardArt>> Decks { get; set; } = [];

    public ObservableCollection<CardArt> FilteredCards { get; set; } = [];

    public bool Searching
    {
        get => _searching;
        set
        {
            _searching = value;
            OnPropertyChanged(nameof(Searching));
        }
    }

    public SortDeckStatus SortStatus
    {
        get => _sortStatus;
        set
        {
            _sortStatus = value;
            OnPropertyChanged(nameof(SortStatus));
            OnPropertyChanged(nameof(SortDeckIcon));
        }
    }

    public string SortDeckIcon => SortStatus switch
    {
        SortDeckStatus.Unsorted => Solid.Sort,
        SortDeckStatus.Ascending => Solid.SortDown,
        SortDeckStatus.Descending => Solid.SortUp,
        _ => throw new UnreachableException($"An option for {nameof(SortStatus)} is unavailable.")
    };

    public string SearchText { get; set; } = "";

    public TabViewTabPlacement TabViewTabPlacement
    {
        get => _tabPlacement;
        set
        {
            if (_tabPlacement != value)
            {
                _tabPlacement = value;
                OnPropertyChanged(nameof(TabViewTabPlacement));
            }
        }
    }

    public string WindowTitle {
        get => _windowTitle;
        set
        {
            _windowTitle = value;
            OnPropertyChanged(nameof(WindowTitle));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
