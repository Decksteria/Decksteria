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

        await appShell.Value.Navigation.PopToRootAsync();
    }

    public ContentPage CreateHomePageInstance()
    {
        return GetPageInstance<LoadPlugIn>();
    }

    public async Task OpenPageAsync<T>(CancellationToken cancellationToken = default) where T : ContentPage
    {
        cancellationToken.ThrowIfCancellationRequested();

        var newPage = GetPageInstance<T>();
        await appShell.Value.Navigation.PushAsync(newPage);
    }

    private T GetPageInstance<T>() where T : ContentPage
    {
        var instance = ActivatorUtilities.GetServiceOrCreateInstance<T>(services);
        var page = instance as T;
        return page!;
    }
}