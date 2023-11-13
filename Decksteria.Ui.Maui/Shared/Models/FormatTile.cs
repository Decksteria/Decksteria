namespace Decksteria.Ui.Maui.Shared.Models;

using Decksteria.Core;

public readonly struct FormatTile
{
    public FormatTile(string gameName, IDecksteriaFormat format)
    {
        Name = format.Name;
        DisplayName = format.DisplayName;
        DeckDirectory = $"{gameName}\\{format.Name}";

        if (format.Icon != null)
        {
            var imgString = Convert.ToBase64String(format.Icon);
            IconImg = format.Icon;
            IconSrc = $"data:image/png;base64,{imgString}";
        }
    }

    public string Name { get; init; }

    public string DisplayName { get; init; }

    public byte[]? IconImg { get; init; }

    public string? IconSrc { get; init; }

    public string DeckDirectory { get; init; }
}