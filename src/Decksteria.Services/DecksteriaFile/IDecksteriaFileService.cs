namespace Decksteria.Service.DecksteriaFile;

using System.IO;
using System.Threading.Tasks;
using Decksteria.Core.Models;

public interface IDecksteriaFileService
{
    Task<Decklist> LoadDecksteriaFileAsync(Stream stream);

    Task SaveDecksteriaFileAsync(Decklist decklist, Stream stream);
}