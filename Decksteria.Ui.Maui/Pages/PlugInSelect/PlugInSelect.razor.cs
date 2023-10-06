namespace Decksteria.Ui.Maui.Pages.PlugInSelect;

using Decksteria.Core;
using Decksteria.Ui.Maui.Services.PlugInInitializer;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

public partial class PlugInSelect
{
    [Inject]
    protected IPlugInInitializer PlugInInitializer { get; set; } = default!;

    protected IEnumerable<IDecksteriaGame>? GameList;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        GameList = await PlugInInitializer.GetOrInitializeAllPlugInsAsync();
    }
}
