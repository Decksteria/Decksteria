namespace Decksteria.Ui.Maui.Pages.PlugInSelect;

using System.Collections.Generic;
using System.Threading.Tasks;
using Decksteria.Core;
using Decksteria.Services.PlugInFactory;
using Decksteria.Services.PlugInFactory.Models;
using Microsoft.AspNetCore.Components;

public partial class PlugInSelect
{
    [Inject]
    protected IDecksteriaPlugInFactory PlugInFactory { get; set; } = default!;

    protected IEnumerable<DecksteriaPlugIn>? GameList;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        GameList = PlugInFactory.GetOrInitializePlugIns();
    }
}
