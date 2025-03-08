namespace Decksteria.Ui.Maui.Pages.Preferences;

using System;
using Decksteria.Ui.Maui.Services.CardImageService;
using Decksteria.Ui.Maui.Services.PageService;
using Decksteria.Ui.Maui.Services.PreferencesService;
using Decksteria.Ui.Maui.Shared.Configuration;
using UraniumUI.Pages;

public partial class UserPreferences : UraniumContentPage
{
    private readonly IPreferencesService preferencesService;

    private readonly IDecksteriaCardImageService cardImageService;

    private readonly IPageService pageService;

    private PreferencesViewModel viewModel;

    public UserPreferences(IPreferencesService preferencesService, IDecksteriaCardImageService cardImageService, IPageService pageService)
	{
		InitializeComponent();
        this.preferencesService = preferencesService;
        this.cardImageService = cardImageService;
        this.pageService = pageService;
        this.viewModel = new PreferencesViewModel(preferencesService.GetConfiguration().Value);
        this.BindingContext = this.viewModel;
    }

    private async void Button_ClearImages_Pressed(object sender, EventArgs e)
    {
        await cardImageService.DeleteAllImagesAsync();
    }

    private async void Button_Save_Pressed(object sender, EventArgs e)
    {
        preferencesService.SaveToSettings(viewModel.Preferences);
        await pageService.PopModalAsync<UserPreferences>();
    }

    private void Button_Cancel_Pressed(object sender, EventArgs e)
    {
        pageService.PopModalAsync<UserPreferences>();
    }
}