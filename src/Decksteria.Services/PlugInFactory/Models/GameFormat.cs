namespace Decksteria.Services.PlugInFactory.Models;

using Decksteria.Core;

public record GameFormat(string GameName, IDecksteriaGame Game, IDecksteriaFormat Format);