namespace Decksteria.Ui.Maui.Shared.Models;

using Decksteria.Services.FileService.Models;

internal record CardTile(long CardId, long ArtId, string ImageUrl, string FileName, string Details) : CardArt(CardId, ArtId, ImageUrl, FileName, Details)
{
    public string LocalFileLocation => FileName;
}
