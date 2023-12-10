namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using Decksteria.Core;
using Decksteria.Services.Deckbuilding;
using Decksteria.Services.FileService.Models;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;

public partial class Deckbuilder : ContentPage
{
    private readonly DeckbuilderViewModel viewModel = new();

    // private readonly IDeckbuildingService deckbuilder;

    public Deckbuilder(IDecksteriaFormat format)
    {
        InitializeComponent();
        BindingContext = viewModel;
        // this.deckbuilder = ;

        CollectionView RenderCollectionView(IDecksteriaDeck decksteriaDeck)
        {
            var bindedCollection = new ObservableCollection<CardArt>();
            viewModel.Decks.Add(decksteriaDeck, bindedCollection);

            // Header Label
            var headerLabel = new Label
            {
                Text = $"{decksteriaDeck.DisplayName}:",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            headerLabel.SetDynamicResource(Label.TextColorProperty, "SecondaryTextColor");

            // Collection View
            var collectionView = new CollectionView
            {
                Header = headerLabel,
                ItemTemplate = CollectionView_CardItem,
                ItemsSource = bindedCollection
            };
            return collectionView;
        }
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

    private void Button_ExpandSearch_Pressed(object sender, EventArgs e)
    {
        viewModel.ExpandSearch = !viewModel.ExpandSearch;
    }
}