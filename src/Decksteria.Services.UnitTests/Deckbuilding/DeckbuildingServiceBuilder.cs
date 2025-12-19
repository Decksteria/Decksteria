namespace Decksteria.Services.UnitTests.Deckbuilding;

using Decksteria.Core;
using Decksteria.Services.Deckbuilding;
using Decksteria.Services.UnitTests.Deckbuilding.DefaultImplementation;
using NSubstitute;

internal sealed class DeckbuildingServiceBuilder
{
    private readonly DefaultDecksteriaGame decksteriaGame;

    public DeckbuildingServiceBuilder()
    {
        decksteriaGame = new DefaultDecksteriaGame();
        DecksteriaFormat = Substitute.For<IDecksteriaFormat>();
        DecksteriaDeck = Substitute.For<IDecksteriaDeck>();

        // Default and consistent mocking for the Default Deck.
        DecksteriaDeck.Name.Returns("Default");
        DecksteriaDeck.DisplayName.Returns("Default Deck");
        DecksteriaDeck.IsDeckValidAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(Task.FromResult(true));
        DecksteriaDeck.IsCardCanBeAddedAsync(Arg.Any<long>(), Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(Task.FromResult(false));

        // Default and consistent mocking for the Format.
        DecksteriaFormat.Name.Returns("TestFormat");
        DecksteriaFormat.DisplayName.Returns("Test Format");
        DecksteriaFormat.GetCardAsync(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns((id) =>
        {
            var card = new DefaultDecksteriaCard
            {
                CardId = id.Arg<long>(),
                Arts = [new DefaultDecksteriaCardArt()]
            };
            return Task.FromResult<IDecksteriaCard>(card);
        });
        DecksteriaFormat.GetDefaultDeckAsync(Arg.Any<long>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(DecksteriaDeck);
    }

    public DeckbuildingService<IDecksteriaFormat> Build()
    {
        DecksteriaFormat.Decks.Returns([DecksteriaDeck]);
        decksteriaGame.Formats = [DecksteriaFormat];
        return new DeckbuildingService<IDecksteriaFormat>(decksteriaGame, DecksteriaFormat);
    }

    public IDecksteriaFormat DecksteriaFormat { get; private set; }

    public IDecksteriaDeck DecksteriaDeck { get; }

    public string GameName => nameof(DefaultDecksteriaGame);
}