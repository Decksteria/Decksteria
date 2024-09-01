namespace Decksteria.Ui.Maui.Shared.Models;

public readonly struct DeckTile(string deckName, bool isValid)
{
    public string DeckName { get; init; } = deckName;

    public bool IsValid { get; init; } = isValid;
}