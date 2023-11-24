namespace Decksteria.Ui.Maui.Services.PlugInInitializer;

using Decksteria.Core;
using Decksteria.Core.Data;
using Decksteria.Ui.Maui.Services.DialogService;
using Decksteria.Ui.Maui.Services.FileReader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

internal sealed class PlugInInitializer : IPlugInInitializer
{
    private readonly IServiceProvider serviceProvider;

    public Dictionary<string, IDecksteriaGame>? GameList { get; private set; }

    public PlugInInitializer()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<IDecksteriaFileReader, DecksteriaFileReader>();
        serviceCollection.AddScoped<IDialogService, DialogService>();
        serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public async Task<IEnumerable<IDecksteriaGame>> GetOrInitializeAllPlugInsAsync()
    {
        GameList ??= (await InitializePlugInsAsync()).ToDictionary(plugin => plugin.Name);

        return GameList.Values;
    }

    public async Task<IEnumerable<IDecksteriaGame>> InitializePlugInsAsync()
    {
        var dllFiles = Directory.GetFiles(FileSystem.AppDataDirectory, "*.dll", SearchOption.TopDirectoryOnly);
        var plugIns = new ConcurrentQueue<IDecksteriaGame>();
        var tasks = dllFiles.Select(Assembly.LoadFile)
            .Select(AddToPlugInQueue);
        await Task.WhenAll(tasks);
        return plugIns;

        async Task AddToPlugInQueue(Assembly assembly)
        {
            var plugIn = await GetPlugInInterface(assembly);
            if (plugIn != null)
            {
                plugIns.Enqueue(plugIn);
            }
        }

        Task<IDecksteriaGame?> GetPlugInInterface(Assembly assembly)
        {
            var types = assembly.GetTypes();
            var plugInType = types.FirstOrDefault(t => typeof(IDecksteriaGame).IsAssignableFrom(t));
            if (plugInType == null)
            {
                return Task.FromResult<IDecksteriaGame?>(null);
            }

            var plugIn = ActivatorUtilities.CreateInstance(serviceProvider, plugInType) as IDecksteriaGame;
            return Task.FromResult(plugIn);
        }
    }

    public IDecksteriaGame? TryGetNewPlugIn(string file)
    {
        var assembly = Assembly.LoadFile(file);
        var types = assembly.GetTypes();
        var plugInType = types.FirstOrDefault(t => typeof(IDecksteriaGame).IsAssignableFrom(t));

        if (plugInType == null)
        {
            return null;
        }

        try
        {
            var plugIn = ActivatorUtilities.CreateInstance(serviceProvider, plugInType) as IDecksteriaGame;

            if (GameList != null && plugIn != null)
            {
                GameList[plugIn.Name] = plugIn;

                // Load into App Data
                var fileName = Path.GetFileName(file);
                File.Copy(file, $"{FileSystem.AppDataDirectory}/{fileName}");
            }

            return plugIn;
        }
        catch
        {
            return null;
        }
    }
}
