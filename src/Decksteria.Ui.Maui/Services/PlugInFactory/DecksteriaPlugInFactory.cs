﻿namespace Decksteria.Ui.Maui.Services.PlugInFactory;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Decksteria.Core;
using Decksteria.Core.Data;
using Decksteria.Services.PlugInFactory;
using Decksteria.Services.PlugInFactory.Models;
using Decksteria.Ui.Maui.Services.DialogService;
using Decksteria.Ui.Maui.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;

internal sealed class DecksteriaPlugInFactory : IDecksteriaPlugInFactory
{
    private readonly IServiceProvider serviceProvider;

    private DecksteriaPlugIn? plugInType;

    private FormatDetails? formatDetails;

    public DecksteriaPlugInFactory(IDecksteriaFileReader fileReader, IDialogService dialogService)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(fileReader);
        serviceCollection.AddSingleton(dialogService);
        serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public Dictionary<string, DecksteriaPlugIn>? GameList { get; private set; }

    public GameFormat CreatePlugInInstance()
    {
        var game = plugInType?.CreateInstance(serviceProvider) ?? throw new ArgumentNullException("Valid Plug-In has not been selected.");
        var format = game.Formats.FirstOrDefault(format => format.Name == formatDetails?.Name) ?? throw new ArgumentNullException("Valid Format has not been selected.");
        return new(game, format);
    }

    public IEnumerable<DecksteriaPlugIn> GetOrInitializePlugIns()
    {
        GameList ??= GetDecksteriaPlugIns().ToDictionary(plugin => plugin.Name);
        return GameList.Values;
    }

    private IEnumerable<DecksteriaPlugIn> GetDecksteriaPlugIns()
    {
        var dllFiles = Directory.GetFiles(FileSystem.AppDataDirectory, "*.dll", SearchOption.TopDirectoryOnly);
        var plugInTypes = dllFiles.Select(Assembly.LoadFile).Select(GetPlugInInterface);
        return plugInTypes.Where(plugin => plugin is not null).Select(type => new DecksteriaPlugIn(serviceProvider, type!));
    }

    public void SelectGame(string gameName, string formatName)
    {
        plugInType = GameList?.GetValueOrDefault(gameName);
        formatDetails = plugInType?.Formats.FirstOrDefault(format => format.Name == formatName);
    }

    public bool TryAddGame(string dllFilePath)
    {
        var assemblyFile = Assembly.LoadFile(dllFilePath);
        var plugInType = GetPlugInInterface(assemblyFile);

        if (plugInType is null)
        {
            return false;
        }

        GameList ??= [];
        var plugIn = new DecksteriaPlugIn(serviceProvider, plugInType);
        GameList.Add(plugIn.Name, plugIn);
        return true;
    }

    private static Type? GetPlugInInterface(Assembly assembly)
    {
        var types = assembly.GetTypes();
        var plugInType = types.FirstOrDefault(t => typeof(IDecksteriaGame).IsAssignableFrom(t));
        return plugInType;
    }
}
