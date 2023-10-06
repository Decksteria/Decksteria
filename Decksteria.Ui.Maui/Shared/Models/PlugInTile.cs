namespace Decksteria.Ui.Maui.Shared.Models;

using Decksteria.Core;

internal sealed class PlugInTile
{
    public PlugInTile(IDecksteriaGame plugIn)
    {
        Name = plugIn.Name;
        DisplayName = plugIn.DisplayName;
        Formats = plugIn.Formats.Select(format => new FormatTile(format));

        if (plugIn.Icon != null)
        {
            var imgString = Convert.ToBase64String(plugIn.Icon);
            IconSrc = $"data:image/png;base64,{imgString}";
        }
    }

    public string Name { get; }

    public string DisplayName { get; }

    public string? IconSrc { get; }

    public IEnumerable<FormatTile> Formats { get; }
}
