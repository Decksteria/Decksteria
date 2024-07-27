namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Decksteria.Services.FileService.Models;
using UraniumUI.Material.Controls;

internal class DeckbuilderViewModel : INotifyPropertyChanged
{
    public string ActiveDeckTab { get; set; } = string.Empty;

    private TabViewTabPlacement _tabPlacement = TabViewTabPlacement.Top;

    private bool _searching = false;

    private string _windowTitle = string.Empty;

    public Dictionary<string, ObservableCollection<CardArt>> Decks { get; set; } = [];

    public ObservableCollection<CardArt> FilteredCards { get; set; } = [];

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool Searching
    {
        get => _searching;
        set
        {
            _searching = value;
            OnPropertyChanged(nameof(Searching));
        }
    }

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

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
