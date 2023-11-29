namespace Decksteria.Services;

using Decksteria.Services.FileService;
using Decksteria.Services.Deckbuilding;
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
        services.TryAddScoped<IDeckbuildingService, DeckbuildingService>();
        return services;
    }
}
