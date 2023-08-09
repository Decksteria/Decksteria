namespace Decksteria.Service.DecksteriaPluginService.Strategies;

using Decksteria.Core;
using Decksteria.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

internal sealed class DecksteriaFormatStrategy : IDecksteriaFormatStrategy
{
    private IDecksteriaFormat? selectedFormat;

    public void ChangeFormat(IDecksteriaFormat? newFormat)
    {
        selectedFormat = newFormat;
    }

    public string Name => CheckAndThrowIfNotSelected(selectedFormat?.Name);

    public string DisplayName => CheckAndThrowIfNotSelected(selectedFormat?.DisplayName);

    public byte[]? Icon => selectedFormat?.Icon;

    public string Description => CheckAndThrowIfNotSelected(selectedFormat?.Description);

    public IEnumerable<IDecksteriaDeck> Decks => CheckAndThrowIfNotSelected(selectedFormat?.Decks);

    public IEnumerable<SearchField> SearchFields => CheckAndThrowIfNotSelected(selectedFormat?.SearchFields);

    public int CompareCards(long cardId1, long cardId2) => CheckAndThrowIfNotSelected(selectedFormat?.CompareCards(cardId1, cardId2));

    public Task<IEnumerable<IDecksteriaCard<IDecksteriaCardArt>>> GetCardsAsync(IEnumerable<SearchField>? filters = null)
        => CheckAndThrowIfNotSelected(selectedFormat?.GetCardsAsync(filters));

    public IDecksteriaDeck GetDeckFromName(string name) => CheckAndThrowIfNotSelected(selectedFormat?.GetDeckFromName(name));

    public Dictionary<string, int> GetDeckStats(IDictionary<IDecksteriaDeck, IEnumerable<long>> decklist, bool isDetailed)
        => CheckAndThrowIfNotSelected(selectedFormat?.GetDeckStats(decklist, isDetailed));

    public IDecksteriaDeck GetDefaultDeck(long cardId) => CheckAndThrowIfNotSelected(selectedFormat?.GetDefaultDeck(cardId));

    private T CheckAndThrowIfNotSelected<T>(T? value)
    {
        return value ?? throw new NotImplementedException("Game Format has not been selected.");
    }

    private int CheckAndThrowIfNotSelected(int? value)
    {
        return value ?? throw new NotImplementedException("Game Format has not been selected.");
    }
}
