namespace Decksteria.Core.Models;

[Serializable]
public record CardArt(long CardId, long ArtId, string ImageUrl, string FileName, string Details)
{
}
