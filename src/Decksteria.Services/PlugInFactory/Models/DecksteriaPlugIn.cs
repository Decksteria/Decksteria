namespace Decksteria.Services.PlugInFactory.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using Decksteria.Core;
using Decksteria.Ui.Maui.Shared.Models;
using Microsoft.Extensions.DependencyInjection;

public sealed class DecksteriaPlugIn
{
    public DecksteriaPlugIn(IServiceProvider serviceProvider, Type pluginType)
    {
        if (!typeof(IDecksteriaGame).IsAssignableFrom(pluginType))
        {
            throw new InvalidCastException("PlugInType is not a Decksteria Plug-In.");
        }

        PlugInType = pluginType;
        var initialLoad = CreateInstance(serviceProvider);
        Name = initialLoad.Name;
        Label = initialLoad.DisplayName;
        Icon = initialLoad.Icon;
        Formats = initialLoad.Formats.Select(format => new FormatDetails(format));
    }

    public string Name { get; init; }

    public string Label { get; init; }

    public byte[]? Icon { get; init; }

    public IEnumerable<FormatDetails> Formats { get; init; }

    public Type PlugInType { get; init; }

    public IDecksteriaGame CreateInstance(IServiceProvider serviceProvider)
    {
        var plugInInstance = ActivatorUtilities.CreateInstance(serviceProvider, PlugInType) as IDecksteriaGame;
        return plugInInstance!;
    }
}
