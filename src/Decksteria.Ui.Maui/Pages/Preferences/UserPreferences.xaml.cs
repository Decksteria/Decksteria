namespace Decksteria.Ui.Maui.Pages.Preferences;

using System;
using Decksteria.Ui.Maui.Services.PreferencesService;
using Decksteria.Ui.Maui.Shared.Configuration;
using Microsoft.Maui.Controls;

public partial class UserPreferences : ContentPage
{
    private readonly IPreferencesService preferencesService;

    private PreferenceConfiguration preferences;

    public UserPreferences(IPreferencesService preferencesService)
	{
		InitializeComponent();
        this.preferencesService = preferencesService;
        this.preferences = new();
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        preferences = preferencesService.GetConfiguration().Value;
    }
}