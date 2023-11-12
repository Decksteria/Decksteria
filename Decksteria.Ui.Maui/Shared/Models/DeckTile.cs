namespace Decksteria.Ui.Maui.Shared.Models;

using Decksteria.Core;
using Decksteria.Core.Models;

public readonly struct DeckTile
{
    public DeckTile(string filePath)
    {
        FileLocation = filePath;
        FileName = Path.GetFileName(filePath);
        DeckName = Path.GetFileNameWithoutExtension(filePath);
    }

    public string FileLocation { get; init; }

    public string FileName { get; init; }

    public string DeckName { get; init; }
}