namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Decksteria.Services.DeckFileService.Models;
using Decksteria.Ui.Maui.Shared.Configuration;
using Microsoft.Extensions.Options;
using UraniumUI.Icons.FontAwesome;
using UraniumUI.Material.Controls;

public class DeckbuilderViewModel : INotifyPropertyChanged
{
    public enum SortDeckStatus
    {
        Unsorted,
        Ascending,
        Descending
    }

    public DeckbuilderViewModel(IOptions<PreferenceConfiguration> preferences)
    {
        this.preferences = preferences.Value;
    }

    private bool _advancedFiltersApplied = false;

    private bool _loading;

    private bool _searching = false;

    private SortDeckStatus _sortStatus = SortDeckStatus.Unsorted;

    private TabViewTabPlacement _tabPlacement = TabViewTabPlacement.Top;

    private bool _validDeckStatus = false;

    private string _windowTitle = string.Empty;

    private readonly PreferenceConfiguration preferences;

    public string ActiveDeckTab { get; set; } = string.Empty;

    public bool AllowDownloading => preferences.DownloadImages;

    public string DecklistName { get; set; } = string.Empty;

    public bool AdvancedFiltersApplied
    {
        get => _advancedFiltersApplied;
        set
        {
            _advancedFiltersApplied = value;
            OnPropertyChanged(nameof(AdvancedFiltersApplied));
        }
    }

    public Dictionary<string, ObservableCollection<CardArt>> Decks { get; set; } = [];

    public ObservableCollection<CardArt> FilteredCards { get; set; } = [];

    public bool Loading
    {
        get => _loading;
        set
        {
            _loading = value;
            OnPropertyChanged(nameof(Loading));
        }
    }

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

    public bool ValidDeckStatus
    {
        get => _validDeckStatus;
        set
        {
            _validDeckStatus = value;
            OnPropertyChanged(nameof(ValidDeckStatus));
            OnPropertyChanged(nameof(ValidDeckIcon));
        }
    }

    public string SortDeckIcon => SortStatus switch
    {
        SortDeckStatus.Unsorted => Solid.Sort,
        SortDeckStatus.Ascending => Solid.SortDown,
        SortDeckStatus.Descending => Solid.SortUp,
        _ => throw new UnreachableException($"An option for {nameof(SortStatus)} is unavailable.")
    };

    public string ValidDeckIcon => _validDeckStatus ? Solid.CircleCheck : Solid.Xmark;

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
