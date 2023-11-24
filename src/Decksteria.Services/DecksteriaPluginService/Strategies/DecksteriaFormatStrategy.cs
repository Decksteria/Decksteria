namespace Decksteria.Service.DecksteriaPluginService.Strategies;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core;
using Decksteria.Core.Models;

internal sealed class DecksteriaFormatStrategy : IDecksteriaFormatStrategy
{
    private IDecksteriaFormat? selectedFormat;

    public string Name => SelectedFormat.Name;

    public string DisplayName => SelectedFormat.DisplayName;

    public byte[]? Icon => SelectedFormat.Icon;

    public string Description => SelectedFormat.Description;

    public IEnumerable<IDecksteriaDeck> Decks => SelectedFormat.Decks;

    public IEnumerable<SearchField> SearchFields => SelectedFormat.SearchFields;

    public void ChangeFormat(IDecksteriaFormat? newFormat) => selectedFormat = newFormat;

    public Task<bool> CheckCardCountAsync(long cardId, IReadOnlyDictionary<string, IEnumerable<long>> decklist, CancellationToken cancellationToken = default)
        => SelectedFormat.CheckCardCountAsync(cardId, decklist, cancellationToken);

    public Task<int> CompareCardsAsync(long cardId1, long cardId2, CancellationToken cancellationToken = default) => SelectedFormat.CompareCardsAsync(cardId1, cardId2, cancellationToken);

    public Task<IDecksteriaCard> GetCardAsync(long cardId, CancellationToken cancellationToken = default) => SelectedFormat.GetCardAsync(cardId, cancellationToken);

    public Task<IEnumerable<IDecksteriaCard>> GetCardsAsync(IEnumerable<SearchFieldFilter>? filters = null, CancellationToken cancellationToken = default)
        => SelectedFormat.GetCardsAsync(filters, cancellationToken);

    public IDecksteriaDeck? GetDeckFromName(string name) => SelectedFormat.GetDeckFromName(name);

    public Task<Dictionary<string, int>> GetDeckStatsAsync(IReadOnlyDictionary<string, IEnumerable<long>> decklist, bool isDetailed, CancellationToken cancellationToken = default)
        => SelectedFormat.GetDeckStatsAsync(decklist, isDetailed, cancellationToken);

    public Task<IDecksteriaDeck> GetDefaultDeckAsync(long cardId, CancellationToken cancellationToken = default)
        => SelectedFormat.GetDefaultDeckAsync(cardId, cancellationToken);

    public Task<bool> IsDecklistLegal(IReadOnlyDictionary<string, IEnumerable<long>> decklist, CancellationToken cancellationToken = default)
        => SelectedFormat.IsDecklistLegal(decklist, cancellationToken);

    private IDecksteriaFormat SelectedFormat => selectedFormat ?? throw new NotImplementedException("Game Format has not been selected.");
}
