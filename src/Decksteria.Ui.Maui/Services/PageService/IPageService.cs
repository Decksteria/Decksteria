namespace Decksteria.Ui.Maui.Services.PageService;

using System.Threading.Tasks;
using Microsoft.Maui.Controls;

public interface IPageService
{
    Task BackToHomeAsync(Page currentPage);

    Task OpenPageAsync(Page newPage, Page? currentPage = null);
}