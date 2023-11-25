namespace Decksteria.Services.DecksteriaFile.Models;

using Decksteria.Core;
using Decksteria.Service.DecksteriaFile.Models;

/// <summary>
/// Model used by the Decksteria Application to represent a Card added to the Deck.
/// </summary>
/// <param name="CardId">The <see cref="IDecksteriaCard.CardId"/> of the Card selected by the user.</param>
/// <param name="ArtId">The <see cref="IDecksteriaCardArt.ArtId"/> of the Art selected by the user.</param>
/// <param name="ImageUrl">The <see cref="IDecksteriaCardArt.DownloadUrl"/> to be used for downloading the Image File.</param>
/// <param name="FileName">The <see cref="IDecksteriaCardArt.FileName"/> to that the Image should be downloaded as.</param>
/// <param name="Details">The <see cref="IDecksteriaCard.Details"/> to be provided to the user viewing the card.</param>
public record CardArt(long CardId, long ArtId, string ImageUrl, string FileName, string Details) : CardArtId(CardId, ArtId);