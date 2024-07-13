namespace Decksteria.Ui.Maui.Pages.CardInfo;

using System.Collections.Generic;
using System.ComponentModel;
using Decksteria.Services.FileService.Models;

internal sealed class CardInfoViewModel : INotifyPropertyChanged
{
    public required CardArt CardInfo { get; set; }

    public IDictionary<string, CardDeckInfo> DeckCounts { get; set; } = new Dictionary<string, CardDeckInfo>();

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}