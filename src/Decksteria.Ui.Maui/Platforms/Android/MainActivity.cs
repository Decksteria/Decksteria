#pragma warning disable IDE0130 // Namespace does not match folder structure, this namespace is required for using the Android namespaces.
namespace Decksteria.Ui.Maui;
#pragma warning disable IDE0130 // Namespace does not match folder structure, this namespace is required for using the Android namespaces.

using Android.App;
using Android.Content.PM;
using Microsoft.Maui;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}
