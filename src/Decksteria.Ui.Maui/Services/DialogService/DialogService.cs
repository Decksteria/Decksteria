namespace Decksteria.Ui.Maui.Services.DialogService;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

internal sealed class DialogService : IDialogService
{
    public async Task<bool> DisplayMessage(string title, string message, FlowDirection flowDirection = FlowDirection.MatchParent)
    {
        if (MainPage == null)
        {
            return false;
        }

        await MainPage.DisplayAlert(title, message, "OK", flowDirection);
        return true;
    }

    public async Task<bool?> DisplayYesNo(string title, string message, FlowDirection flowDirection = FlowDirection.MatchParent)
    {
        return MainPage is not null ? await global::Decksteria.Ui.Maui.Services.DialogService.DialogService.MainPage.DisplayAlert(title, message, "Yes", "No", flowDirection) : null;
    }

    private static Page? MainPage => Application.Current?.Windows.FirstOrDefault()?.Page;
}
