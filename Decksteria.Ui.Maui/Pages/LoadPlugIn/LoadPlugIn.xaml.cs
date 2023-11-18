namespace Decksteria.Ui.Maui.Pages.LoadPlugIn;

using Decksteria.Core;
using Decksteria.Ui.Maui.Services.PageService;
using Decksteria.Ui.Maui.Services.PlugInInitializer;
using Decksteria.Ui.Maui.Shared.Models;

public partial class LoadPlugIn : ContentPage
{
    private const string ErrorAlertTitle = "Error";

    private const string SuccessAlertTitle = "Success";

    private const string InformationButtonText = "OK";

    private const string InvalidPlugInFile = "This file is not a Plug-In file. Plug-In files are '.dll' files.";

    private const string IncompatiblePlugInFile = "This Plug-In file is not compatible with the Decksteria Application. Try another file.";

    private const string ProblemLoading = "There was a problem loading the item. Please restart the application.";

    private const string AddedPlugInSuccess = "Successfully added new Plug-In or replaced old Plug-In.";

    private bool ProcessingInProgress;

    private static readonly FilePickerFileType dllFileTypes = new(new Dictionary<DevicePlatform, IEnumerable<string>>()
    {
        { DevicePlatform.iOS, new[] { "public.data" } },
        { DevicePlatform.Android, new[] { "application/octet-stream" } },
        { DevicePlatform.macOS, new[] { "public.data" } },
        { DevicePlatform.WinUI, new[] { ".dll" } },
        { DevicePlatform.Tizen, new[] { "*/*" } }
    });

    private static readonly FilePickerFileType jsonFileTypes = new(new Dictionary<DevicePlatform, IEnumerable<string>>()
    {
        { DevicePlatform.iOS, new[] { "public.text" } },
        { DevicePlatform.Android, new[] { "application/json" } },
        { DevicePlatform.macOS, new[] { "public.text" } },
        { DevicePlatform.WinUI, new[] { ".json" } },
        { DevicePlatform.Tizen, new[] { "*/*" } }
    });

    private readonly IPageService pageService;

    private readonly IPlugInInitializer plugInInitializer;

    private readonly LoadPluginViewModel viewModel = new();

    public LoadPlugIn(IPageService pageService, IPlugInInitializer plugInInitializer)
    {
        this.pageService = pageService;
        this.plugInInitializer = plugInInitializer;
        this.BindingContext = this.viewModel;

        InitializeComponent();
    }

    private async void ContentPage_LoadedAsync(object sender, EventArgs e)
    {
        var plugIns = await plugInInitializer.GetOrInitializeAllPlugInsAsync();
        UpdatePlugInList(plugIns);
        ListView_PlugInSelect.ItemsSource = viewModel.GameTiles;
    }

    private async void ListView_PlugInSelect_New_Clicked(object sender, EventArgs e)
    {
        if (ProcessingInProgress)
        {
            return;
        }

        ProcessingInProgress = true;
        var result = await FilePicker.Default.PickAsync(new()
        {
            PickerTitle = "Upload a Plug-In File",
            FileTypes = dllFileTypes
        });

        if (result is null)
        {
            ProcessingInProgress = false;
            return;
        }

        // Verify File is Compatible
        if (!result.FileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
        {
            ProcessingInProgress = false;
            await DisplayAlert(ErrorAlertTitle, InvalidPlugInFile, InformationButtonText);
            return;
        }

        var plugIn = plugInInitializer.TryGetNewPlugIn(result.FullPath);
        if (plugIn is null)
        {
            await DisplayAlert(ErrorAlertTitle, IncompatiblePlugInFile, InformationButtonText);
            ProcessingInProgress = false;
            return;
        }

        var plugIns = await plugInInitializer.GetOrInitializeAllPlugInsAsync();
        UpdatePlugInList(plugIns);
        ProcessingInProgress = false;

        await DisplayAlert(SuccessAlertTitle, AddedPlugInSuccess, InformationButtonText);
    }

    private void ListView_PlugInSelect_ItemTapped(object sender, EventArgs e)
    {
        var senderBinding = (sender as ViewCell)?.BindingContext;
        if (senderBinding is not PlugInTile)
        {
            DisplayAlert(ErrorAlertTitle, ProblemLoading, InformationButtonText);
            return;
        }

        UpdateFormatList((PlugInTile) senderBinding);
        ListView_PlugInSelect.FadeTo(0, 100, Easing.Linear);
        viewModel.FormatsExpanded = true;
        ListView_FormatSelect.FadeTo(1, 100, Easing.Linear);
    }

    private void ListView_FormatSelect_Back_Clicked(object sender, EventArgs e)
    {
        ListView_FormatSelect.FadeTo(0, 100, Easing.Linear);
        viewModel.PlugInsExpanded = true;
        ListView_PlugInSelect.FadeTo(1, 100, Easing.Linear);
    }

    private void ListView_FormatSelect_ItemTapped(object sender, EventArgs e)
    {
        var senderBinding = (sender as ViewCell)?.BindingContext;
        if (senderBinding is not FormatTile)
        {
            DisplayAlert(ErrorAlertTitle, ProblemLoading, InformationButtonText);
            return;
        }

        UpdateDeckList((FormatTile) senderBinding);
        ListView_PlugInSelect.FadeTo(0, 100, Easing.Linear);
        viewModel.DecksExpanded = true;
        ListView_FormatSelect.FadeTo(1, 100, Easing.Linear);
    }

    private void ListView_DeckSelect_Back_Clicked(object sender, EventArgs e)
    {
        ListView_DeckSelect.FadeTo(0, 100, Easing.Linear);
        viewModel.FormatsExpanded = true;
        ListView_FormatSelect.FadeTo(1, 100, Easing.Linear);
    }

    private async void ListView_DeckSelect_Upload_Clicked(object sender, EventArgs e)
    {
        if (ProcessingInProgress)
        {
            return;
        }

        ProcessingInProgress = true;
        var result = await FilePicker.Default.PickAsync(new()
        {
            PickerTitle = "Open a Deckbuilder File",
            FileTypes = jsonFileTypes
        });

        ProcessingInProgress = false;
    }

    private void ListView_DeckSelect_Open_Clicked(object sender, EventArgs e)
    {
        if (ListView_DeckSelect.SelectedItem is not DeckTile)
        {
            return;
        }

        var selectedItem = (DeckTile) ListView_DeckSelect.SelectedItem;
        // Open Deckbuilder Window
        pageService.OpenPageAsync(new Page());
    }

    private void UpdatePlugInList(IEnumerable<IDecksteriaGame> plugIns)
    {
        viewModel.GameTiles.Clear();
        foreach (var plugIn in plugIns)
        {
            viewModel.GameTiles.Add(new PlugInTile(plugIn));
        }
    }

    private void UpdateFormatList(PlugInTile plugInTile) => ListView_FormatSelect.ItemsSource = plugInTile.Formats;

    private void UpdateDeckList(FormatTile formatTile)
    {
        // Get All Deck files from the Plug-In Format Application Path
        var deckDirectory = Path.Combine(FileSystem.AppDataDirectory, formatTile.DeckDirectory);

        if (Path.Exists(deckDirectory))
        {
            var files = Directory.GetFiles(deckDirectory, "*.json", SearchOption.TopDirectoryOnly);
            var deckTiles = files.Select(file => new DeckTile(file));
            ListView_DeckSelect.ItemsSource = deckTiles;
        }
    }

    private void ListView_DeckSelect_New_Clicked(object sender, EventArgs e)
    {
        // TODO: Create Deckbuilder Page
        pageService.OpenPageAsync(new Page());
        Console.WriteLine("New Deck Button Clicked");
    }
}
