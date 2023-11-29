namespace Decksteria.Ui.Maui.Shared.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using Decksteria.Core;
using Decksteria.Services.PlugInFactory.Models;

public readonly struct PlugInDetails
{
    public PlugInDetails(DecksteriaPlugIn plugIn)
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

    public IEnumerable<FormatDetails> Formats { get; init; }
}
