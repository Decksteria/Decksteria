namespace Decksteria.Ui.Maui.Pages.DeckStatistics;

using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Maui.Core.Extensions;
using Decksteria.Core.Models;
using Decksteria.Ui.Maui.Services.PageService;
using Microsoft.Maui.Controls;

public partial class DeckStatistics : ContentPage
{
	private readonly DeckStatisticsViewModel viewModel;

	private readonly IPageService pageService;

    public DeckStatistics(IEnumerable<DeckStatisticSection> statisticSections, IPageService pageService)
	{
		InitializeComponent();

        this.pageService = pageService;

        viewModel = new DeckStatisticsViewModel
		{
			Sections = statisticSections.Select(MapToSectionInfo)
		};

		this.BindingContext = viewModel;

		static SectionInfo MapToSectionInfo(DeckStatisticSection section)
		{
			return new()
			{
				Sortable = section.OrderByCount,
				Statistics = section.Statistics.Select(kv => new StatisticInfo
				{
					Count = kv.Value.value,
					Label = kv.Key
				}).ToObservableCollection(),
				Title = section.Label
			};
		}
    }

    private async void CloseButton_Pressed(object sender, EventArgs e)
    {
        await pageService.PopModalAsync<DeckStatistics>();
    }
}