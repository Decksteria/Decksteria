namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using Decksteria.Services.Deckbuilding;
using Decksteria.Services.FileService.Models;
using Microsoft.Maui.Controls;
using System;

public partial class Deckbuilder : ContentPage
{
    private readonly DeckbuilderViewModel viewModel = new();

    // private readonly IDeckbuildingService deckbuilder;

    public Deckbuilder()
    {
        InitializeComponent();
        BindingContext = viewModel;
        // this.deckbuilder = ;
    }

    private async void ContentPage_LoadedAsync(object sender, EventArgs e)
    {
        // var formatCards = await deckbuilder.GetCardsAsync();
        var formatCards = new CardArt[] { };
        foreach (var card in formatCards)
        {
            viewModel.FilteredCards.Add(card);
        }
    }
}