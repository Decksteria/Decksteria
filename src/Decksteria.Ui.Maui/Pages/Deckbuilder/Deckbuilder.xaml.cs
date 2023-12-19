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

public partial class Deckbuilder : ContentPage
{
    private readonly DeckbuilderViewModel viewModel = new();

    private readonly ReadOnlyDictionary<IDecksteriaDeck, CollectionView> deckViews;

    private readonly IDeckbuildingService deckbuilder;

    private readonly GameFormat gameFormat;

    public Deckbuilder(IDeckbuildingService deckbuilder, GameFormat gameFormat)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.deckbuilder = deckbuilder;
        this.gameFormat = gameFormat;
        deckViews = gameFormat.Format.Decks.ToDictionary(deck => deck, RenderCollectionView).AsReadOnly();

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
                ItemsSource = bindedCollection,
                MinimumHeightRequest = 100.0
            };

            DecksLayout.Add(collectionView);
            return collectionView;
        }
    }

    private async void ContentPage_LoadedAsync(object sender, EventArgs e)
    {
        var formatCards = await deckbuilder.GetCardsAsync();
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