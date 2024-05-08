namespace Decksteria.Ui.Maui;

using System;
using CommunityToolkit.Maui;
using Decksteria.Services;
using Decksteria.Services.PlugInFactory;
using Decksteria.Services.PlugInFactory.Models;
using Decksteria.Ui.Maui.Services.DialogService;
using Decksteria.Ui.Maui.Services.PageService;
using Decksteria.Ui.Maui.Services.PlugInFactory;
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
        services.TryAddScoped<GameFormat>((sp) =>
        {
            var formatFactory = sp.GetRequiredService<IDecksteriaPlugInFactory>();
            return formatFactory.GetSelectedFormat();
        });
        services.AddHttpClient();
        services.TryAddScoped<IDialogService, DialogService>();
        services.TryAddScoped<IPageService, PageService>();

        return services;
    }
}
