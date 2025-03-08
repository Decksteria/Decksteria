namespace Decksteria.Ui.Maui.Pages.Search.Model;

using Decksteria.Core.Models;
using Microsoft.Maui.Controls;

internal interface IMauiSearchFilter
{
    public string Title { get; }

    public ISearchFieldFilter[] AsSearchFieldFilterArray();

    public VisualElement GetVisualElement();
}