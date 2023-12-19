namespace Decksteria.Ui.Maui;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core;
using Decksteria.Core.Models;

// TODO: Remove this temporary class
internal sealed record FakeGame : IDecksteriaGame
{
    public IEnumerable<IDecksteriaFormat> Formats => [new TestFormat()];

    public IEnumerable<IDecksteriaImport> Importers => throw new System.NotImplementedException();

    public IEnumerable<IDecksteriaExport> Exporters => throw new System.NotImplementedException();

    public string Name => "superGame";

    public string DisplayName => "Fake Game";

    public byte[]? Icon => null;

    public string Description => throw new System.NotImplementedException();

    private sealed class TestFormat : IDecksteriaFormat
    {
        public IEnumerable<IDecksteriaDeck> Decks => [new DefaultDeck("firstDeck", "First Deck"), new DefaultDeck("secondDeck", "Second Deck") ];

        public IEnumerable<SearchField> SearchFields => [];

        public string Name => "superFormat";

        public string DisplayName => "Fake Format";

        public byte[]? Icon => null;

        public string Description => throw new System.NotImplementedException();

        public Task<bool> CheckCardCountAsync(long cardId, IReadOnlyDictionary<string, IEnumerable<long>> decklist, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> CompareCardsAsync(long cardId1, long cardId2, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IDecksteriaCard> GetCardAsync(long cardId, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<IDecksteriaCard>> GetCardsAsync(IEnumerable<SearchFieldFilter>? filters = null, CancellationToken cancellationToken = default)
        {
            var cardList = new IDecksteriaCard[]
            {
                new DefaultCard(1, "Shadow Wing", ["https://world.digimoncard.com/images/cardlist/card/ST1-13.png"]),
                new DefaultCard(2, "Not Shadow Wing", ["https://world.digimoncard.com/images/cardlist/card/ST1-14.png"]),
                new DefaultCard(3, "Another Card", ["https://world.digimoncard.com/images/cardlist/card/ST2-13.png"]),
            };
            return Task.FromResult<IEnumerable<IDecksteriaCard>>(cardList);
        }

        public Task<Dictionary<string, int>> GetDeckStatsAsync(IReadOnlyDictionary<string, IEnumerable<long>> decklist, bool isDetailed, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IDecksteriaDeck> GetDefaultDeckAsync(long cardId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Decks.First());
        }

        public Task<bool> IsDecklistLegal(IReadOnlyDictionary<string, IEnumerable<long>> decklist, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }
    }

    private sealed class DefaultDeck(string name, string displayName) : IDecksteriaDeck
    {
        public string Name { get; init; } = name;

        public string DisplayName { get; init; } = displayName;

        public Task<bool> IsCardCanBeAddedAsync(long cardId, IEnumerable<long> cards, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }

        public Task<bool> IsDeckValidAsync(IEnumerable<long> cards, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }
    }

    private sealed class DefaultCard(long id, string name, string[] imageUrls) : IDecksteriaCard
    {
        public long CardId => id;

        public IEnumerable<IDecksteriaCardArt> Arts => imageUrls.Select((value, index) => new DefaultArt(index, value));

        public string Name => name;

        public string Details => "No Details";
    }

    private sealed class DefaultArt(int id, string imageUrl) : IDecksteriaCardArt
    {
        public long ArtId => id;

        public string DownloadUrl => imageUrl;

        public string FileName => Path.GetFileName(imageUrl);
    }
}