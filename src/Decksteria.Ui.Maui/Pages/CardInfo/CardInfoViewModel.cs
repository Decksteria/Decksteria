namespace Decksteria.Ui.Maui.Pages.CardInfo;

using System.Collections.Generic;
using Decksteria.Services.FileService.Models;

internal sealed class CardInfoViewModel
{
    public required CardArt CardInfo { get; set; }

    public IDictionary<string, CardDeckInfo> DeckCounts { get; set; } = new Dictionary<string, CardDeckInfo>();
}