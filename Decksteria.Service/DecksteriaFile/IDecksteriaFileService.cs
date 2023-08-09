namespace Decksteria.Service.DecksteriaFile;

using Decksteria.Core.Models;
using System.IO;

public interface IDecksteriaFileService
{
    Task<Decklist> LoadDecksteriaFilterAsync(MemoryStream memoryStream);

    MemoryStream ReadDecksteriaFilter(Decklist decklist);
}