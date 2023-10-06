namespace Decksteria.Ui.Maui.Shared.Models;

using Decksteria.Core;

internal sealed class FormatTile
{
    public FormatTile(IDecksteriaFormat format)
    {
        Name = format.Name;
        DisplayName = format.DisplayName;

        if (format.Icon != null)
        {
            var imgString = Convert.ToBase64String(format.Icon);
            IconSrc = $"data:image/png;base64,{imgString}";
        }
    }

    public string Name { get; }

    public string DisplayName { get; }

    public string? IconSrc { get; }
}