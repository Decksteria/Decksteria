namespace Decksteria.Ui.Maui.Shared.Models;

public readonly struct DeckTile(string filePath)
{
    public string FileLocation { get; init; } = filePath;

    public string FileName { get; init; } = Path.GetFileName(filePath);

    public string DeckName { get; init; } = Path.GetFileNameWithoutExtension(filePath);
}