namespace Decksteria.Ui.Maui.Pages.CardInfo;

using System;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Services.Deckbuilding;
using Decksteria.Services.DeckFileService.Models;
using Decksteria.Ui.Maui.Services.CardImageService;
using Decksteria.Ui.Maui.Services.PageService;
using Decksteria.Ui.Maui.Shared.Controls;
using Microsoft.Maui.Controls;

public partial class CardInfo : ContentPage
{
    private readonly IDecksteriaCardImageService cardImageService;

    private readonly IDeckbuildingService deckbuildingService;

    private readonly IPageService pageService;

    private readonly CardInfoViewModel viewModel;

    public CardInfo(CardArt cardArt, IDecksteriaCardImageService cardImageService, IDeckbuildingService deckbuildingService, IPageService pageService)
	{
		InitializeComponent();
        this.cardImageService = cardImageService;
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
            var view = cardDeckInfo.CreateVisualElement(deck.Label, CreateAddAction(deck.Name, cardDeckInfo), CreateRemoveAction(deck.Name, cardDeckInfo));
            DecksLayout.Add(view);
        }

        Action<object?, EventArgs> CreateAddAction(string deckName, CardDeckInfo cardDeckInfo)
        {
            return async (sender, e) =>
            {
                var added = await deckbuildingService.AddCardAsync(viewModel.CardInfo, deckName);
                if (added)
                {
                    UpdateDeckInformation(deckName, cardDeckInfo);
                }
            };
        }

        Action<object?, EventArgs> CreateRemoveAction(string deckName, CardDeckInfo cardDeckInfo)
        {
            return async (sender, e) =>
            {
               
                var removed = await deckbuildingService.RemoveCardAsync(viewModel.CardInfo, deckName);
                if (removed)
                {
                    UpdateDeckInformation(deckName, cardDeckInfo);
                }
            };
        }
    }

    private async void CloseButton_Pressed(object sender, EventArgs e)
    {
        await pageService.PopModalAsync<CardInfo>();
    }

    private void DecksteriaImageControl_SetService(object sender, EventArgs e)
    {
        if (sender is not DownloadableImage imageControl)
        {
            return;
        }

        imageControl.SetCardImageService(cardImageService);
    }

    private async void UpdateDeckInformation(string deckName, CardDeckInfo? cardDeckInfo = null)
    {
        if (cardDeckInfo is null && !viewModel.DeckCounts.TryGetValue(deckName, out cardDeckInfo))
        {
            return;
        }

        var cardInfo = viewModel.CardInfo;
        cardDeckInfo.Count = deckbuildingService.GetCardCountFromDeck(cardInfo.CardId, deckName);
        cardDeckInfo.CanAddCard = await deckbuildingService.CanAddCardAsync(cardInfo.CardId, deckName);
        DecksChanged = true;
    }
}