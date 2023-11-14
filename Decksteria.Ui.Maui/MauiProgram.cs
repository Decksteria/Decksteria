namespace Decksteria.Ui.Maui;

using CommunityToolkit.Maui;
using Decksteria.Core.Data;
using Decksteria.Service;
using Decksteria.Ui.Maui.Pages.LoadPlugIn;
using Decksteria.Ui.Maui.Services.DialogService;
using Decksteria.Ui.Maui.Services.FileReader;
using Decksteria.Ui.Maui.Services.PageService;
using Decksteria.Ui.Maui.Services.PlugInInitializer;
using Microsoft.Extensions.Logging;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
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
        builder.Services.AddDecksteriaMAUI();

        return builder.Build();
    }

    private static IServiceCollection AddDecksteriaMAUI(this IServiceCollection services)
    {
        services.AddMAUIServices();

        services.AddSingleton<LoadPlugIn>();
        return services;
    }

    private static IServiceCollection AddMAUIServices(this IServiceCollection services)
    {
        services.AddSingleton<IPlugInInitializer, PlugInInitializer>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IDecksteriaFileReader, DecksteriaFileReader>();
        services.AddSingleton<IPageService, PageService>();

        return services;
    }
}
