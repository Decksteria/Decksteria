namespace Decksteria.Ui.Maui.Services.PlugInFactory;

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
    private readonly IServiceProvider plugInSeviceProvider;

    private FormatDetails? formatDetails;

    private DecksteriaPlugIn? selectedPlugIn;

    public DecksteriaPlugInFactory(IDecksteriaFileReader fileReader, IDialogService dialogService)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(fileReader);
        serviceCollection.AddSingleton(dialogService);
        plugInSeviceProvider = serviceCollection.BuildServiceProvider();
    }

    public Dictionary<string, DecksteriaPlugIn>? GameList { get; private set; }

    private IEnumerable<DecksteriaPlugIn> GetDecksteriaPlugIns()
    {
        var dllFiles = Directory.GetFiles(FileSystem.AppDataDirectory, "*.dll", SearchOption.TopDirectoryOnly);
        var plugInTypes = dllFiles.Select(Assembly.LoadFile).Select(GetPlugInInterface);
        return plugInTypes.Where(plugin => plugin is not null).Select(type => new DecksteriaPlugIn(plugInSeviceProvider, type!));
    }


    public IEnumerable<DecksteriaPlugIn> GetOrInitializePlugIns()
    {
        GameList ??= GetDecksteriaPlugIns().ToDictionary(plugin => plugin.Name);
        return GameList.Values;
    }

    public GameFormat GetSelectedFormat()
    {
        var game = selectedPlugIn?.CreateInstance(plugInSeviceProvider) ?? throw new ArgumentNullException("Valid Plug-In has not been selected.");
        var format = game.Formats.FirstOrDefault(format => format.Name == formatDetails?.Name) ?? throw new ArgumentNullException("Valid Format has not been selected.");
        return new(game, format);
    }

    public void SelectGame(string gameName, string formatName)
    {
        selectedPlugIn = GameList?.GetValueOrDefault(gameName);
        formatDetails = selectedPlugIn?.Formats.FirstOrDefault(format => format.Name == formatName);
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
        var plugIn = new DecksteriaPlugIn(plugInSeviceProvider, plugInType);
        GameList.Add(plugIn.Name, plugIn);

        var fileName = Path.GetFileName(dllFilePath);
        File.Copy(dllFilePath, $"{FileSystem.AppDataDirectory}/{fileName}", true);
        return true;
    }

    private static Type? GetPlugInInterface(Assembly assembly)
    {
        var types = assembly.GetTypes();
        var plugInType = types.FirstOrDefault(t => typeof(IDecksteriaGame).IsAssignableFrom(t));
        return plugInType;
    }
}
