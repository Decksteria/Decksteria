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

    [ObjectFactory]
    private IDictionary<string, IEnumerable<FileCard>> CreateDecksteriaFileDecks(IDictionary<IDecksteriaDeck, IEnumerable<CardArt>> decks) => decks.ToDictionary(kv => kv.Key.Name, kv => kv.Value.Select(fileCardMapper.ToFileCard));

    private async Task<IDictionary<IDecksteriaDeck, IEnumerable<CardArt>>> CreateDecklistDecksAsync(IDictionary<string, IEnumerable<FileCard>> decks)
    {
        var tasks = decks.Select(SelectAsync);
        return (await Task.WhenAll(tasks)).ToDictionary(kv => kv.Key, kv => kv.Value);

        async Task<KeyValuePair<IDecksteriaDeck, IEnumerable<CardArt>>> SelectAsync(KeyValuePair<string, IEnumerable<FileCard>> deck)
        {
            var tasks = deck.Value.Select(fileCardMapper.ToCardArtAsync);
            return new KeyValuePair<IDecksteriaDeck, IEnumerable<CardArt>>(selectedFormat.GetDeckFromName(deck.Key), await Task.WhenAll(tasks));
        }
    }
}
