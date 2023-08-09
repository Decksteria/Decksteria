namespace Decksteria.Service;

using Decksteria.Core;
using Decksteria.Service.DecksteriaFile;
using Decksteria.Service.DecksteriaFile.Models;
using Decksteria.Service.DecksteriaPluginService;
using Decksteria.Service.DecksteriaPluginService.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDecksteriaServices(this IServiceCollection services, IEnumerable<IDecksteriaGame> decksteriaGames)
    {
        var decksteriaGameStrategy = new DecksteriaGameStrategy();
        services.TryAddSingleton<IDecksteriaGameStrategy>(decksteriaGameStrategy);
        services.TryAddSingleton<IDecksteriaGame>(decksteriaGameStrategy);

        var decksteriaFormatStrategy = new DecksteriaFormatStrategy();
        services.TryAddSingleton<IDecksteriaFormatStrategy>(decksteriaFormatStrategy);
        services.TryAddSingleton<IDecksteriaFormat>(decksteriaFormatStrategy);

        services.TryAddSingleton<IPlugInManagerService>(provider =>
        {
            var gameStrategy = provider.GetRequiredService<IDecksteriaGameStrategy>();
            var formatStrategy = provider.GetRequiredService<IDecksteriaFormatStrategy>();
            return new PlugInManagerService(gameStrategy, formatStrategy, decksteriaGames);
        });

        return services;
    }

    public static IServiceCollection AddDecksteriaFileServices(this IServiceCollection services)
    {
        services.TryAddScoped<IDecksteriaFileService, DecksteriaFileService>();
        services.TryAddScoped<DeckFileMapper>();
        return services;
    }
}
