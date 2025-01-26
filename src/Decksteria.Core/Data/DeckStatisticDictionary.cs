namespace Decksteria.Core.Data;

using System.Collections.Generic;
using System.Linq;
using Decksteria.Core.Models;

/// <summary>
/// An object model for tracking the number of cards that match a particular condition as a statistic.
/// </summary>
public sealed class DeckStatisticDictionary : Dictionary<string, (DeckStatisticMetadata metadata, int value)>
{
    /// <summary>
    /// Converts it to a <see cref="ReadOnlyDeckStatisticDictionary"/> implicitly.
    /// </summary>
    /// <param name="dictionary"></param>
    public static implicit operator ReadOnlyDeckStatisticDictionary(DeckStatisticDictionary dictionary)
    {
        return dictionary.AsReadOnly();
    }

    /// <inheritdoc cref="Add(string, int, DeckStatisticMetadata?)"/>m>
    /// <summary>
    /// Adds the specified key with a count of 0 to the dictionary.
    /// </summary>
    public void AddKey(string key, DeckStatisticMetadata? metadata = null)
    {
        this.Add(key, 0, metadata);
    }

    /// <inheritdoc cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/>
    /// <param name="value">The number of cards that match the statistic.</param>
    /// <param name="metadata">Metadata to apply as part of the key.</param>
    public void Add(string key, int value, DeckStatisticMetadata? metadata = null)
    {
        base.Add(key, (metadata ?? new DeckStatisticMetadata(), value));
    }

    /// <summary>
    /// If the key already exists, then it increments the count for the statistic.
    /// If it does not, it adds an item to it with a count of 1 and no metadata.
    /// </summary>
    /// <param name="key">The key of the element to add or increment.</param>
    public void AddOrIncrementValue(string key)
    {
        var hasValue = base.TryGetValue(key, out var data);
        if (hasValue)
        {
            data.value++;
            return;
        }

        this.Add(key, 1, new());
    }

    /// <inheritdoc cref="CollectionExtensions.AsReadOnly{T}(IList{T})"/>
    /// <returns>A read-only dictionary with the same records as this dictionary.</returns>
    public ReadOnlyDeckStatisticDictionary AsReadOnly()
    {
        return new ReadOnlyDeckStatisticDictionary(this);
    }

    /// <inheritdoc cref="Dictionary{TKey, TValue}.Remove(TKey)"/>
    public new bool Remove(string key)
    {
        return base.Remove(key);
    }

    /// <inheritdoc cref="Dictionary{TKey, TValue}.Remove(TKey)"/>
    /// <param name="metadata">The metadata stored for the key.</param>
    /// <param name="count">The count stored for the key.</param>
    public bool Remove(string key, out DeckStatisticMetadata metadata, out int count)
    {
        var removed = base.Remove(key, out var data);
        metadata = data.metadata;
        count = data.value;
        return removed;
    }

    /// <summary>
    /// Sorts the items inside the dictionary by the key in ascending order.
    /// </summary>
    public void SortKeys()
    {
        var sortedEntries = this.OrderBy(kv => kv.Key).ToList();

        this.Clear();

        foreach (var kv in sortedEntries)
        {
            this.Add(kv.Key, kv.Value);
        }
    }

    /// <summary>
    /// Sorts the items inside the dictionary by the counts in descending order.
    /// </summary>
    public void SortValues()
    {
        var sortedEntries = this.OrderByDescending(kv => kv.Value.value).ToList();

        this.Clear();

        foreach (var kv in sortedEntries)
        {
            this.Add(kv.Key, kv.Value);
        }
    }

    /// <inheritdoc cref="TryAdd(string, int, DeckStatisticMetadata?)"/>
    /// <summary>
    /// Attempts to adds the specified key with a count of 0 to the dictionary.
    /// </summary>
    public bool TryAddKey(string key, DeckStatisticMetadata? metadata = null)
    {
        return this.TryAdd(key, 0, metadata);
    }

    /// <inheritdoc cref="Dictionary{TKey, TValue}.TryAdd(TKey, TValue)"/>
    /// <param name="value">The count of the statistic.</param>
    /// <param name="metadata">The metadata associated with the key.</param>
    public bool TryAdd(string key, int value, DeckStatisticMetadata? metadata = null)
    {
        return base.TryAdd(key, (metadata ?? new DeckStatisticMetadata(), value));
    }

    /// <inheritdoc cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
    public bool TryGetMetadata(string key, out DeckStatisticMetadata value)
    {
        var hasItem = base.TryGetValue(key, out var data);
        value = data.metadata;
        return hasItem;
    }

    /// <inheritdoc cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
    public bool TryGetValue(string key, out int value)
    {
        var hasItem = base.TryGetValue(key, out var data);
        value = data.value;
        return hasItem;
    }

    /// <summary>
    /// Replaces the metadata associated with the specified key.
    /// </summary>
    /// <param name="key">The specified key to update the metadata with.</param>
    /// <param name="value">The values to update the metadata to.</param>
    public void UpdateMetadata(string key, DeckStatisticMetadata value)
    {
        var hasValue = base.TryGetValue(key, out var data);
        if (hasValue)
        {
            data.metadata = value;
            return;
        }

        this.Add(key, 0, value);
    }

    /// <summary>
    /// Replaces the count associated with the specified key.
    /// </summary>
    /// <param name="key">The specified key to update the metadata with.</param>
    /// <param name="value">The count of items associated with the specified key.</param>
    public void UpdateValue(string key, int value)
    {
        var hasValue = base.TryGetValue(key, out var data);
        if (hasValue)
        {
            data.value = value;
            return;
        }

        this.Add(key, value, new());
    }
}
