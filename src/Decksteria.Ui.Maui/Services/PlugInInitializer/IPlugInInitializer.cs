namespace Decksteria.Ui.Maui.Services.PlugInInitializer;

using Decksteria.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPlugInInitializer
{
    Task<IEnumerable<IDecksteriaGame>> GetOrInitializeAllPlugInsAsync();

    Task<IEnumerable<IDecksteriaGame>> InitializePlugInsAsync();

    IDecksteriaGame? TryGetNewPlugIn(string file);
}