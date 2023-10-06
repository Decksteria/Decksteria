namespace Decksteria.Ui.Maui.Services.DialogService;

using Microsoft.Maui;
using System.Threading.Tasks;

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
        if (MainPage == null)
        {
            return null;
        }

        return await MainPage.DisplayAlert(title, message, "Yes", "No", flowDirection);
    }

    private Page? MainPage => Application.Current?.MainPage;
}
