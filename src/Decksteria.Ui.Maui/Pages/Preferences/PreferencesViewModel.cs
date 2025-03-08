namespace Decksteria.Ui.Maui.Pages.Preferences;

using Decksteria.Ui.Maui.Shared;
using Decksteria.Ui.Maui.Shared.Configuration;

/// <summary>
/// View model for preferences before it gets saved.
/// </summary>
public sealed class PreferencesViewModel : BaseViewModel
{
    public PreferencesViewModel(PreferenceConfiguration preferences)
    {
        this.Preferences = preferences;
    }

    /// <summary>
    /// The preference configurations.
    /// </summary>
    internal PreferenceConfiguration Preferences { get; init; }

    public bool AllowDownloads
    {
        get => Preferences.DownloadImages;
        set
        {
            Preferences.DownloadImages = value;
            OnPropertyChanged();
        }
    }

    public bool PreventDownloads
    {
        get => !Preferences.DownloadImages;
        set
        {
            Preferences.DownloadImages = !value;
            OnPropertyChanged();
        }
    }
}
