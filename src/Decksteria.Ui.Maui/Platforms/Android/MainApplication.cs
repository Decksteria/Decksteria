#pragma warning disable IDE0130 // Namespace does not match folder structure, this namespace is required for using the Android namespaces.
namespace Decksteria.Ui.Maui;
#pragma warning disable IDE0130 // Namespace does not match folder structure, this namespace is required for using the Android namespaces.

using System;
using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
