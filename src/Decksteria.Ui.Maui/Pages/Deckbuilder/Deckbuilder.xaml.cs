namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Models;
using Decksteria.Services.Deckbuilding;
using Decksteria.Services.Deckbuilding.Models;
using Decksteria.Services.FileService.Models;
using Decksteria.Ui.Maui.Pages.CardInfo;
using Decksteria.Ui.Maui.Pages.Search;
using Decksteria.Ui.Maui.Services.PageService;
using Decksteria.Ui.Maui.Shared.Extensions;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using UraniumUI.Extensions;
using UraniumUI.Material.Controls;
using UraniumUI.Pages;

public partial class Deckbuilder : UraniumContentPage
{
    private readonly DeckbuilderViewModel viewModel = new();

    private readonly IDeckbuildingService deckbuilder;

    private readonly IPageService pageService;

    private ReadOnlyDictionary<string, CollectionView>? deckViews;

    private IEnumerable<ISearchFieldFilter> searchFieldFilters;

    private bool firstLoaded = false;

    public Deckbuilder(IDeckbuildingService deckbuilder, IPageService pageService)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.deckbuilder = deckbuilder;
        this.pageService = pageService;
        this.searchFieldFilters = Array.Empty<ISearchFieldFilter>();
    }

    private async void ContentPage_LoadedAsync(object sender, EventArgs e)
    {
        if (firstLoaded)
        {
            return;
        }

        var decks = await deckbuilder.ReInitializeAsync();
        var deckInfo = deckbuilder.DeckInformation;
        DecksLayout.Items.Clear();
        viewModel.Decks = decks.ToDictionary(kv => kv.Key, kv => new ObservableCollection<CardArt>(kv.Value));
        deckViews = deckInfo.ToDictionary(v => v.Name, RenderCollectionView).AsReadOnly();
        Title = $"{deckbuilder.GameTitle} Deckbuilder - {deckbuilder.FormatTitle}";
        firstLoaded = true;

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

    private void AdaptiveGrid_Main_SizeChanged(object sender, EventArgs e)
    {
        viewModel.TabViewTabPlacement = AdaptiveGrid_Main.HorizontalDisplay ? TabViewTabPlacement.Top : TabViewTabPlacement.Bottom;
    }

    private void DecksLayout_SelectedTabChanged(object _, TabItem e)
    {
        if (e.BindingContext is not DecksteriaDeck deck)
        {
            return;
        }

        viewModel.ActiveDeckTab = deck.Name;
    }

    private async void AdvancedFilter_Pressed(object sender, EventArgs e)
    {
        await pageService.OpenFormPage<SearchModal>(OnSubmitAsync, OnCancelAsync, null);

        async Task OnSubmitAsync(SearchModal searchModal, CancellationToken cancellationToken)
        {
            searchFieldFilters = searchModal.ViewModel.SearchFieldFilters.SelectMany(f => f.AsSearchFieldFilterArray());
            cancellationToken.ThrowIfCancellationRequested();
            await PerformSearch(cancellationToken);
        }

        Task OnCancelAsync(SearchModal searchModal, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    private async Task PerformSearch(CancellationToken cancellationToken = default)
    {
        viewModel.Searching = true;
        var results = await deckbuilder.GetCardsAsync(viewModel.SearchText, searchFieldFilters, cancellationToken);
        viewModel.FilteredCards.ReplaceData(results);
        viewModel.Searching = false;
    }

    private async void TapGestureRecognizer_PrimaryTapped(object sender, TappedEventArgs e)
    {
        if (sender is not Image image || image.BindingContext is not CardArt card)
        {
            return;
        }

        var cardInfo = new CardInfo(card, deckbuilder, pageService);
        await pageService.OpenModalPage(CardInfoClosed, cardInfo);

        Task CardInfoClosed(CardInfo cardInfo, CancellationToken cancellationToken)
        {
            if (!cardInfo.DecksChanged)
            {
                return Task.CompletedTask;
            }

            return UpdateDeckCollections(null, cancellationToken);
        }
    }

    private async void TapGestureRecognizer_SecondaryTapped(object sender, TappedEventArgs e)
    {
        if (sender is not Image image || image.BindingContext is not CardArt card)
        {
            return;
        }

        // If it was right-clicked, attempt to add the card to the currently open deck automatically.
        var modified = await deckbuilder.AddCardAsync(card, viewModel.ActiveDeckTab);
        if (!modified)
        {
            return;
        }

        await UpdateDeckCollections(viewModel.ActiveDeckTab);
    }

    private async void TextSearch_Entered(object sender, EventArgs e)
    {
        if (viewModel.SearchText.Length < 3)
        {
            return;
        }

        await PerformSearch();
    }

    private Task UpdateDeckCollections(string? deckName = null, CancellationToken cancellationToken = default)
    {
        if (deckName is null)
        {
            var collectionTasks = viewModel.Decks.Select(deck => UpdateDeckCollections(deck.Key));
            return Task.WhenAll(collectionTasks);
        }

        cancellationToken.ThrowIfCancellationRequested();
        var newData = deckbuilder.GetDeckCards(deckName);
        if (newData is null || !viewModel.Decks.TryGetValue(deckName, out var collection))
        {
            return Task.CompletedTask;
        }

        collection.ReplaceData(newData);
        return Task.CompletedTask;
    }
}