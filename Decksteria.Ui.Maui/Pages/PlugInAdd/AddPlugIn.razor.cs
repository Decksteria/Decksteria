namespace Decksteria.Ui.Maui.Pages.PlugInAdd;

using Decksteria.Ui.Maui.Services.DialogService;
using Decksteria.Ui.Maui.Services.PlugInInitializer;
using Decksteria.Ui.Maui.Shared.Models;
using Microsoft.AspNetCore.Components;

public partial class AddPlugIn
{
    private const string InvalidPlugInFile = "This file is not a Plug-In file. Plug-In files are '.dll' files.";

    private const string IncompatiblePlugInFile = "This Plug-In file is not compatible with the Decksteria Application. Try another file.";

    private static readonly FilePickerFileType dllFileTypes = new(new Dictionary<DevicePlatform, IEnumerable<string>>()
    {
        { DevicePlatform.iOS, new[] { "public.data" } },
        { DevicePlatform.Android, new[] { "application/octet-stream" } },
        { DevicePlatform.macOS, new[] { "public.data" } },
        { DevicePlatform.WinUI, new[] { ".dll" } },
        { DevicePlatform.Tizen, new[] { "*/*" } }
    });

    [Inject]
    protected IDialogService DialogService { get; set; } = default!;

    [Inject]
    protected IPlugInInitializer PlugInInitializer { get; set; } = default!;

    private string? ErrorMessage;

    private IEnumerable<PlugInTile>? GameList;

    private bool ProcessingInProgress = true;

    protected override async Task OnInitializedAsync()
    {
        var plugIns = await PlugInInitializer.GetOrInitializeAllPlugInsAsync();
        GameList = plugIns.Select(pi => new PlugInTile(pi));

        await base.OnInitializedAsync();

        ProcessingInProgress = false;
    }

    private async Task UploadFileOnClickAsync()
    {
        ProcessingInProgress = true;
        var result = await FilePicker.Default.PickAsync(new()
        {
            PickerTitle = "Upload a Plug-In File",
            FileTypes = dllFileTypes
        });

        ErrorMessage = null;
        if (result == null)
        {
            ProcessingInProgress = false;
            return;
        }

        // Verify File is Compatible
        if (!result.FileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
        {
            await DisplayErrorMessage(InvalidPlugInFile);
            ProcessingInProgress = false;
            return;
        }

        var plugIn = PlugInInitializer.TryGetNewPlugIn(result.FullPath);
        if (plugIn == null)
        {
            await DisplayErrorMessage(IncompatiblePlugInFile);
            ProcessingInProgress = false;
            return;
        }

        var plugIns = await PlugInInitializer.GetOrInitializeAllPlugInsAsync();
        GameList = plugIns.Select(pi => new PlugInTile(pi));
        ProcessingInProgress = false;
    }

    private async Task DisplayErrorMessage(string errorMessage)
    {
        ErrorMessage = !await DialogService.DisplayMessage("Error", errorMessage) ? errorMessage : null;
    }
}
