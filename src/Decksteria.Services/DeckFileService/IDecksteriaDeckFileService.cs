namespace Decksteria.Services.DeckFileService;

using Decksteria.Core.Models;

public interface IDecksteriaDeckFileService
{
    public string CreateDeckFileJson(Decklist decklist, bool deckIsValid);

    public Decklist? ReadDeckFileJson(string deckFileJson);
}