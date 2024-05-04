namespace Decksteria.Ui.Maui;

using System;
using Decksteria.Ui.Maui.Services.PageService;
using Microsoft.Maui.Controls;

public partial class AppShell : Shell
{
    public AppShell(IPageService pageService)
    {
        InitializeComponent();
        ShellContent_Main.Content = pageService.CreateHomePageInstance();
    }

    private void Shell_Loaded(object sender, EventArgs e)
    {

    }
}
