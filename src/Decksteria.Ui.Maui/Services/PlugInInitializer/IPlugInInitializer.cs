namespace Decksteria.Ui.Maui.Services.PlugInInitializer;

using System.Collections.Generic;
using System.Threading.Tasks;
using Decksteria.Core;

public interface IPlugInInitializer
{
    Task<IEnumerable<IDecksteriaGame>> GetOrInitializeAllPlugInsAsync();

    Task<IEnumerable<IDecksteriaGame>> InitializePlugInsAsync();

    IDecksteriaGame? TryGetNewPlugIn(string file);
}