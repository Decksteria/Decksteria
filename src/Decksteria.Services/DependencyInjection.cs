namespace Decksteria.Services;

using Decksteria.Services.Deckbuilding;
using Decksteria.Services.DeckFileService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDecksteriaFileServices(this IServiceCollection services)
    {
        services.TryAddScoped<IDecksteriaDeckFileService, DecksteriaDeckFileService>();
        return services;
    }

    public static IServiceCollection AddDeckbuildingServices(this IServiceCollection services)
    {
        services.TryAddScoped<IDeckbuildingServiceFactory, DeckbuildingServiceFactory>();
        return services;
    }
}
