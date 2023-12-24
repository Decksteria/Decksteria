namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using Decksteria.Core;
using Decksteria.Services.Deckbuilding;
using Decksteria.Services.FileService.Models;
using Decksteria.Services.PlugInFactory.Models;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UraniumUI.Material.Controls;
using UraniumUI.Pages;

public partial class Deckbuilder : UraniumContentPage
{
    private readonly DeckbuilderViewModel viewModel = new();

    private ReadOnlyDictionary<IDecksteriaDeck, CollectionView>? deckViews;

    private readonly IDeckbuildingService deckbuilder;

    private readonly GameFormat gameFormat;

    public Deckbuilder(IDeckbuildingService deckbuilder, GameFormat gameFormat)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.deckbuilder = deckbuilder;
        this.gameFormat = gameFormat;
    }

    private async void ContentPage_LoadedAsync(object sender, EventArgs e)
    {
        deckViews = gameFormat.Format.Decks.ToDictionary(deck => deck, RenderCollectionView).AsReadOnly();
        viewModel.FilteredCards = new(await deckbuilder.GetCardsAsync());

        CollectionView RenderCollectionView(IDecksteriaDeck decksteriaDeck)
        {
            var bindedCollection = new ObservableCollection<CardArt>();
            viewModel.Decks.Add(decksteriaDeck, bindedCollection);

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
                Title = decksteriaDeck.DisplayName,
                BindingContext = decksteriaDeck,
                Content = frameView
            };

            DecksLayout.Items.Add(tabItem);
            return collectionView;
        }
    }

    private void Button_ExpandSearch_Pressed(object sender, EventArgs e)
    {
        viewModel.ExpandSearch = !viewModel.ExpandSearch;
    }
}