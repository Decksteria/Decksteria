namespace Decksteria.Ui.Maui.Services.PageService;

using Microsoft.Maui.Controls;
using System.Threading.Tasks;

public interface IPageService
{
    Task BackToHomeAsync(Page currentPage);
    Task OpenPageAsync(Page newPage, Page? currentPage = null);
}