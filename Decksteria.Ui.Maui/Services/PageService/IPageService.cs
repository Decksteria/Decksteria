namespace Decksteria.Ui.Maui.Services.PageService;

using Microsoft.Maui.Controls;
using System.Threading.Tasks;

public interface IPageService
{
    Page CurrentPage { get; }

    Page HomePage { get; init; }

    Task<Page> BackToHome();

    Task<Page> OpenPageAsync(Page newPage);
}