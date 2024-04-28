namespace Decksteria.Ui.Maui.Shared.Models;

using System.IO;

public readonly struct DeckTile(string filePath)
{
    public string FileLocation { get; init; } = filePath;

    public string FileName { get; init; } = Path.GetFileName(filePath);

    public string DeckName { get; init; } = Path.GetFileNameWithoutExtension(filePath);
}