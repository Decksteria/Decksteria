namespace Decksteria.Ui.Maui.Pages.Search.Model;

using Decksteria.Core.Models;

internal interface ISearchFilter
{
    SearchFieldFilter[] AsSearchFieldFilterArray();
}