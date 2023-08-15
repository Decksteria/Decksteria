namespace Decksteria.Service.DecksteriaFile.Models;

using Decksteria.Core;
using Decksteria.Core.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
internal partial class DeckFileMapper
{
    private readonly IDecksteriaFormat selectedFormat;

    private readonly FileCardMapper fileCardMapper;

    public DeckFileMapper(IDecksteriaFormat SelectedFormat, FileCardMapper fileCardMapper)
    {
        selectedFormat = SelectedFormat;
        this.fileCardMapper = fileCardMapper;
    }

    public partial DeckFile ToDeckFile(Decklist decklist);

    public async Task<Decklist> ToDecklistAsync(DeckFile decksteriaFile)
        => new Decklist(decksteriaFile.Game, decksteriaFile.Format, await CreateDecklistDecksAsync(decksteriaFile.Decks));

    private IReadOnlyDictionary<string, IEnumerable<FileCard>> CreateDecksteriaFileDecks(IReadOnlyDictionary<IDecksteriaDeck, IEnumerable<CardArt>> decks) => 
        decks.ToDictionary(kv => kv.Key.Name, kv => kv.Value.Select(fileCardMapper.ToFileCard));

    private async Task<IReadOnlyDictionary<string, IEnumerable<CardArt>>> CreateDecklistDecksAsync(IReadOnlyDictionary<string, IEnumerable<FileCard>> decks)
    {
        var mappedDecks = new Dictionary<string, IEnumerable<CardArt>>();
        foreach (var decklist in decks)
        {
            var tasks = decklist.Value.Select(fileCardMapper.ToCardArtAsync);
            var list = await Task.WhenAll(tasks);
            mappedDecks.Add(decklist.Key, list);
        }

        return mappedDecks;
    }
}
