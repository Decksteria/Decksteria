namespace Decksteria.Services.UnitTests.Deckbuilding.DefaultImplementation;

using System.Collections.Generic;
using Decksteria.Core;


internal sealed record DefaultDecksteriaCard : IDecksteriaCard
{
    public required long CardId { get; init; }

    public IEnumerable<IDecksteriaCardArt> Arts { get; init; } = [new DefaultDecksteriaCardArt()];

    public string Name => CardId.ToString();

    public string Details => string.Empty;
}