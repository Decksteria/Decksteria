namespace Decksteria.Ui.Maui;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

public partial class App : Application
{
    private readonly AppShell appShell;

    public App(AppShell appShell)
    {
        InitializeComponent();

        this.appShell = appShell;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(appShell);
    }
}
