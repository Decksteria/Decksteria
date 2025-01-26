namespace Decksteria.Services.DeckFileService.Models;

using Decksteria.Core;
using Decksteria.Core.Data;
using Decksteria.Core.Models;

/// <summary>
/// Model used by the Decksteria Application to represent a Card added to the Deck.
/// </summary>
/// <param name="CardId">The <see cref="IDecksteriaCard.CardId"/> of the Card selected by the user.</param>
/// <param name="ArtId">The <see cref="IDecksteriaCardArt.ArtId"/> of the Art selected by the user.</param>
/// <param name="Image">The <see cref="DecksteriaImage"/> used buy the application to retrieve the image.</param>
/// <param name="Details">The <see cref="IDecksteriaCard.Details"/> to be provided to the user viewing the card.</param>
public record CardArt(long CardId, long ArtId, DecksteriaImage Image, string Details) : CardArtId(CardId, ArtId), IDecksteriaCardArt;