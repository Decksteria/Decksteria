namespace Decksteria.Services.PlugInFactory.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using Decksteria.Core;

public sealed class DecksteriaPlugIn
{
    public DecksteriaPlugIn(IDecksteriaGame decksteriaGame)
    {
        PlugInType = decksteriaGame.GetType();
        Name = PlugInType.Name;
        Label = decksteriaGame.DisplayName;
        Icon = decksteriaGame.Icon;
        Formats = decksteriaGame.Formats.Select(format => new FormatDetails(format));
    }

    public string Name { get; init; }

    public string Label { get; init; }

    public byte[]? Icon { get; init; }

    public IEnumerable<FormatDetails> Formats { get; init; }

    public Type PlugInType { get; init; }
}
