namespace Decksteria.Ui.Maui.Services.PageService;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

public interface IPageService
{
    Task BackToHomeAsync(CancellationToken cancellationToken = default);

    Task OpenPageAsync<T>(CancellationToken cancellationToken = default) where T : Page;
}