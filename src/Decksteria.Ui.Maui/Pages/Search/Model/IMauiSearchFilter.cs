namespace Decksteria.Ui.Maui.Pages.Search.Model;

using Decksteria.Services.Deckbuilding.Models;
using Microsoft.Maui.Controls;

internal interface IMauiSearchFilter
{
    string Title { get; }

    SearchFieldFilter[] AsSearchFieldFilterArray();

    VisualElement GetVisualElement();
}