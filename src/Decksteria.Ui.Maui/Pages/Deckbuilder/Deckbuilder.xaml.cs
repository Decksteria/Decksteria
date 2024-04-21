namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using Decksteria.Services.Deckbuilding;
using Decksteria.Services.Deckbuilding.Models;
using Decksteria.Services.FileService.Models;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UraniumUI.Material.Controls;
using UraniumUI.Pages;

public partial class Deckbuilder : UraniumContentPage
{
    private readonly DeckbuilderViewModel viewModel = new();

    private ReadOnlyDictionary<string, CollectionView>? deckViews;

    private readonly IDeckbuildingService deckbuilder;

    public Deckbuilder(IDeckbuildingService deckbuilder)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.deckbuilder = deckbuilder;
    }

    private async void ContentPage_LoadedAsync(object sender, EventArgs e)
    {
        var decks = await deckbuilder.ReInitializeAsync();
        var deckInfo = deckbuilder.GetDeckInformation();
        viewModel.Decks = decks.ToDictionary(kv => kv.Key, kv => new ObservableCollection<CardArt>(kv.Value));
        deckViews = deckInfo.ToDictionary(v => v.Name, RenderCollectionView).AsReadOnly();
        viewModel.FilteredCards = new(await deckbuilder.GetCardsAsync());

        CollectionView RenderCollectionView(DecksteriaDeck decksteriaDeck)
        {
            var bindedCollection = new ObservableCollection<CardArt>();

            // Collection View
            var collectionView = new CollectionView
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                VerticalScrollBarVisibility = ScrollBarVisibility.Default,
                VerticalOptions = LayoutOptions.Start,
                ItemTemplate = CollectionView_CardItem,
                ItemsSource = bindedCollection,
                MinimumHeightRequest = 100.0,
            };
            var frameView = new Frame
            {
                Padding = 1,
                CornerRadius = 15,
                Margin = 2,
                Content = collectionView
            };

            // Create Tab Item
            var tabItem = new TabItem
            {
                Title = decksteriaDeck.Label,
                BindingContext = decksteriaDeck,
                Content = frameView
            };

            DecksLayout.Items.Add(tabItem);
            return collectionView;
        }
    }

    private void ContentPage_UnloadedAsync(object sender, EventArgs e)
    {
        DecksLayout.Items.Clear();
    }

    private void Button_ExpandSearch_Pressed(object sender, EventArgs e)
    {
        viewModel.ExpandSearch = !viewModel.ExpandSearch;
    }
}