namespace Decksteria.Ui.Maui.Pages.Search.Model;

using Decksteria.Services.Deckbuilding.Models;

internal interface IMauiSearchFilter
{
    public string Title { get; }

    SearchFieldFilter[] AsSearchFieldFilterArray();
}