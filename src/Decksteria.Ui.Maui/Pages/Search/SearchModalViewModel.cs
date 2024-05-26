namespace Decksteria.Ui.Maui.Pages.Search;

using System.Collections.Generic;
using Decksteria.Ui.Maui.Pages.Search.Model;

internal sealed class SearchModalViewModel
{
    public IEnumerable<IMauiSearchFilter> SearchFieldFilters { get; internal set; } = new List<IMauiSearchFilter>();

    public List<string> TestOptions =
    [
        "Stuff",
        "Stuff1",
        "Stuff2",
        "Stuff3",
        "Stuff4"
    ];
}
