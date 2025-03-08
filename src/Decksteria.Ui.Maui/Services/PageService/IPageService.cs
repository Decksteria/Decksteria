namespace Decksteria.Ui.Maui.Services.PageService;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

public interface IPageService
{
    Task BackToHomeAsync(CancellationToken cancellationToken = default);

    ContentPage CreateHomePageInstance();
    T CreatePageInstance<T>(params object[] parameters) where T : Page;
    Task<T> OpenFormPage<T>(T? newPage = null, bool waitUntilDisappear = true, CancellationToken cancellationToken = default) where T : Page;

    Task<T> OpenModalPage<T>(T? newPage = null, bool waitUntilDisappear = true, CancellationToken cancellationToken = default) where T : Page;

    Task<T> OpenPageAsync<T>(T? newPage = null, bool waitUntilDisappear = true, CancellationToken cancellationToken = default) where T : ContentPage;

    Task<T?> PopModalAsync<T>(CancellationToken cancellationToken = default) where T : Page;
}