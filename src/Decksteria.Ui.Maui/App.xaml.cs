namespace Decksteria.Ui.Maui;

using System;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

public partial class App : Application
{
    private readonly AppShell appShell;

    private readonly ILogger<App> logger;

    public App(AppShell appShell, ILogger<App> logger)
    {
        InitializeComponent();

        this.appShell = appShell;
        this.logger = logger;
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(appShell);
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception exception)
        {
            var fullMessage = $"{exception.Message}.\r\n\t{exception.StackTrace}";
            logger.LogError(exception, fullMessage);
        }
    }
}
