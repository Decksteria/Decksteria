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

    Task OpenModalAsync<T>(CancellationToken cancellationToken = default) where T : Page;

    Task OpenModalAsync<T>(Func<T, CancellationToken, Task> OnPopAsync, CancellationToken cancellationToken = default) where T : Page, IModalPage<T>;

    Task OpenModalAsync<T>(Func<T, CancellationToken, Task> OnSubmitAsync, Func<T, CancellationToken, Task> OnPopAsync, CancellationToken cancellationToken = default) where T : Page, IFormModalPage<T>;

    Task OpenPageAsync<T>(CancellationToken cancellationToken = default) where T : ContentPage;

    Task<T?> PopModalAsync<T>(CancellationToken cancellationToken = default) where T : Page;
}