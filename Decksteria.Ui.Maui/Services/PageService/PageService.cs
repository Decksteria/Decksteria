namespace Decksteria.Ui.Maui.Services.PageService;

using System.Threading.Tasks;

internal sealed class PageService() : IPageService
{
    public async Task OpenPageAsync(Page newPage, Page? currentPage = null)
    {
        if (Application.MainPage is null)
        {
            Application.MainPage = newPage;
            return;
        }

        currentPage ??= Application.MainPage;
        if (currentPage is NavigationPage)
        {
            await currentPage!.Navigation.PushAsync(newPage);
            return;
        }

        Application.OpenWindow(new Window(newPage));
    }

    public async Task BackToHomeAsync(Page currentPage)
    {
        if (currentPage is NavigationPage)
        {
            await currentPage.Navigation.PopToRootAsync();
            return;
        }

        foreach (var window in Application.Windows)
        {
            if (window.Parent is not null)
            {
                Application.CloseWindow(window);
            }
        }
    }

    private static Application Application => Application.Current ?? throw new ApplicationException("Application has not loaded correctly.");
}