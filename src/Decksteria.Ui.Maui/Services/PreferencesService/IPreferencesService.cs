namespace Decksteria.Ui.Maui.Services.PreferencesService;

using Decksteria.Ui.Maui.Shared.Configuration;
using Microsoft.Extensions.Options;

/// <summary>
/// Service used for maintaining the state of the configuration during runtime and saving it for the next run.
/// </summary>
public interface IPreferencesService
{
    /// <summary>
    /// Retrieves the preferences configuration.
    /// </summary>
    /// <returns></returns>
    IOptions<PreferenceConfiguration> GetConfiguration();

    /// <summary>
    /// Reset all settings to the last saved settings state.
    /// </summary>
    void LoadFromSavedSettings();

    /// <summary>
    /// Saves settings to the application, propagates the changes to other services.
    /// </summary>
    /// <param name="newConfiguration">The new configuration values.</param>
    void SaveToSettings(PreferenceConfiguration newConfiguration);
}