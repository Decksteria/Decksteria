namespace Decksteria.Core.Models;

[Serializable]
public record CardArt(long CardId, long ArtId, byte[] Image, string Details)
{
    public CardArt(IDecksteriaCard decksteriaCard, long artId)
        : this(decksteriaCard.CardId, artId, decksteriaCard.Arts.First(art => art.ArtId == artId).Image, decksteriaCard.Details)
    {

    }
}
