namespace Decksteria.Ui.Maui.Services.PageService;

using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

internal sealed class PageService(IServiceProvider services) : IPageService
{
    private readonly IServiceProvider services = services;

    public async Task OpenPageAsync<T>() where T : Page
    {
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

    public async Task BackToHomeAsync()
    {
        if (Application.Current?.MainPage is null)
        {
            return;
        }

        await Application.Current.MainPage.Navigation.PopToRootAsync();
    }

    private T GetPageInstance<T>() where T : Page => ( services.GetService(typeof(T)) as T )!;
}