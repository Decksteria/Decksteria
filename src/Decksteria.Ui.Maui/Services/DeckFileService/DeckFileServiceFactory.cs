namespace Decksteria.Ui.Maui.Services.DeckFileService;

using System;
using Decksteria.Services.DeckFileService;
using Decksteria.Services.PlugInFactory.Models;
using Microsoft.Extensions.DependencyInjection;

internal sealed class DeckFileServiceFactory : IDeckFileServiceFactory
{
    private readonly IServiceProvider services;

    public DeckFileServiceFactory(IServiceProvider services)
    {
        this.services = services;
    }

    public IDeckFileService GetDeckFileService()
    {
        var gameFormat = services.GetRequiredService<GameFormat>();
        var deckFileService = services.GetRequiredService<IDecksteriaDeckFileService>();
        return new DeckFileService(gameFormat, deckFileService);
    }
}
