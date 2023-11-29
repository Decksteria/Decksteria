namespace Decksteria.Ui.Maui.Shared.Models;

using System.Collections.Generic;
using System.Linq;

public record PlugInTile(PlugInDetails PlugIn)
{
    public string Name { get; init; } = PlugIn.Name;

    public string DisplayName { get; init; } = PlugIn.DisplayName;

    public byte[]? IconImg { get; init; } = PlugIn.IconImg;

    public string? IconSrc { get; init; } = PlugIn.IconSrc;

    public IEnumerable<FormatTile> Formats { get; init; } = PlugIn.Formats.Select(format => new FormatTile(PlugIn.Name, format));
}
