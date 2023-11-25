namespace Decksteria.Ui.Maui;

using CommunityToolkit.Maui;
using Decksteria.Core;
using Decksteria.Core.Data;
using Decksteria.Service;
using Decksteria.Ui.Maui.Pages.LoadPlugIn;
using Decksteria.Ui.Maui.Services.DialogService;
using Decksteria.Ui.Maui.Services.FileReader;
using Decksteria.Ui.Maui.Services.PageService;
using Decksteria.Ui.Maui.Services.PlugInFactory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts => fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"));

        // builder.Services.AddMauiBlazorWebView();

#if DEBUG
        // builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

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
        services.AddSingleton<IDecksteriaPlugInFactory, DecksteriaPlugInFactory>();
        services.AddScoped<IDecksteriaGame>((sp) =>
        {
            var formatFactory = sp.GetRequiredService<IDecksteriaPlugInFactory>();
            return formatFactory.CreatePlugInInstance();
        });
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IDecksteriaFileReader, DecksteriaFileReader>();
        services.AddSingleton<IPageService, PageService>();

        return services;
    }
}
