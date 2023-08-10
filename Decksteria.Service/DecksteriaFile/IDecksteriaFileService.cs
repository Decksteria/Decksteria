namespace Decksteria.Service.DecksteriaFile;

using Decksteria.Core.Models;
using System.IO;

public interface IDecksteriaFileService
{
    Task<Decklist> LoadDecksteriaFileAsync(Stream stream);

    Task SaveDecksteriaFileAsync(Decklist decklist, Stream stream);
}