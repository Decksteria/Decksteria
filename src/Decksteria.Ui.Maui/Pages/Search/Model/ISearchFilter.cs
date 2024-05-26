namespace Decksteria.Ui.Maui.Pages.Search.Model;

using Decksteria.Services.Deckbuilding.Models;

internal interface ISearchFilter
{
    SearchFieldFilter[] AsSearchFieldFilterArray();
}