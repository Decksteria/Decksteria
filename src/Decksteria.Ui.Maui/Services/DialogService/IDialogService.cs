namespace Decksteria.Ui.Maui.Services.DialogService;

using Microsoft.Maui;
using System.Threading.Tasks;

public interface IDialogService
{
    Task<bool> DisplayMessage(string title, string message, FlowDirection flowDirection = FlowDirection.MatchParent);

    Task<bool?> DisplayYesNo(string title, string message, FlowDirection flowDirection = FlowDirection.MatchParent);
}