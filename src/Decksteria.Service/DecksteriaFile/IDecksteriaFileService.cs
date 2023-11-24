namespace Decksteria.Service.DecksteriaFile;

using Decksteria.Core.Models;
using System.IO;
using System.Threading.Tasks;

public interface IDecksteriaFileService
{
    Task<Decklist> LoadDecksteriaFileAsync(Stream stream);

    Task SaveDecksteriaFileAsync(Decklist decklist, Stream stream);
}