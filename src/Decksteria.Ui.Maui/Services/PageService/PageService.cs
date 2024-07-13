namespace Decksteria.Ui.Maui.Services.PageService;

using System;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Ui.Maui.Pages.LoadPlugIn;
using Decksteria.Ui.Maui.Shared.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

internal sealed class PageService : IPageService
{
    private readonly Lazy<AppShell> appShell;

    private readonly IServiceProvider services;

    public PageService(Lazy<AppShell> appShell, IServiceProvider services)
    {
        this.appShell = appShell;
        this.services = services;
    }

    public async Task BackToHomeAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await AppShellNavigation.PopToRootAsync(true);
    }

    public ContentPage CreateHomePageInstance()
    {
        return GetPageInstance<LoadPlugIn>();
    }

    public async Task OpenFormPage<T>(Func<T, CancellationToken, Task> OnPopAsync, T? newPage = null, CancellationToken cancellationToken = default) where T : Page, IFormPage<T>
    {
        cancellationToken.ThrowIfCancellationRequested();

        var page = newPage ?? GetPageInstance<T>();
        page.OnPopAsync = OnPopAsync;
        await AppShellNavigation.PushModalAsync(new NavigationPage(page), true);
    }

    public async Task OpenFormPage<T>(Func<T, CancellationToken, Task> OnSubmitAsync, Func<T, CancellationToken, Task> OnPopAsync, T? newPage = null, CancellationToken cancellationToken = default) where T : Page, IActionFormPage<T>
    {
        cancellationToken.ThrowIfCancellationRequested();

        var page = newPage ?? GetPageInstance<T>();
        page.OnPopAsync = OnPopAsync;
        page.OnSubmitAsync = OnSubmitAsync;
        await AppShellNavigation.PushModalAsync(new NavigationPage(page), true);
    }

    public async Task OpenModalPage<T>(Func<T, CancellationToken, Task> onPopAsync, T? newPage = null, CancellationToken cancellationToken = default) where T : Page, IFormPage<T>
    {
        cancellationToken.ThrowIfCancellationRequested();

        var page = newPage ?? GetPageInstance<T>();
        page.OnPopAsync = onPopAsync;
        await AppShellNavigation.PushModalAsync(page, true);
    }

    public async Task OpenPageAsync<T>(T? newPage = null, CancellationToken cancellationToken = default) where T : ContentPage
    {
        cancellationToken.ThrowIfCancellationRequested();

        var page = newPage ?? GetPageInstance<T>();
        await AppShellNavigation.PushAsync(page, true);
    }

    public async Task<T?> PopModalAsync<T>(T? newPage = null, CancellationToken cancellationToken = default) where T : Page
    {
        cancellationToken.ThrowIfCancellationRequested();

        var poppedPage = await AppShellNavigation.PopModalAsync(true) as T;

        if (poppedPage is IFormPage<T> modalPage && modalPage.OnPopAsync is not null)
        {
            await modalPage.OnPopAsync(poppedPage, cancellationToken);
        }

        if (poppedPage is IActionFormPage<T> formPage && formPage.OnSubmitAsync is not null && formPage.IsSubmitted)
        {
            await formPage.OnSubmitAsync(poppedPage, cancellationToken);
        }

        return poppedPage;
    }

    private INavigation AppShellNavigation => appShell.Value.Navigation;

    private T GetPageInstance<T>() where T : Page
    {
        var instance = ActivatorUtilities.GetServiceOrCreateInstance<T>(services);
        var page = instance as T;
        return page!;
    }
}