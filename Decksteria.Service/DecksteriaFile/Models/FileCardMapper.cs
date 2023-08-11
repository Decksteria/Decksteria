namespace Decksteria.Service.DecksteriaFile.Models;

using Decksteria.Core;
using Decksteria.Core.Models;
using Riok.Mapperly.Abstractions;
using System.Threading.Tasks;

[Mapper]
internal partial class FileCardMapper
{
    private readonly IDecksteriaFormat format;

    public FileCardMapper(IDecksteriaFormat format)
    {
        this.format = format;
    }

    public partial FileCard ToFileCard(CardArt cardArt);

    public async Task<CardArt> ToCardArtAsync(FileCard fileCard)
    {
        var card = await format.GetCardAsync(fileCard.CardId);
        var art = card.Arts.First(art => art.ArtId == fileCard.ArtId);
        return new CardArt(fileCard.CardId, fileCard.ArtId, art.DownloadUrl, art.FileName, card.Details);
    }
}
