namespace Decksteria.Ui.Maui.Shared.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using Decksteria.Services.PlugInFactory.Models;

public record PlugInTile(DecksteriaPlugIn PlugIn)
{
    public string Name { get; init; } = PlugIn.Name;

    public string DisplayName { get; init; } = PlugIn.Label;

    public byte[]? IconImg { get; init; } = PlugIn.Icon;

    public string? IconSrc { get; init; } = PlugIn.Icon is not null ? $"data:image/png;base64,{Convert.ToBase64String(PlugIn.Icon)}" : null;

    public IEnumerable<FormatTile> Formats { get; init; } = PlugIn.Formats.Select(format => new FormatTile(PlugIn.Name, format));
}