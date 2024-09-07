namespace Decksteria.Ui.Maui.Services.PageService;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Ui.Maui.Pages.LoadPlugIn;
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

    public async Task<T> OpenFormPage<T>(T? newPage = null, bool waitUntilDisappear = true, CancellationToken cancellationToken = default) where T : Page
    {
        cancellationToken.ThrowIfCancellationRequested();

        var taskCompletion = new TaskCompletionSource<bool>();
        var page = newPage ?? GetPageInstance<T>();
        var disappearAction = CreateOnModalDisappearAction(page, taskCompletion);
        page.Disappearing += (sender, e) => disappearAction(sender, e);
        await AppShellNavigation.PushModalAsync(page, true);

        // Wait for the page to have been closed.
        if (waitUntilDisappear)
        {
            await taskCompletion.Task;
        }

        return page;
    }

    public async Task<T> OpenModalPage<T>(T? newPage = null, bool waitUntilDisappear = true, CancellationToken cancellationToken = default) where T : Page
    {
        cancellationToken.ThrowIfCancellationRequested();

        var taskCompletion = new TaskCompletionSource<bool>();
        var page = newPage ?? GetPageInstance<T>();
        var disappearAction = CreateOnModalDisappearAction(page, taskCompletion);
        page.Disappearing += (sender, e) => disappearAction(sender, e);
        await AppShellNavigation.PushModalAsync(page, true);

        // Wait for the page to have been closed.
        if (waitUntilDisappear)
        {
            await taskCompletion.Task;
        }

        return page;
    }

    public async Task<T> OpenPageAsync<T>(T? newPage = null, bool waitUntilDisappear = true, CancellationToken cancellationToken = default) where T : ContentPage
    {
        cancellationToken.ThrowIfCancellationRequested();

        var taskCompletion = new TaskCompletionSource<bool>();
        var page = newPage ?? GetPageInstance<T>();
        var disappearAction = CreateOnModalDisappearAction(page, taskCompletion);
        page.Disappearing += (sender, e) => disappearAction(sender, e);
        await AppShellNavigation.PushAsync(page, true);

        // Wait for the page to have been closed.
        if (waitUntilDisappear)
        {
            await taskCompletion.Task;
        }

        return page;
    }

    public async Task<T?> PopModalAsync<T>(CancellationToken cancellationToken = default) where T : Page
    {
        cancellationToken.ThrowIfCancellationRequested();
        var poppedPage = await AppShellNavigation.PopModalAsync(true) as T;
        return poppedPage;
    }

    private INavigation AppShellNavigation => appShell.Value.Navigation;

    private T GetPageInstance<T>() where T : Page
    {
        var instance = ActivatorUtilities.GetServiceOrCreateInstance<T>(services);
        var page = instance as T;
        return page!;
    }

    private Action<object?, EventArgs> CreateOnModalDisappearAction<T>(T page, TaskCompletionSource<bool> completionSource) where T : Page
    {
        return (sender, e) =>
        {
            if (AppShellNavigation.ModalStack.Contains(page))
            {
                return;
            }

            completionSource.TrySetResult(true);
        };
    }

    private Action<object?, EventArgs> CreateOnNavigationDisappearAction<T>(T page, TaskCompletionSource<bool> completionSource) where T : Page
    {
        return (sender, e) =>
        {
            if (AppShellNavigation.NavigationStack.Contains(page))
            {
                return;
            }

            completionSource.TrySetResult(true);
        };
    }
}