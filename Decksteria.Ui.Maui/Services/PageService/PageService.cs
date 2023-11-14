namespace Decksteria.Ui.Maui.Services.PageService;

using System;
using System.Threading.Tasks;

internal sealed class PageService : IPageService
{
    private readonly Application application;

    private readonly Window homeWindow;

    public PageService(Application application, LoadPlugIn plugInSelectPage)
    {
        this.application = application;
        homeWindow = application.Windows[0];
        HomePage = IsMobile ? new NavigationPage(plugInSelectPage) : plugInSelectPage;
        CurrentPage = HomePage;
    }

    public async Task<Page> OpenPageAsync(Page newPage)
    {
        if (IsMobile)
        {
            await CurrentPage!.Navigation.PushAsync(newPage);
            CurrentPage = newPage;
            return newPage;
        }

        application.OpenWindow(new Window(newPage));
        CurrentPage = newPage;
        return newPage;
    }

    public async Task<Page> BackToHome()
    {
        if (IsMobile)
        {
            await CurrentPage.Navigation.PopToRootAsync();
            CurrentPage = HomePage;
            return CurrentPage;
        }

        foreach (var window in application.Windows)
        {
            if (!window.Equals(homeWindow))
            {
                application.CloseWindow(window);
            }
        }

        return HomePage;
    }

    public Page HomePage { get; init; }

    public Page CurrentPage { get; private set; }

    private static bool IsMobile => DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS;
}
