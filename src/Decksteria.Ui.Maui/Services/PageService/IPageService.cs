namespace Decksteria.Ui.Maui.Services.PageService;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

public interface IPageService
{
    public Task BackToHomeAsync(CancellationToken cancellationToken = default);

    public ContentPage CreateHomePageInstance();
    public T CreatePageInstance<T>(params object[] parameters) where T : Page;
    public Task<T> OpenFormPage<T>(T? newPage = null, bool waitUntilDisappear = true, CancellationToken cancellationToken = default) where T : Page;

    public Task<T> OpenModalPage<T>(T? newPage = null, bool waitUntilDisappear = true, CancellationToken cancellationToken = default) where T : Page;

    public Task<T> OpenPageAsync<T>(T? newPage = null, bool waitUntilDisappear = true, CancellationToken cancellationToken = default) where T : ContentPage;

    public Task<T?> PopModalAsync<T>(CancellationToken cancellationToken = default) where T : Page;
}