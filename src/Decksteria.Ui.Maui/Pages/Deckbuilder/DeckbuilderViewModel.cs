namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Decksteria.Services.FileService.Models;
using UraniumUI.Material.Controls;

internal class DeckbuilderViewModel : INotifyPropertyChanged
{
    private TabViewTabPlacement _tabPlacement = TabViewTabPlacement.Top;

    public Dictionary<string, ObservableCollection<CardArt>> Decks { get; set; } = [];

    public ObservableCollection<CardArt> FilteredCards { get; set; } = [];

    public event PropertyChangedEventHandler? PropertyChanged;

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

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
