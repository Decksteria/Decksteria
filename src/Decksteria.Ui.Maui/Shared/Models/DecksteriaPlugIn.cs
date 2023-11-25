namespace Decksteria.Ui.Maui.Shared.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using Decksteria.Core;
using Microsoft.Extensions.DependencyInjection;

public sealed class DecksteriaPlugIn
{
    public DecksteriaPlugIn(IServiceProvider serviceProvider, Type pluginType)
    {
        if (!typeof(IDecksteriaGame).IsAssignableFrom(pluginType))
        {
            throw new InvalidCastException("plugInType is not a Decksteria Plug-In.");
        }

        var initialLoad = CreateInstance(serviceProvider);
        Name = initialLoad.Name;
        Label = initialLoad.DisplayName;
        Icon = initialLoad.Icon;
        Formats = initialLoad.Formats.Select(format => new FormatTile(initialLoad.Name, format));
        PlugInType = pluginType;
    }

    public string Name { get; init; }

    public string Label { get; init; }

    public byte[]? Icon { get; init; }

    public IEnumerable<FormatTile> Formats { get; init; }

    public Type PlugInType { get; init; }

    public IDecksteriaGame CreateInstance(IServiceProvider serviceProvider)
    {
        var plugInInstance = ActivatorUtilities.CreateInstance(serviceProvider, PlugInType) as IDecksteriaGame;
        return plugInInstance!;
    }
}
