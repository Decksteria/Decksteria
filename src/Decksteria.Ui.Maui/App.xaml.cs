namespace Decksteria.Ui.Maui;

using Microsoft.Maui.Controls;

public partial class App : Application
{
    public App(AppShell appShell)
    {
        InitializeComponent();

        MainPage = appShell;
    }
}
