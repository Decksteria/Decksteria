namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using Decksteria.Core;
using Decksteria.Core.Models;
using Decksteria.Service.Deckbuilding;
using System.Threading.Tasks;

public partial class Deckbuilder : ContentPage
{
    private readonly DeckbuilderViewModel viewModel = new();

    private readonly IDeckbuildingService deckbuilder;

    public Deckbuilder(IDeckbuildingService deckbuilder)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.deckbuilder = deckbuilder;
    }

    private async void ContentPage_LoadedAsync(object sender, EventArgs e)
    {
        var formatCards = await deckbuilder.GetCardsAsync();
        foreach (var card in formatCards)
        {
            viewModel.FilteredCards.Add(card);
        }
    }
}