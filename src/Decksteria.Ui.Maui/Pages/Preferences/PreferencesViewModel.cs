namespace Decksteria.Ui.Maui.Pages.Preferences;

/// <summary>
/// View model for preferences before it gets saved.
/// </summary>
internal sealed class PreferencesViewModel
{
    /// <summary>
    /// Determines whether card images will be downloaded by the application.
    /// </summary>
    public bool DownloadImages { get; set; }
}
