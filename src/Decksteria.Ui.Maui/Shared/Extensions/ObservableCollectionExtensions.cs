namespace Decksteria.Ui.Maui.Shared.Extensions;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

    public static void UpdateData<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> items) where T : notnull
    {
        // Count occurrences in both lists
        var count1 = observableCollection.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
        var count2 = items.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

        // Remove items from observable collection
        for (var i = observableCollection.Count - 1; i >= 0; i--)
        {
            if (!count2.ContainsKey(observableCollection[i]) || count1[observableCollection[i]] > count2[observableCollection[i]])
            {
                count1[observableCollection[i]]--;
                observableCollection.RemoveAt(i);
            }
        }

        // Add items from list2 to list1 to match the counts
        foreach (var kvp in count2)
        {
            var difference = kvp.Value - (count1.ContainsKey(kvp.Key) ? count1[kvp.Key] : 0);
            for (var i = 0; i < difference; i++)
            {
                observableCollection.Add(kvp.Key);
            }
        }
    }
}
