namespace Decksteria.Ui.Maui.Pages.Search;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Models;
using Decksteria.Services.PlugInFactory.Models;
using Decksteria.Ui.Maui.Pages.Search.Model;
using Decksteria.Ui.Maui.Services.PageService;
using Decksteria.Ui.Maui.Shared.Pages;
using UraniumUI.Pages;

public partial class SearchModal : UraniumContentPage, IFormModalPage<SearchModal>
{
    private readonly IPageService pageService;

    private readonly IEnumerable<SearchField> searchFields;

    private bool firstLoaded;

    public SearchModal(GameFormat gameFormat, IPageService pageService)
    {
        InitializeComponent();
        this.pageService = pageService;
        this.BindingContext = ViewModel;
        this.searchFields = gameFormat.Format.SearchFields;
    }

    public bool IsSubmitted { get; internal set; } = false;

    public Func<SearchModal, CancellationToken, Task>? OnSubmitAsync { get; set; } = null;

    public Func<SearchModal, CancellationToken, Task>? OnPopAsync { get; set; } = null;

    internal SearchModalViewModel ViewModel { get; } = new();

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        if (firstLoaded)
        {
            return;
        }

        Layout_SearchFilters.Clear();
        ViewModel.SearchFieldFilters = CreateSearchFields(searchFields);
        firstLoaded = true;

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

    private async void Button_Submit_Pressed(object sender, EventArgs e)
    {
        IsSubmitted = true;
        _ = await pageService.PopModalAsync<SearchModal>();
    }

    private async void Button_Cancel_Pressed(object sender, EventArgs e)
    {
        _ = await pageService.PopModalAsync<SearchModal>();
    }
}