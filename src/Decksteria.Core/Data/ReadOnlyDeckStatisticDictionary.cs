namespace Decksteria.Core.Data;

using System.Collections.ObjectModel;
using Decksteria.Core.Models;

/// <inheritdoc cref="ReadOnlyDictionary{TKey, TValue}"/>
public sealed class ReadOnlyDeckStatisticDictionary : ReadOnlyDictionary<string, (DeckStatisticMetadata metadata, int value)>
{
    private readonly DeckStatisticDictionary dictionary;

    /// <summary>
    /// Initialises a new instance of <see cref="ReadOnlyDeckStatisticDictionary"/> that is a wrapper for the specified dictionary.
    /// </summary>
    /// <param name="dictionary">The dictionary to wrap.</param>
    public ReadOnlyDeckStatisticDictionary(DeckStatisticDictionary dictionary) : base(dictionary)
    {
        this.dictionary = dictionary;
    }

    /// <inheritdoc cref="DeckStatisticDictionary.SortKeys"/>
    /// <returns>A new dictionary with the keys sorted.</returns>
    public ReadOnlyDeckStatisticDictionary SortKeys()
    {
        dictionary.SortKeys();
        return dictionary;
    }

    /// <inheritdoc cref="DeckStatisticDictionary.SortValues"/>
    /// <returns>A new dictionary with the values sorted.</returns>
    public ReadOnlyDeckStatisticDictionary SortValues()
    {
        dictionary.SortValues();
        return dictionary;
    }

    /// <inheritdoc cref="DeckStatisticDictionary.TryGetMetadata(string, out DeckStatisticMetadata)"/>
    public bool TryGetMetadata(string key, out DeckStatisticMetadata value)
    {
        return dictionary.TryGetMetadata(key, out value);
    }

    /// <inheritdoc cref="DeckStatisticDictionary.TryGetValue(string, out int)"/>
    public bool TryGetValue(string key, out int value)
    {
        return dictionary.TryGetValue(key, out value);
    }
}