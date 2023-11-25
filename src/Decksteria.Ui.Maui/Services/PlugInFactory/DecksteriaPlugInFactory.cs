namespace Decksteria.Ui.Maui.Services.PlugInFactory;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Decksteria.Core;
using Decksteria.Core.Data;
using Decksteria.Ui.Maui.Services.DialogService;
using Decksteria.Ui.Maui.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;

internal sealed class DecksteriaPlugInFactory : IDecksteriaPlugInFactory
{
    private readonly IServiceProvider serviceProvider;

    private DecksteriaPlugIn? plugInType;

    public DecksteriaPlugInFactory(IDecksteriaFileReader fileReader, IDialogService dialogService)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(fileReader);
        serviceCollection.AddSingleton(dialogService);
        serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public Dictionary<string, DecksteriaPlugIn>? GameList { get; private set; }

    public IDecksteriaGame CreatePlugInInstance()
    {
        return plugInType?.CreateInstance(serviceProvider) ?? throw new ArgumentNullException("Plug-In has not been selected.");
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

    public void SelectGame(string gameName)
    {
        plugInType = GameList?.GetValueOrDefault(gameName);
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
