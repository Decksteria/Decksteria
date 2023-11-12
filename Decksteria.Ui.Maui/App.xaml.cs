namespace Decksteria.Ui.Maui;

using Decksteria.Ui.Maui.Services.PlugInInitializer;

public partial class App : Application
{
    public App(IPlugInInitializer plugInInitializer)
    {
        InitializeComponent();

        MainPage = new LoadPlugIn(plugInInitializer, new());
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Title = "Decksteria Deckbuilder";
        return window;
    }
}
