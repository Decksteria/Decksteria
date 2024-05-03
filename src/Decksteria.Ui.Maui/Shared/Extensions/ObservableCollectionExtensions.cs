namespace Decksteria.Ui.Maui.Shared.Extensions;

using System.Collections.Generic;
using System.Collections.ObjectModel;

internal static class ObservableCollectionExtensions
{
    public static void ReplaceData<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> items)
    {
        observableCollection.Clear();

        foreach (var item in items)
        {
            observableCollection.Add(item);
        }
    }
}
