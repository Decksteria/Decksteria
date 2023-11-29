namespace Decksteria.Services.PlugInFactory.Models;

using Decksteria.Core;

public record GameFormat(IDecksteriaGame Game, IDecksteriaFormat Format);