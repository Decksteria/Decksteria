namespace Decksteria.Ui.Maui;

using Decksteria.Ui.Maui.Pages.LoadPlugIn;
using Decksteria.Ui.Maui.Services.PageService;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;

public partial class App : Application
{
    public App(IPageService pageService)
    {
        InitializeComponent();
        pageService.OpenPageAsync<LoadPlugIn>();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Title = "Decksteria Deckbuilder";
        return window;
    }
}
