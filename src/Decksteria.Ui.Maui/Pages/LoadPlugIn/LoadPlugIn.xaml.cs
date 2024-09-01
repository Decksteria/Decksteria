namespace Decksteria.Ui.Maui.Pages.LoadPlugIn;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Decksteria.Services.PlugInFactory;
using Decksteria.Services.PlugInFactory.Models;
using Decksteria.Ui.Maui.Pages.Deckbuilder;
using Decksteria.Ui.Maui.Services.DeckFileService;
using Decksteria.Ui.Maui.Services.PageService;
using Decksteria.Ui.Maui.Shared.Extensions;
using Decksteria.Ui.Maui.Shared.Models;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using UraniumUI.Pages;

public partial class LoadPlugIn : UraniumContentPage
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
        { DevicePlatform.Android, new[] { "application/*" } },
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

    private readonly IDecksteriaPlugInFactory plugInFactory;

    private readonly IDeckFileService deckFileService;

    private readonly LoadPluginViewModel viewModel = new();

    public LoadPlugIn(IPageService pageService, IDecksteriaPlugInFactory plugInFactory, IDeckFileService deckFileService)
    {
        this.pageService = pageService;
        this.plugInFactory = plugInFactory;
        this.deckFileService = deckFileService;
        this.BindingContext = this.viewModel;

        InitializeComponent();
    }

    private void ContentPage_LoadedAsync(object sender, EventArgs e)
    {
        var plugIns = plugInFactory.GetOrInitializePlugIns();
        UpdatePlugInList(plugIns);
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

        var plugInLoaded = plugInFactory.TryAddGame(result.FullPath);
        if (!plugInLoaded)
        {
            await DisplayAlert(ErrorAlertTitle, IncompatiblePlugInFile, InformationButtonText);
            ProcessingInProgress = false;
            return;
        }

        var plugIns = plugInFactory.GetOrInitializePlugIns();
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
        viewModel.SelectedPlugIn = null;
        ListView_PlugInSelect.FadeTo(1, 100, Easing.Linear);
    }

    private async void ListView_FormatSelect_ItemTapped(object sender, EventArgs e)
    {
        var senderBinding = (sender as ViewCell)?.BindingContext;
        if (senderBinding is not FormatTile)
        {
            await DisplayAlert(ErrorAlertTitle, ProblemLoading, InformationButtonText);
            return;
        }

        await UpdateDeckListAsync((FormatTile) senderBinding);
        await ListView_FormatSelect.FadeTo(0, 100, Easing.Linear);
        viewModel.DecksExpanded = true;
        await ListView_DeckSelect.FadeTo(1, 100, Easing.Linear);
    }

    private void ListView_DeckSelect_Back_Clicked(object sender, EventArgs e)
    {
        ListView_DeckSelect.FadeTo(0, 100, Easing.Linear);
        viewModel.FormatsExpanded = true;
        viewModel.SelectedFormat = null;
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

        plugInFactory.SelectGame(viewModel.SelectedPlugIn!.Name, viewModel.SelectedFormat!.Name);
        var selectedItem = (DeckTile) ListView_DeckSelect.SelectedItem;
        // Open Deckbuilder Window
        pageService.OpenPageAsync<Deckbuilder>();
    }

    private void UpdatePlugInList(IEnumerable<DecksteriaPlugIn> plugIns)
    {
        viewModel.GameTiles.Clear();
        foreach (var plugIn in plugIns)
        {
            viewModel.GameTiles.Add(new PlugInTile(plugIn));
        }
    }

    private void UpdateFormatList(PlugInTile plugInTile)
    {
        viewModel.FormatTiles.Clear();
        viewModel.SelectedPlugIn = plugInTile;
        foreach (var format in plugInTile.Formats)
        {
            viewModel.FormatTiles.Add(format);
        }
    }

    private async Task UpdateDeckListAsync(FormatTile formatTile)
    {
        // Get All Deck files from the Plug-In Format Application Path
        viewModel.SelectedFormat = formatTile;
        var deckNames = await deckFileService.GetSavedDecksAsync();
        var deckTiles = deckNames.Select(x => new DeckTile(x, true));
        viewModel.DeckTiles.ReplaceData(deckTiles);
    }

    private void ListView_DeckSelect_New_Clicked(object sender, EventArgs e)
    {
        plugInFactory.SelectGame(viewModel.SelectedPlugIn!.Name, viewModel.SelectedFormat!.Name);
        pageService.OpenPageAsync<Deckbuilder>();
    }
}
