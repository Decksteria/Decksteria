namespace Decksteria.Ui.Maui.Services.DialogService;

using System.Threading.Tasks;
using Microsoft.Maui;

public interface IDialogService
{
    public Task<bool> DisplayMessage(string title, string message, FlowDirection flowDirection = FlowDirection.MatchParent);

    public Task<bool?> DisplayYesNo(string title, string message, FlowDirection flowDirection = FlowDirection.MatchParent);
}