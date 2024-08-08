namespace Decksteria.Services;

using Decksteria.Core;
using Decksteria.Services.Deckbuilding;
using Decksteria.Services.FileService;
using Decksteria.Services.PlugInFactory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDecksteriaFileServices(this IServiceCollection services)
    {
        services.TryAddScoped<IDecksteriaFileService, DecksteriaFileService>();
        return services;
    }

    public static IServiceCollection AddDeckbuildingServices(this IServiceCollection services)
    {
        services.TryAddScoped<IDeckbuildingService>(options =>
        {
            var pluginFactory = options.GetRequiredService<IDecksteriaPlugInFactory>();
            var gameFormat = pluginFactory.GetSelectedFormat();
            return new DeckbuildingService<IDecksteriaFormat>(gameFormat.Game, gameFormat.Format);
        });
        return services;
    }
}
