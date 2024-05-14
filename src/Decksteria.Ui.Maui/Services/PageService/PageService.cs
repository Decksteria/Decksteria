namespace Decksteria.Ui.Maui.Services.PageService;

using System;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Ui.Maui.Pages.LoadPlugIn;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

internal sealed class PageService(Lazy<AppShell> appShell, IServiceProvider services) : IPageService
{
    private readonly Lazy<AppShell> appShell = appShell;

    private readonly IServiceProvider services = services;

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

    public async Task OpenPageAsync<T>(CancellationToken cancellationToken = default) where T : ContentPage
    {
        cancellationToken.ThrowIfCancellationRequested();

        var newPage = GetPageInstance<T>();
        await AppShellNavigation.PushAsync(newPage, true);
    }

    public async Task<T?> PopModalAsync<T>(CancellationToken cancellationToken = default) where T : Page
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await AppShellNavigation.PopModalAsync(true) as T;
    }

    private INavigation AppShellNavigation => appShell.Value.Navigation;

    private T GetPageInstance<T>() where T : Page
    {
        var instance = ActivatorUtilities.GetServiceOrCreateInstance<T>(services);
        var page = instance as T;
        return page!;
    }
}