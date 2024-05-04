namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Decksteria.Services.FileService.Models;

internal class DeckbuilderViewModel()
{
    public Dictionary<string, ObservableCollection<CardArt>> Decks { get; set; } = [];

    public ObservableCollection<CardArt> FilteredCards { get; set; } = [];

    public string SearchText { get; set; } = "";
}
