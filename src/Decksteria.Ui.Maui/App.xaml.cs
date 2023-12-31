﻿namespace Decksteria.Ui.Maui;

using Decksteria.Ui.Maui.Pages.LoadPlugIn;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;

public partial class App : Application
{
    public App(LoadPlugIn homePage)
    {
        InitializeComponent();
        this.MainPage = IsMobile ? new NavigationPage(homePage) : homePage;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Title = "Decksteria Deckbuilder";
        return window;
    }

    private static bool IsMobile => DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS;
}
