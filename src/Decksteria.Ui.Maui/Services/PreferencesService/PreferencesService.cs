namespace Decksteria.Ui.Maui.Services.PreferencesService;

using Decksteria.Ui.Maui.Shared.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Maui.Storage;

internal sealed class PreferencesService : IPreferencesService
{
    private const string downloadKey = nameof(PreferenceConfiguration.DownloadImages);

    public PreferenceConfiguration preferences;

    public PreferencesService()
    {
        preferences = FromSavedSettings();
    }

    public IOptions<PreferenceConfiguration> GetConfiguration()
    {
        return Options.Create(preferences);
    }

    public void LoadFromSavedSettings()
    {
        preferences = FromSavedSettings();
    }

    public void SaveToSettings(PreferenceConfiguration newConfiguration)
    {
        preferences.DownloadImages = newConfiguration.DownloadImages;
        SaveSettings();
    }

    private PreferenceConfiguration FromSavedSettings()
    {
        return new()
        {
            DownloadImages = Preferences.Get(downloadKey, true)
        };
    }

    private void SaveSettings()
    {
        Preferences.Set(downloadKey, preferences.DownloadImages);
    }
}
