namespace Decksteria.Service;

using Decksteria.Core;
using Decksteria.Service.Deckbuilding;
using Decksteria.Service.DecksteriaFile;
using Decksteria.Service.DecksteriaPluginService;
using Decksteria.Service.DecksteriaPluginService.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDecksteriaStrategyServices(this IServiceCollection services)
    {
        var decksteriaGameStrategy = new DecksteriaGameStrategy();
        services.TryAddSingleton<IDecksteriaGameStrategy>(decksteriaGameStrategy);
        services.TryAddSingleton<IDecksteriaGame>(decksteriaGameStrategy);

        var decksteriaFormatStrategy = new DecksteriaFormatStrategy();
        services.TryAddSingleton<IDecksteriaFormatStrategy>(decksteriaFormatStrategy);
        services.TryAddSingleton<IDecksteriaFormat>(decksteriaFormatStrategy);

        services.TryAddSingleton<IPlugInManagerService, PlugInManagerService>();

        return services;
    }

    public static IServiceCollection AddDecksteriaFileServices(this IServiceCollection services)
    {
        services.TryAddScoped<IDecksteriaFileService, DecksteriaFileService>();
        return services;
    }

    public static IServiceCollection AddDeckbuildingServices(this IServiceCollection services)
    {
        services.TryAddScoped<IDeckbuildingService, DeckbuildingService>();
        return services;
    }
}
