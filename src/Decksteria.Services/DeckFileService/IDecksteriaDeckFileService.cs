namespace Decksteria.Services.DeckFileService;

using Decksteria.Core.Models;

public interface IDecksteriaDeckFileService
{
    string CreateDeckFileJson(Decklist decklist, bool deckIsValid);

    Decklist? ReadDeckFileJson(string deckFileJson);
}