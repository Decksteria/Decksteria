namespace Decksteria.Ui.Maui;

using System;
using CommunityToolkit.Maui;
using Decksteria.Core.Data;
using Decksteria.Services;
using Decksteria.Services.PlugInFactory;
using Decksteria.Services.PlugInFactory.Models;
using Decksteria.Ui.Maui.Pages.Deckbuilder;
using Decksteria.Ui.Maui.Services.CardImageService;
using Decksteria.Ui.Maui.Services.DeckFileService;
using Decksteria.Ui.Maui.Services.DialogService;
using Decksteria.Ui.Maui.Services.FileLocator;
using Decksteria.Ui.Maui.Services.FileReader;
using Decksteria.Ui.Maui.Services.LoggingProvider;
using Decksteria.Ui.Maui.Services.PageService;
using Decksteria.Ui.Maui.Services.PlugInFactory;
using Decksteria.Ui.Maui.Services.PreferencesService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using UraniumUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseUraniumUI()
            .UseUraniumUIMaterial()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFontAwesomeIconFonts();
            });

        builder.Logging.ClearProviders();
        builder.Logging.AddProvider(new LoggingProvider(TimeProvider.System));

#if DEBUG
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
        services.TryAddSingleton<AppShell>();
        services.TryAddSingleton<Lazy<AppShell>>((sp) => new(() => sp.GetRequiredService<AppShell>()));
        return services;
    }

    private static IServiceCollection AddMAUIServices(this IServiceCollection services)
    {
        services.AddSingleton<IDecksteriaPlugInFactory, DecksteriaPlugInFactory>();
        services.TryAddTransient<GameFormat>((sp) =>
        {
            var formatFactory = sp.GetRequiredService<IDecksteriaPlugInFactory>();
            return formatFactory.GetSelectedFormat();
        });
        services.AddHttpClient();
        services.TryAddSingleton<IPreferencesService, PreferencesService>();
        services.TryAddSingleton(sp =>
        {
            var preferenceService = sp.GetRequiredService<IPreferencesService>();
            return preferenceService.GetConfiguration();
        });
        services.TryAddScoped<IDialogService, DialogService>();
        services.TryAddScoped<IDeckFileServiceFactory, DeckFileServiceFactory>();
        services.TryAddScoped<IDecksteriaFileLocator, DecksteriaFileLocator>();
        services.TryAddScoped<IDecksteriaCardImageService, DecksteriaCardImageService>();
        services.TryAddScoped<IDecksteriaFileReader, DecksteriaFileReader>();

        services.TryAddScoped<DeckbuilderViewModel>();
        // Use a different implementation for the Paging Service that turns Modals into a new Window.
#if WINDOWS
        services.TryAddScoped<IPageService, PageService>();
#else
        services.TryAddScoped<IPageService, PageService>();
#endif

        return services;
    }
}
