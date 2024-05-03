namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using Decksteria.Services.FileService.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

internal class DeckbuilderViewModel()
{
    public Dictionary<string, ObservableCollection<CardArt>> Decks { get; set; } = [];

    public ObservableCollection<CardArt> FilteredCards { get; set; } = [];

    public string SearchText { get; set; } = "";
}
