namespace Decksteria.Services.UnitTests.Deckbuilding.DefaultImplementation;

using Decksteria.Core;
using Decksteria.Core.Data;

internal sealed record DefaultDecksteriaCardArt(long ArtId = 0) : IDecksteriaCardArt
{
    public long ArtId { get; } = ArtId;

    public DecksteriaImage Image { get; } = new(string.Empty, string.Empty);

}