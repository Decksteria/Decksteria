namespace Decksteria.Ui.Maui.Pages.Search;

using System.Collections.Generic;
using Decksteria.Core.Models;

internal sealed class SearchModalViewModel
{
    public IEnumerable<SearchFieldFilter> SearchFieldFilters { get; internal set; } = new List<SearchFieldFilter>();

    public List<string> TestOptions =
    [
        "Stuff",
        "Stuff1",
        "Stuff2",
        "Stuff3",
        "Stuff4"
    ];
}
