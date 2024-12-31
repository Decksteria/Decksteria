namespace Decksteria.Ui.Maui.Pages.DeckStatistics;

using System.Collections.ObjectModel;
using System.ComponentModel;

internal sealed class SectionInfo : INotifyPropertyChanged
{
    public required bool Sortable { get; init; }

    public required ObservableCollection<StatisticInfo> Statistics { get; set; }

    public required string Title { get; init; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
