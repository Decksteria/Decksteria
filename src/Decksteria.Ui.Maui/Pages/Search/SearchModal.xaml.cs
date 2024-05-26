namespace Decksteria.Ui.Maui.Pages.Search;

using System;
using System.Collections.Generic;
using Decksteria.Core.Models;
using Decksteria.Services.PlugInFactory.Models;
using Decksteria.Ui.Maui.Pages.Search.Model;
using Decksteria.Ui.Maui.Services.PageService;
using Decksteria.Ui.Maui.Shared.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using UraniumUI.Material.Controls;
using UraniumUI.Pages;

public partial class SearchModal : UraniumContentPage
{
    private readonly SearchModalViewModel viewModel = new();

    private readonly IPageService pageService;

    private readonly IEnumerable<SearchField> searchFields;

    public SearchModal(GameFormat gameFormat, IPageService pageService)
    {
        InitializeComponent();
        this.pageService = pageService;
        this.BindingContext = viewModel;
        this.searchFields = gameFormat.Format.SearchFields;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        Layout_SearchFilters.Clear();
        viewModel.SearchFieldFilters = CreateSearchFields(searchFields);

        IEnumerable<IMauiSearchFilter> CreateSearchFields(IEnumerable<SearchField> searchFields)
        {
            var mauiSearchFields = new List<IMauiSearchFilter>();
            foreach (var field in searchFields)
            {
                IMauiSearchFilter? filterField = field.FieldType switch
                {
                    FieldType.Text => new TextSearchFilter(field),
                    FieldType.Number => new NumberSearchFilter(field),
                    FieldType.SingleSelect => new SingleSelectionSearchFilter(field),
                    FieldType.MultiSelect => new MultiSelectionSearchFilter(field),
                    _ => null
                };

                if (filterField is null)
                {
                    continue;
                }

                Layout_SearchFilters.Add(filterField.GetVisualElement());
                mauiSearchFields.Add(filterField);
            }

            return mauiSearchFields;
        }
    }

    private async void Button_Cancel_Pressed(object sender, EventArgs e)
    {
        _ = await pageService.PopModalAsync<SearchModal>();
    }
}