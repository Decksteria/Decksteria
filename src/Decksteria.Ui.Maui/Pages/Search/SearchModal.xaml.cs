namespace Decksteria.Ui.Maui.Pages.Search;

using System.Collections.Generic;
using System.Linq;
using Decksteria.Core.Models;
using Decksteria.Services.PlugInFactory.Models;
using UraniumUI.Pages;

public partial class SearchModal : UraniumContentPage
{
    private readonly SearchModalViewModel viewModel;

    public SearchModal(GameFormat gameFormat)
    {
        InitializeComponent();
        viewModel.SearchFieldFilters = gameFormat.Format.SearchFields.Select(s => new SearchFieldFilter(s, ComparisonType.Equals));
    }

    public int? ValueTest { get; set; }
}