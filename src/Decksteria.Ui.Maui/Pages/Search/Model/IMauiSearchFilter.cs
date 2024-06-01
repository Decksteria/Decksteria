namespace Decksteria.Ui.Maui.Pages.Search.Model;

using Decksteria.Core.Models;
using Microsoft.Maui.Controls;

internal interface IMauiSearchFilter
{
    string Title { get; }

    ISearchFieldFilter[] AsSearchFieldFilterArray();

    VisualElement GetVisualElement();
}