namespace Decksteria.Ui.Maui.Shared.Pages;

using Microsoft.Maui.Controls;

public interface IActionFormPage<T> where T : Page
{
    public bool IsSubmitted { get; }
}
