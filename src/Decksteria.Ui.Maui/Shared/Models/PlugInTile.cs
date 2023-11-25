namespace Decksteria.Ui.Maui.Shared.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using Decksteria.Core;

public readonly struct PlugInTile
{
    public PlugInTile(DecksteriaPlugIn plugIn)
    {
        Name = plugIn.Name;
        DisplayName = plugIn.Label;
        Formats = plugIn.Formats;

        if (plugIn.Icon != null)
        {
            var imgString = Convert.ToBase64String(plugIn.Icon);
            IconImg = plugIn.Icon;
            IconSrc = $"data:image/png;base64,{imgString}";
        }
    }

    public string Name { get; init; }

    public string DisplayName { get; init; }

    public byte[]? IconImg { get; init; }

    public string? IconSrc { get; init; }

    public IEnumerable<FormatTile> Formats { get; init; }
}
