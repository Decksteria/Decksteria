namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using Decksteria.Core;
using Decksteria.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

internal class DeckbuilderViewModel()
{
    public bool Expand { get; set; } = false;

    public ObservableCollection<CardArt> FilteredCards { get; set; } = [];

    public Dictionary<IDecksteriaDeck, ObservableCollection<CardArt>> Decks { get; set; } = [];
}
