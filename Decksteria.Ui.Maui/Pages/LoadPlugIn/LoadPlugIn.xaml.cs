﻿namespace Decksteria.Ui.Maui;

using Decksteria.Core;
using Decksteria.Ui.Maui.Pages.LoadPlugIn;
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

    private readonly IPlugInInitializer plugInInitializer;

    private LoadPluginViewModel viewModel;

    public LoadPlugIn(IPlugInInitializer plugInInitializer, LoadPluginViewModel viewModel)
    {
        this.plugInInitializer = plugInInitializer;
        this.viewModel = viewModel;
        this.BindingContext = this.viewModel;

        InitializeComponent();
    }

    private async void ContentPage_LoadedAsync(object sender, EventArgs e)
    {
        var plugIns = await plugInInitializer.GetOrInitializeAllPlugInsAsync();
        UpdatePlugInList(plugIns);
    }

    private async void Button_NewPlugIn_ClickedAsync(object sender, EventArgs e)
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

        if (result == null)
        {
            ProcessingInProgress = false;
            return;
        }

        // Verify File is Compatible
        if (!result.FileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
        {
            ProcessingInProgress = false;
            await this.DisplayAlert(ErrorAlertTitle, InvalidPlugInFile, InformationButtonText);
            return;
        }

        var plugIn = plugInInitializer.TryGetNewPlugIn(result.FullPath);
        if (plugIn == null)
        {
            await this.DisplayAlert(ErrorAlertTitle, IncompatiblePlugInFile, InformationButtonText);
            ProcessingInProgress = false;
            return;
        }

        var plugIns = await plugInInitializer.GetOrInitializeAllPlugInsAsync();
        UpdatePlugInList(plugIns);
        ProcessingInProgress = false;

        await this.DisplayAlert(SuccessAlertTitle, AddedPlugInSuccess, InformationButtonText);
    }

    private void Button_NewDeck_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("New Deck Button Clicked");
    }

    private async void Button_OpenDeck_ClickedAsync(object sender, EventArgs e)
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

    private void Back_Button_Clicked(object sender, EventArgs e)
    {

    }

    private void ListView_PlugInSelect_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is not PlugInTile)
        {
            this.DisplayAlert(ErrorAlertTitle, ProblemLoading, InformationButtonText);
            return;
        }

        UpdateFormatList((PlugInTile) e.SelectedItem);
        ListView_PlugInSelect.FadeTo(0, 1000, Easing.Linear);
        ListView_FormatSelect.FadeTo(1, 1000, Easing.Linear);
    }

    private void UpdatePlugInList(IEnumerable<IDecksteriaGame> plugIns)
    {
        viewModel.GameTiles.Clear();
        viewModel.FormatTiles = new FormatTile[0];
        foreach (var plugIn in plugIns)
        {
            viewModel.GameTiles.Add(new PlugInTile(plugIn));
        }
    }

    private void UpdateFormatList(PlugInTile plugInTile)
    {
        viewModel.FormatTiles = plugInTile.Formats;
    }
}
