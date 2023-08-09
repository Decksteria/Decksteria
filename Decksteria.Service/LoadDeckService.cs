namespace Decksteria.Service;

using Decksteria.Core;
using Decksteria.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class LoadDeckService
{
    private readonly IEnumerable<IDecksteriaGame> gamePlugins;

    public LoadDeckService(IEnumerable<IDecksteriaGame> gamePlugins)
    {
        this.gamePlugins = gamePlugins;
    }

    public Decklist LoadDecksteriaFilter(MemoryStream memoryStream)
    {
        var deckGame = "";
        var deckFormat = "";

        var plugIn = gamePlugins.FirstOrDefault(game => game.Name == deckGame) ?? throw new IndexOutOfRangeException($"{deckGame} Plug-In not loaded.");
        var format = plugIn.Formats.FirstOrDefault(format => format.Name == deckFormat) ?? throw new IndexOutOfRangeException($"{deckFormat} not found in {plugIn.DisplayName} Plug-In.");

        return new(plugIn, format, new Dictionary<IDecksteriaDeck, IEnumerable<CardArt>>());
    }

    public MemoryStream ReadDecksteriaFilter(Decklist decklist)
    {

    }
}
