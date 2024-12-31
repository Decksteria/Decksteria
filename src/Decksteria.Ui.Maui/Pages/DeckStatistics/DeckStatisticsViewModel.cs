namespace Decksteria.Ui.Maui.Pages.DeckStatistics;

using System.Collections.Generic;
using System.ComponentModel;

internal sealed class DeckStatisticsViewModel : INotifyPropertyChanged
{
    public required IEnumerable<SectionInfo> Sections { get; init; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
