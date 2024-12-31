namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Storage;
using Decksteria.Core.Models;
using Decksteria.Services.Deckbuilding;
using Decksteria.Services.Deckbuilding.Models;
using Decksteria.Services.FileService.Models;
using Decksteria.Ui.Maui.Pages.CardInfo;
using Decksteria.Ui.Maui.Pages.DeckStatistics;
using Decksteria.Ui.Maui.Pages.Search;
using Decksteria.Ui.Maui.Services.DeckFileService;
using Decksteria.Ui.Maui.Services.PageService;
using Decksteria.Ui.Maui.Shared.Extensions;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using UraniumUI.Material.Controls;
using UraniumUI.Pages;
using Path = System.IO.Path;

public partial class Deckbuilder : UraniumContentPage
{
    private readonly DeckbuilderViewModel viewModel = new();

    private readonly IDeckbuildingServiceFactory deckbuilderFactory;

    private readonly IDeckFileService deckFileService;

    private readonly IPageService pageService;

    private IDeckbuildingService deckbuilder;

    private SearchModal? searchModal = null;

    private ReadOnlyDictionary<string, CollectionView>? deckViews;

    private IEnumerable<ISearchFieldFilter> searchFieldFilters;

    private bool firstLoaded = false;

    public Deckbuilder(IDeckbuildingServiceFactory deckbuilderFactory, IDeckFileServiceFactory deckFileServiceFactory, IPageService pageService)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.deckbuilderFactory = deckbuilderFactory;
        this.deckbuilder = deckbuilderFactory.GetCurrentDeckbuildingService();
        this.deckFileService = deckFileServiceFactory.GetDeckFileService();
        this.pageService = pageService;
        this.searchFieldFilters = Array.Empty<ISearchFieldFilter>();
    }

    public async Task LoadDecklistAsync(string deckName, CancellationToken cancellationToken = default)
    {
        viewModel.Loading = true;
        viewModel.DecklistName = deckName;
        var decklist = await deckFileService.ReadDecklistAsync(deckName, cancellationToken);
        await LoadDecklistAsync(decklist, cancellationToken);
        viewModel.Loading = false;
    }

    private async void ContentPage_LoadedAsync(object? sender, EventArgs e)
    {
        if (firstLoaded)
        {
            return;
        }

        var decks = await deckbuilder.ReInitializeAsync();
        var deckInfo = deckbuilder.DeckInformation;
        DecksLayout.Tabs.Clear();
        viewModel.Decks = decks.ToDictionary(kv => kv.Key, kv => new ObservableCollection<CardArt>(kv.Value));
        deckViews = deckInfo.ToDictionary(v => v.Name, RenderCollectionView).AsReadOnly();
        viewModel.WindowTitle = $"{deckbuilder.GameTitle} Deckbuilder - {deckbuilder.FormatTitle}";
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
                ItemTemplate = DeckView_CardItem,
                ItemsSource = bindedCollection,
                MinimumHeightRequest = 100.0,
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    HorizontalItemSpacing = 2,
                    SnapPointsAlignment = SnapPointsAlignment.Center,
                    Span = 8,
                    VerticalItemSpacing = 2
                }
            };
            collectionView.SizeChanged += AdaptiveGrid_Main_SizeChanged;

            var frameView = new Border
            {
                Padding = 1,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(15)
                },
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

            DecksLayout.Tabs.Add(tabItem);
            return collectionView;
        }
    }

    private void AdaptiveGrid_Main_SizeChanged(object? sender, EventArgs e)
    {
        viewModel.TabViewTabPlacement = AdaptiveGrid_Main.HorizontalDisplay ? TabViewTabPlacement.Top : TabViewTabPlacement.Bottom;
    }

    private async void CheckDeck_Button_Pressed(object sender, EventArgs e)
    {
        var cancellationToken = default(CancellationToken);
        var statisticSections = await deckbuilder.GetDeckStatsAsync(true, cancellationToken);
        var modalPage = new DeckStatistics(statisticSections, pageService);
        await pageService.OpenModalPage(modalPage, false, cancellationToken);
    }

    private void DecksLayout_SelectedTabChanged(object? _, TabItem e)
    {
        if (e.BindingContext is not DecksteriaDeck deck)
        {
            return;
        }

        viewModel.ActiveDeckTab = deck.Name;
    }

    private async Task LoadDecklistAsync(Decklist decklist, CancellationToken cancellationToken = default)
    {
        // Ask user if they want to switch formats
        if (decklist.Format != deckbuilder.FormatName)
        {
            var switchFormats = await DisplayAlert("Switch formats?", "The current format does not support the decklist, do you want to switch formats?", "Continue", "Cancel");
            if (!switchFormats)
            {
                viewModel.Loading = false;
                return;
            }

            deckbuilderFactory.ChangeFormat(decklist.Format);
            deckbuilder = deckbuilderFactory.GetCurrentDeckbuildingService();
            viewModel.WindowTitle = $"{deckbuilder.GameTitle} Deckbuilder - {deckbuilder.FormatTitle}";
        }

        await deckbuilder.LoadDecklistAsync(decklist, cancellationToken);
        await UpdateDeckCollections(cancellationToken: cancellationToken);
    }

    private async void MenuButtonExport_Pressed(object sender, EventArgs e)
    {
        var cancellationToken = new CancellationToken();

        var exportOptions = deckFileService.GetExportFileTypes();
        var exportLabels = exportOptions.Keys.ToArray();

        var selectedExport = await DisplayActionSheet("Export to...", "Cancel", null, exportLabels);
        if (selectedExport == "Cancel")
        {
            return;
        }

        using var memoryStream = await deckFileService.ExportDecklistAsync(selectedExport, deckbuilder.CreateDecklist(), cancellationToken);

        var extension = exportOptions[selectedExport].TrimStart('.');
        var fileSaveResult = await FileSaver.Default.SaveAsync($"{viewModel.DecklistName}.{extension}", memoryStream, cancellationToken);

        // Provide user se
        if (fileSaveResult.IsSuccessful)
        {
            var fileName = Path.GetFileName(fileSaveResult.FilePath);
            await DisplayAlert("Success", $"File saved to: {fileName}", "OK");
        }
        else
        {
            await DisplayAlert("Error", $"File not saved: {fileSaveResult.Exception.Message}", "OK");
        }
    }

    private async void MenuButtonImport_Pressed(object sender, EventArgs e)
    {
        var importOptions = deckFileService.GetImportFileTypes();
        var importLabels = importOptions.Keys.ToArray();

        var selectedImport = await DisplayActionSheet("Import from...", "Cancel", null, importLabels);
        if (selectedImport == "Cancel")
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(viewModel.DecklistName))
        {
            var overwrite = await DisplayAlert("Overwrite existing deck?", "Do you want to use a different name for the deck?", "Yes", "No", FlowDirection.LeftToRight);
            if (overwrite)
            {
                viewModel.DecklistName = string.Empty;
            }
        }

        var extension = importOptions[selectedImport].TrimStart('.');
        var fileOpenResult = await FilePicker.Default.PickAsync(new()
        {
            FileTypes = GetFilePickerTypes(extension),
            PickerTitle = $"Please select a {selectedImport} file"
        });

        var fullPath = fileOpenResult?.FullPath;
        if (string.IsNullOrWhiteSpace(fullPath))
        {
            return;
        }

        viewModel.Loading = true;
        var decklist = await deckFileService.ImportDecklistAsync(fullPath, selectedImport);
        await LoadDecklistAsync(decklist);
        viewModel.Loading = false;

        static FilePickerFileType GetFilePickerTypes(string extension)
        {
            return new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { "public.data" } }, // Generic UTType for custom data files
                { DevicePlatform.Android, new[] { "application/octet-stream" } }, // Generic MIME type for binary files
                { DevicePlatform.WinUI, new[] { $".{extension}" } }, // Custom file extensions for Windows
                { DevicePlatform.MacCatalyst, new[] { "public.data" } } // Generic UTType for custom data files
            });
        }
    }

    private async void MenuButtonSave_Pressed(object sender, EventArgs e)
    {
        const string save = "Save";
        const string saveAs = "Save As";
        var action = saveAs;
        if (!string.IsNullOrWhiteSpace(viewModel.DecklistName))
        {
            action = await DisplayActionSheet("Save as new?", "Cancel", null, save, saveAs);
        }

        // Return if action selection was cancelled.
        if (action is null)
        {
            return;
        }

        var deckName = viewModel.DecklistName;
        if (action == saveAs)
        {
            var isValidFileName = true;
            do
            {
                deckName = await DisplayPromptAsync("Deck Name", "What name do you want to save it as?", null, default, viewModel.DecklistName, 20, Keyboard.Default);

                // If prompt was cancelled, exit out of the function.
                if (deckName is null)
                {
                    return;
                }

                // Check that file name is valid.
                isValidFileName = IsValidFileName(deckName);
                if (!isValidFileName)
                {
                    await DisplayAlert("Error", "Deck name cannot contain any of these characters.\n\\/:*?\"<>|", "OK");
                }
            } while (!isValidFileName);
        }

        await deckFileService.SaveDecklistAsync(deckName, ConvertToCardArtIdDictionary(viewModel.Decks));
        await DisplayAlert("Deck Saved", $"Your deck {deckName} has been saved.", "OK");

        // Create Decklist in CardArtId format.
        static Dictionary<string, IEnumerable<CardArtId>> ConvertToCardArtIdDictionary(IDictionary<string, ObservableCollection<CardArt>> decklist)
        {
            return decklist.ToDictionary
            (
                keyValue => keyValue.Key,
                keyValue => keyValue.Value.Select(card => new CardArtId(card.CardId, card.ArtId))
            );
        }

        static bool IsValidFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var arrayIntersect = invalidChars.Intersect(fileName.ToCharArray());
            return !arrayIntersect.Any();
        }
    }

    private async void SearchAdvancedFilter_Pressed(object? sender, EventArgs e)
    {
        searchModal = await pageService.OpenFormPage<SearchModal>(searchModal);
        if (!searchModal.IsSubmitted)
        {
            return;
        }

        if (!searchModal.ViewModel.SearchFieldFilters.Any())
        {
            SearchClearFilter_Pressed(sender, e);
            return;
        }

        viewModel.AdvancedFiltersApplied = true;
        searchFieldFilters = searchModal.ViewModel.SearchFieldFilters.SelectMany(f => f.AsSearchFieldFilterArray());
        await SearchExecuteAsync();
    }

    private async void SearchClearFilter_Pressed(object? sender, EventArgs e)
    {
        searchModal = null;
        searchFieldFilters = Array.Empty<ISearchFieldFilter>();
        viewModel.AdvancedFiltersApplied = false;
        await SearchExecuteAsync();
    }

    private async void SearchTextSearch_Entered(object? sender, EventArgs e)
    {
        await SearchExecuteAsync();
    }

    private async Task SearchExecuteAsync(CancellationToken cancellationToken = default)
    {
        if (viewModel.SearchText.Length < 3 && !searchFieldFilters.Any())
        {
            viewModel.FilteredCards.Clear();
            return;
        }

        viewModel.Searching = true;
        var results = await deckbuilder.GetCardsAsync(viewModel.SearchText, searchFieldFilters, cancellationToken);
        viewModel.FilteredCards.UpdateData(results);
        viewModel.Searching = false;
    }

    private async void TapGestureRecognizer_PrimaryTapped(object? sender, TappedEventArgs e)
    {
        if (sender is not Image image || image.BindingContext is not CardArt card)
        {
            return;
        }

        var cardInfo = new CardInfo(card, deckbuilder, pageService);
        var page = await pageService.OpenModalPage(cardInfo);
        if (page.DecksChanged)
        {
            // Loading is only necessary if multiple decks will be refreshed.
            viewModel.Loading = true;
            await UpdateDeckCollections(null);
            viewModel.Loading = false;
        }
    }

    private async void TapGestureRecognizer_SearchSecondaryTapped(object? sender, TappedEventArgs e)
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

    private async void TapGestureRecognizer_DeckSecondaryTapped(object? sender, TappedEventArgs e)
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

    private async Task UpdateDeckCollections(string? deckName = null, CancellationToken cancellationToken = default)
    {
        if (deckName is null)
        {
            var collectionTasks = viewModel.Decks.Select(deck => UpdateDeckCollections(deck.Key));
            await Task.WhenAll(collectionTasks);
            return;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var newData = deckbuilder.GetDeckCards(deckName);
        if (newData is null || !viewModel.Decks.TryGetValue(deckName, out var collection) || deckViews is null || !deckViews.TryGetValue(deckName, out var collectionView))
        {
            return;
        }

        collection.UpdateData(newData);
        collectionView.ItemsSource = collection;
        viewModel.ValidDeckStatus = await deckbuilder.ValidDecklistAsync(cancellationToken);
    }
}