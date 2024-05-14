namespace Decksteria.Ui.Maui.Pages.Search;

using System.Collections.Generic;
using System.Linq;
using Decksteria.Core.Models;
using Decksteria.Services.PlugInFactory.Models;
using Decksteria.Ui.Maui.Services.PageService;
using UraniumUI.Pages;

public partial class SearchModal : UraniumContentPage
{
    private readonly SearchModalViewModel viewModel = new();

    private readonly IPageService pageService;

    public SearchModal(GameFormat gameFormat, IPageService pageService)
    {
        InitializeComponent();
        viewModel.SearchFieldFilters = gameFormat.Format.SearchFields.Select(s => new SearchFieldFilter(s, ComparisonType.Equals));
        this.pageService = pageService;
        this.BindingContext = viewModel;
    }

    private async void Button_Pressed(object sender, System.EventArgs e)
    {
        await pageService.PopModalAsync<SearchModal>();
    }

    private void UraniumContentPage_Loaded(object sender, System.EventArgs e)
    {
        PickerField_Main.ItemsSource = viewModel.TestOptions;
    }
}