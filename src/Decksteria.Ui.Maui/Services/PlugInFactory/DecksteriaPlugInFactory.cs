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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;

internal sealed class DecksteriaPlugInFactory : IDecksteriaPlugInFactory
{
    private readonly IServiceProvider serviceProvider;

    private readonly IDialogService dialogService;

    private readonly ILogger<DecksteriaPlugInFactory> logger;

    private FormatDetails? formatDetails;

    private DecksteriaPlugIn? selectedPlugIn;

    public GameFormat? selectedGameFormat;

    public DecksteriaPlugInFactory(IServiceProvider serviceProvider, IDialogService dialogService, ILogger<DecksteriaPlugInFactory> logger)
    {
        this.serviceProvider = serviceProvider;
        this.dialogService = dialogService;
        this.logger = logger;
    }

    public Dictionary<string, DecksteriaPlugIn>? GameList { get; private set; }

    private IEnumerable<DecksteriaPlugIn> GetDecksteriaPlugIns()
    {
        var dllFiles = Directory.GetFiles(FileSystem.AppDataDirectory, "*.dll", SearchOption.TopDirectoryOnly);
        var plugInTypes = dllFiles.Select(GetPlugInInterface).Select(InitializeSelectedGame);
        return plugInTypes.Where(plugin => plugin is not null).Select(plugIn => new DecksteriaPlugIn(plugIn!));
    }

    public IEnumerable<DecksteriaPlugIn> GetOrInitializePlugIns()
    {
        GameList ??= GetDecksteriaPlugIns().ToDictionary(plugin => plugin.Name);
        return GameList.Values;
    }

    public GameFormat GetSelectedFormat()
    {
        if (selectedGameFormat is not null)
        {
            return selectedGameFormat;
        }

        IDecksteriaGame? initializedGame;
        if (selectedPlugIn is null || (initializedGame = InitializeSelectedGame(selectedPlugIn?.PlugInType)) is null)
        {
            throw new ArgumentNullException("A valid plug-in has not been selected.");
        }

        var format = initializedGame.Formats.FirstOrDefault(format => format.Name == formatDetails?.Name) ?? throw new ArgumentNullException("Valid format has not been selected.");
        selectedGameFormat = new(selectedPlugIn!.Name, initializedGame, format);
        return selectedGameFormat;
    }

    public void SelectGame(string gameName, string formatName)
    {
        selectedPlugIn = GameList?.GetValueOrDefault(gameName);
        formatDetails = selectedPlugIn?.Formats.FirstOrDefault(format => format.Name == formatName);
        selectedGameFormat = null;
    }

    public bool TryAddGame(string dllFilePath)
    {
        var plugInType = GetPlugInInterface(dllFilePath);

        if (plugInType is null)
        {
            return false;
        }

        GameList ??= [];
        var plugIn = new DecksteriaPlugIn(InitializeSelectedGameThrowIfNull(plugInType));
        GameList[plugIn.Name] = plugIn;

        var fileName = Path.GetFileName(dllFilePath);
        File.Copy(dllFilePath, $"{FileSystem.AppDataDirectory}/{fileName}", true);
        return true;
    }

    private Type? GetPlugInInterface(string filePath)
    {
        try
        {
            var assemblyBytes = File.ReadAllBytes(filePath);
            var assembly = Assembly.Load(assemblyBytes);
            var types = assembly.GetTypes();
            var plugInType = types.FirstOrDefault(t => typeof(IDecksteriaGame).IsAssignableFrom(t));
            return plugInType;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return null;
        }
    }

    private IDecksteriaGame? InitializeSelectedGame(Type? type)
    {
        if (type is null || !typeof(IDecksteriaGame).IsAssignableFrom(type))
        {
            return null;
        }

        var serviceCollection = new ServiceCollection();
        serviceCollection.TryAddSingleton(dialogService);
        serviceCollection.TryAddSingleton(serviceProvider.GetRequiredService<IDecksteriaFileReader>());
        serviceCollection.AddLogging();
        var sp = serviceCollection.BuildServiceProvider();

        try
        {
            var plugIn = ActivatorUtilities.GetServiceOrCreateInstance(sp, type);
            return plugIn as IDecksteriaGame;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return null;
        }
    }

    private IDecksteriaGame InitializeSelectedGameThrowIfNull(Type type)
    {
        return InitializeSelectedGame(type) ?? throw new TypeLoadException($"The type {type.FullName} does not inherit IDecksteriaGame.");
    }
}
