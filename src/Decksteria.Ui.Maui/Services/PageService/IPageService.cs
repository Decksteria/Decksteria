namespace Decksteria.Ui.Maui.Services.PageService;

using System.Threading.Tasks;
using Microsoft.Maui.Controls;

public interface IPageService
{
    Task BackToHomeAsync();
    Task OpenPageAsync<T>() where T : Page;
}