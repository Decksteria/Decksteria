namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Decksteria.Services.Deckbuilding;
using Decksteria.Services.Deckbuilding.Models;
using Decksteria.Services.FileService.Models;
using Decksteria.Ui.Maui.Shared.Extensions;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
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
        Title = $"{deckbuilder.GameTitle} Deckbuilder - {deckbuilder.FormatTitle}";

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

    private async void Entry_Completed(object sender, EventArgs e)
    {
        if (viewModel.SearchText.Length < 3)
        {
            return;
        }

        viewModel.FilteredCards.ReplaceData(await deckbuilder.GetCardsAsync(viewModel.SearchText));
    }

    private void AdaptiveGrid_Main_SizeChanged(object sender, EventArgs e)
    {
        viewModel.TabViewTabPlacement = AdaptiveGrid_Main.HorizontalDisplay ? TabViewTabPlacement.Top : TabViewTabPlacement.Bottom;
    }
}