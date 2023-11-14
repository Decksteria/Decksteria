namespace Decksteria.Ui.Maui;

using Decksteria.Ui.Maui.Services.PageService;

public partial class App : Application
{
    public App(IPageService pageService)
    {
        InitializeComponent();

        this.MainPage = pageService.HomePage;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Title = "Decksteria Deckbuilder";
        return window;
    }
}
