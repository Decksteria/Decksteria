namespace Decksteria.Core.Models;

/// <summary>
/// Represents a card that has been added to a <see cref="Decklist">.
/// </summary>
/// <param name="CardId">The unique identifier of the card added to the decklist.</param>
/// <param name="ArtId">The unique identifier of the specific card's artwork added to the decklist.</param>
public record CardArtId(long CardId, long ArtId);
