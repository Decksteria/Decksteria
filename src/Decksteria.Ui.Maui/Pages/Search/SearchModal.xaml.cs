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

    private async void Button_Pressed(object sender, EventArgs e)
    {
        _ = await pageService.PopModalAsync<SearchModal>();
    }

    private void UraniumContentPage_Loaded(object sender, EventArgs e)
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
                    FieldType.Text => CreateTextField(field),
                    FieldType.Number => CreateNumberField(field),
                    FieldType.SingleSelect => CreateSingleSelectField(field),
                    FieldType.MultiSelect => CreateMultiSelectField(field),
                    _ => null
                };

                if (filterField is null)
                {
                    continue;
                }

                mauiSearchFields.Add(filterField);
            }

            return mauiSearchFields;
        }

        TextSearchFilter CreateTextField(SearchField searchField)
        {
            var textSearchFilter = new TextSearchFilter(searchField);
            var textField = new TextField
            {
                Title = textSearchFilter.Title
            };
            textField.SetBinding(TextField.MaxLengthProperty, new Binding(nameof(TextSearchFilter.MaxLength), BindingMode.OneWay, source: textSearchFilter));
            textField.SetBinding(TextField.TextProperty, new Binding(nameof(TextSearchFilter.Value), BindingMode.TwoWay, source: textSearchFilter));
            Layout_SearchFilters.Add(textField);
            return textSearchFilter;
        }

        SingleSelectionSearchFilter CreateSingleSelectField(SearchField searchField)
        {
            var selectSearchFilter = new SingleSelectionSearchFilter(searchField);
            var textField = new PickerField
            {
                Title = selectSearchFilter.Title
            };
            textField.SetBinding(PickerField.ItemsSourceProperty, new Binding(nameof(SingleSelectionSearchFilter.SelectableItems), BindingMode.OneWay, source: selectSearchFilter));
            textField.SetBinding(PickerField.SelectedItemProperty, new Binding(nameof(SingleSelectionSearchFilter.Value), BindingMode.TwoWay, source: selectSearchFilter));
            Layout_SearchFilters.Add(textField);
            return selectSearchFilter;
        }

        MultiSelectionSearchFilter CreateMultiSelectField(SearchField searchField)
        {
            var selectSearchFilter = new MultiSelectionSearchFilter(searchField);
            var textField = new MultiplePickerField
            {
                Title = selectSearchFilter.Title
            };
            textField.SetBinding(MultiplePickerField.ItemsSourceProperty, new Binding(nameof(MultiSelectionSearchFilter.SelectableItems), BindingMode.OneWay, source: selectSearchFilter));
            textField.SetBinding(MultiplePickerField.SelectedItemsProperty, new Binding(nameof(MultiSelectionSearchFilter.Values), BindingMode.TwoWay, source: selectSearchFilter));
            Layout_SearchFilters.Add(textField);
            return selectSearchFilter;
        }

        NumberSearchFilter CreateNumberField(SearchField searchField)
        {
            var numberSearchFilter = new NumberSearchFilter(searchField);
            var horizontalStack = new HorizontalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center
            };

            var stackControlMargins = new Thickness(5, 0);
            var lowerRangeField = new NumericField
            {
                Margin = stackControlMargins,
                Title = "Min"
            };
            lowerRangeField.SetBinding(NumericField.MinProperty, new Binding(nameof(NumberSearchFilter.Minimum), BindingMode.OneWay, source: numberSearchFilter));
            lowerRangeField.SetBinding(NumericField.MaxProperty, new Binding(nameof(NumberSearchFilter.MaximumValue), BindingMode.OneWay, source: numberSearchFilter));
            lowerRangeField.SetBinding(WidthRequestProperty, new Binding(nameof(NumberSearchFilter.WidthRequest), BindingMode.OneWay, source: numberSearchFilter));
            lowerRangeField.SetBinding(MinimumWidthRequestProperty, new Binding(nameof(NumberSearchFilter.WidthRequest), BindingMode.OneWay, source: numberSearchFilter));
            lowerRangeField.SetBinding(NumericField.ValueProperty, new Binding(nameof(NumberSearchFilter.MinimumValue), BindingMode.TwoWay, source: numberSearchFilter));

            var lessThanSymbolLabel = new Label
            {
                Text = "\u2264",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                Margin = stackControlMargins
            };

            var titleLabel = new Label
            {
                Margin = stackControlMargins,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
            };

            titleLabel.SetValue(Label.FontSizeProperty, "Medium");
            titleLabel.SetBinding(Label.TextProperty, new Binding(nameof(NumberSearchFilter.Title), BindingMode.OneWay, source: numberSearchFilter));

            var lessThanSymbolLabel2 = new Label
            {
                Text = "\u2264",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                Margin = stackControlMargins
            };

            var upperRangeField = new NumericField
            {
                Margin = stackControlMargins,
                Title = "Min"
            };
            upperRangeField.SetBinding(NumericField.MinProperty, new Binding(nameof(NumberSearchFilter.MinimumValue), BindingMode.OneWay, source: numberSearchFilter));
            upperRangeField.SetBinding(NumericField.MaxProperty, new Binding(nameof(NumberSearchFilter.Maximum), BindingMode.OneWay, source: numberSearchFilter));
            upperRangeField.SetBinding(WidthRequestProperty, new Binding(nameof(NumberSearchFilter.WidthRequest), BindingMode.OneWay, source: numberSearchFilter));
            upperRangeField.SetBinding(MinimumWidthRequestProperty, new Binding(nameof(NumberSearchFilter.WidthRequest), BindingMode.OneWay, source: numberSearchFilter));
            upperRangeField.SetBinding(NumericField.ValueProperty, new Binding(nameof(NumberSearchFilter.MaximumValue), BindingMode.TwoWay, source: numberSearchFilter));

            horizontalStack.Add(lowerRangeField);
            horizontalStack.Add(lessThanSymbolLabel);
            horizontalStack.Add(titleLabel);
            horizontalStack.Add(lessThanSymbolLabel2);
            horizontalStack.Add(upperRangeField);

            Layout_SearchFilters.Add(horizontalStack);
            return numberSearchFilter;
        }
    }
}