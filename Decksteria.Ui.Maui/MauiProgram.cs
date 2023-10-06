namespace Decksteria.Ui.Maui;

using Decksteria.Core.Data;
using Decksteria.Service;
using Decksteria.Ui.Maui.Services.DialogService;
using Decksteria.Ui.Maui.Services.FileReader;
using Decksteria.Ui.Maui.Services.PlugInInitializer;
using Microsoft.Extensions.Logging;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        builder.Services.AddDecksteriaStrategyServices();
        builder.Services.AddDecksteriaFileServices();
        builder.Services.AddDeckbuildingServices();
        builder.Services.AddSingleton<IPlugInInitializer, PlugInInitializer>();
        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<IDecksteriaFileReader, DecksteriaFileReader>();

        return builder.Build();
    }
}
