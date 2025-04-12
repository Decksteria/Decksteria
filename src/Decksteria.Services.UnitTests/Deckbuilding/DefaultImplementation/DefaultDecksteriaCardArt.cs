namespace Decksteria.Services.UnitTests.Deckbuilding.DefaultImplementation;

using Decksteria.Core;
using Decksteria.Core.Data;

internal sealed record DefaultDecksteriaCardArt : IDecksteriaCardArt
{
    public long ArtId => 0;

    public DecksteriaImage Image { get;set; } = new(string.Empty, string.Empty);

}