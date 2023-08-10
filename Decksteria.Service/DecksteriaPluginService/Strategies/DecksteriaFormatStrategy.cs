namespace Decksteria.Service.DecksteriaPluginService.Strategies;

using Decksteria.Core;
using Decksteria.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

internal sealed class DecksteriaFormatStrategy : IDecksteriaFormatStrategy
{
    private IDecksteriaFormat? selectedFormat;

    public string Name => SelectedFormat.Name;

    public string DisplayName => SelectedFormat.DisplayName;

    public byte[]? Icon => SelectedFormat.Icon;

    public string Description => SelectedFormat.Description;

    public IEnumerable<IDecksteriaDeck> Decks => SelectedFormat.Decks;

    public IEnumerable<SearchField> SearchFields => SelectedFormat.SearchFields;

    public void ChangeFormat(IDecksteriaFormat? newFormat)
    {
        selectedFormat = newFormat;
    }

    public Task<bool> CheckCardCountAsync(long cardId, IReadOnlyDictionary<IDecksteriaDeck, IEnumerable<long>> decklist, CancellationToken cancellationToken = default)
        => SelectedFormat.CheckCardCountAsync(cardId, decklist, cancellationToken);

    public int CompareCards(long cardId1, long cardId2) => SelectedFormat.CompareCards(cardId1, cardId2);

    public Task<IDecksteriaCard> GetCardAsync(long cardId, CancellationToken? cancellationToken = null) => SelectedFormat.GetCardAsync(cardId, cancellationToken);

    public Task<IEnumerable<IDecksteriaCard>> GetCardsAsync(IEnumerable<SearchField>? filters = null, CancellationToken cancellationToken = default)
        => SelectedFormat.GetCardsAsync(filters, cancellationToken);

    public IDecksteriaDeck? GetDeckFromName(string name) => SelectedFormat.GetDeckFromName(name);

    public Task<Dictionary<string, int>> GetDeckStatsAsync(IReadOnlyDictionary<IDecksteriaDeck, IEnumerable<long>> decklist, bool isDetailed, CancellationToken cancellationToken = default)
        => SelectedFormat.GetDeckStatsAsync(decklist, isDetailed, cancellationToken);

    public Task<IDecksteriaDeck> GetDefaultDeckAsync(long cardId, CancellationToken cancellationToken = default)
        => SelectedFormat.GetDefaultDeckAsync(cardId, cancellationToken);

    private IDecksteriaFormat SelectedFormat => selectedFormat ?? throw new NotImplementedException("Game Format has not been selected.");
}
