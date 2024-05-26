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

    public async Task OpenModalAsync<T>(CancellationToken cancellationToken = default) where T : Page
    {
        cancellationToken.ThrowIfCancellationRequested();

        var newPage = GetPageInstance<T>();
        await AppShellNavigation.PushModalAsync(new NavigationPage(newPage), true);
    }

    public async Task OpenModalAsync<T>(Func<T, CancellationToken, Task>? OnPopAsync = null, CancellationToken cancellationToken = default) where T : Page, IModalPage<T>
    {
        cancellationToken.ThrowIfCancellationRequested();

        var newPage = GetPageInstance<T>();
        newPage.OnPopAsync = OnPopAsync;
        await AppShellNavigation.PushModalAsync(new NavigationPage(newPage), true);
    }

    public async Task OpenModalAsync<T>(Func<T, CancellationToken, Task>? OnSubmitAsync = null, Func<T, CancellationToken, Task>? OnPopAsync = null, CancellationToken cancellationToken = default) where T : Page, IFormModalPage<T>
    {
        cancellationToken.ThrowIfCancellationRequested();

        var newPage = GetPageInstance<T>();
        newPage.OnPopAsync = OnPopAsync;
        newPage.OnSubmitAsync = OnSubmitAsync;
        await AppShellNavigation.PushModalAsync(new NavigationPage(newPage), true);
    }

    public async Task OpenPageAsync<T>(CancellationToken cancellationToken = default) where T : ContentPage
    {
        cancellationToken.ThrowIfCancellationRequested();

        var newPage = GetPageInstance<T>();
        await AppShellNavigation.PushAsync(newPage, true);
    }

    public async Task<T?> PopModalAsync<T>(CancellationToken cancellationToken = default) where T : Page
    {
        cancellationToken.ThrowIfCancellationRequested();

        var poppedPage = await AppShellNavigation.PopModalAsync(true) as T;

        if (poppedPage is IModalPage<T> modalPage && modalPage.OnPopAsync is not null)
        {
            await modalPage.OnPopAsync(poppedPage, cancellationToken);
        }

        if (poppedPage is IFormModalPage<T> formPage && formPage.OnSubmitAsync is not null && formPage.IsSubmitted)
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