namespace Decksteria.Ui.Maui.Shared.Models;

using Decksteria.Core.Data;
using Decksteria.Services.DeckFileService.Models;

internal record CardTile(long CardId, long ArtId, string ImageUrl, string FileName, string Details) : CardArt(CardId, ArtId, new DecksteriaImage(FileName, ImageUrl), Details)
{
    public string LocalFileLocation => FileName;
}
