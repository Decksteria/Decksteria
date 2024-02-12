namespace Decksteria.Ui.Maui.Services.PageService;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

internal sealed class PageService(IServiceProvider services) : IPageService
{
    private readonly IServiceProvider services = services;

    public async Task BackToHomeAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (Application.Current?.MainPage is null)
        {
            return;
        }

        await Application.Current.MainPage.Navigation.PopToRootAsync();
    }

    public async Task OpenPageAsync<T>(CancellationToken cancellationToken = default) where T : Page
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (Application.Current is null)
        {
            return;
        }

        if (Application.Current.MainPage is null)
        {
            var newPage = GetPageInstance<T>();
            Application.Current.MainPage = new NavigationPage(newPage);
        }
        else
        {
            var newPage = GetPageInstance<T>();
            await Application.Current.MainPage.Navigation.PushAsync(newPage);
        }
    }

    private T GetPageInstance<T>() where T : Page
    {
        var instance = services.GetService(typeof(T));
        var page = instance as T;
        return page!;
    }
}