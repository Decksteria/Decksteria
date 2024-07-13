namespace Decksteria.Ui.Maui.Services.PageService;

using System;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Ui.Maui.Shared.Pages;
using Microsoft.Maui.Controls;

public interface IPageService
{
    Task BackToHomeAsync(CancellationToken cancellationToken = default);

    ContentPage CreateHomePageInstance();

    Task OpenFormPage<T>(Func<T, CancellationToken, Task> onPopAsync, T? newPage = null, CancellationToken cancellationToken = default) where T : Page, IFormPage<T>;

    Task OpenFormPage<T>(Func<T, CancellationToken, Task> onSubmitAsync, Func<T, CancellationToken, Task> onPopAsync, T? newPage = null, CancellationToken cancellationToken = default) where T : Page, IActionFormPage<T>;

    Task OpenModalPage<T>(Func<T, CancellationToken, Task> onPopAsync, T? newPage = null, CancellationToken cancellationToken = default) where T : Page, IFormPage<T>;

    Task OpenPageAsync<T>(T? newPage = null, CancellationToken cancellationToken = default) where T : ContentPage;

    Task<T?> PopModalAsync<T>(T? newPage = null, CancellationToken cancellationToken = default) where T : Page;
}