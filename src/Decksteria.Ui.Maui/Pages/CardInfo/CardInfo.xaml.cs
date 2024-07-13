namespace Decksteria.Ui.Maui.Pages.CardInfo;

using System;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Services.Deckbuilding;
using Decksteria.Services.FileService.Models;
using Decksteria.Ui.Maui.Services.PageService;
using Decksteria.Ui.Maui.Shared.Pages;
using Microsoft.Maui.Controls;

public partial class CardInfo : ContentPage, IFormPage<CardInfo>
{
    private readonly IDeckbuildingService deckbuildingService;

    private readonly IPageService pageService;

    private readonly CardInfoViewModel viewModel;

    public CardInfo(CardArt cardArt, IDeckbuildingService deckbuildingService, IPageService pageService)
	{
		InitializeComponent();
        this.deckbuildingService = deckbuildingService;
        this.pageService = pageService;
        viewModel = new()
        {
            CardInfo = cardArt
        };
        this.BindingContext = viewModel;
    }

    public bool DecksChanged { get; set; } = false;

    public Func<CardInfo, CancellationToken, Task>? OnPopAsync { get; set; }

    private async void CardInfo_Loaded(object sender, EventArgs e)
    {
        foreach (var deck in deckbuildingService.DeckInformation)
        {
            var id = viewModel.CardInfo.CardId;
            var count = deckbuildingService.GetCardCountFromDeck(id, deck.Name);
            var canAdd = await deckbuildingService.CanAddCardAsync(id, deck.Name);
            var cardDeckInfo = new CardDeckInfo
            {
                CanAddCard = canAdd,
                Count = count
            };

            viewModel.DeckCounts.Add(deck.Name, cardDeckInfo);
            DecksLayout.Add(cardDeckInfo.CreateVisualElement(deck.Label, CreateAddAction(deck.Name, cardDeckInfo), CreateRemoveAction(deck.Name, cardDeckInfo)));
        }

        Action<object?, EventArgs> CreateAddAction(string deckName, CardDeckInfo cardDeckInfo)
        {
            return async (sender, e) =>
            {
                var cardInfo = viewModel.CardInfo;
                await deckbuildingService.AddCardAsync(cardInfo, deckName);
                cardDeckInfo.Count = deckbuildingService.GetCardCountFromDeck(cardInfo.CardId, deckName);
                cardDeckInfo.CanAddCard = await deckbuildingService.CanAddCardAsync(cardInfo.CardId, deckName);
                DecksChanged = true;
            };
        }

        Action<object?, EventArgs> CreateRemoveAction(string deckName, CardDeckInfo cardDeckInfo)
        {
            return async (sender, e) =>
            {
                var cardInfo = viewModel.CardInfo;
                var removed = await deckbuildingService.RemoveCardAsync(cardInfo, deckName);
                if (!removed)
                {
                    return;
                }

                cardDeckInfo.Count = deckbuildingService.GetCardCountFromDeck(cardInfo.CardId, deckName);
                cardDeckInfo.CanAddCard = await deckbuildingService.CanAddCardAsync(cardInfo.CardId, deckName);
                DecksChanged = true;
            };
        }
    }

    private async void CloseButton_Pressed(object sender, EventArgs e)
    {
        await pageService.PopModalAsync<CardInfo>();
    }
}