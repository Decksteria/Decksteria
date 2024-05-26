namespace Decksteria.Core.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

public record SearchField
{
    /// <summary>
    /// Default constructor for initialising a <see cref="FieldType.Text"/> advanced filter field.
    /// </summary>
    /// <param name="fieldName">Label and name of the advanced filter field.</param>
    /// <param name="length">Maximum length of the text field.</param>
    public SearchField(string fieldName, int? length = null)
    {
        FieldName = fieldName;
        FieldType = FieldType.Text;
        Length = length ?? 255;
    }

    /// <summary>
    /// Default constructor for initialising a <see cref="FieldType.Number"/> advanced filter field.
    /// </summary>
    /// <param name="fieldName">Label and name of the advanced filter field.</param>
    /// <param name="minValue">Minimum value of the integer.</param>
    /// <param name="maxValue">Maximum value of the integer.</param>
    public SearchField(string fieldName, int minValue, int maxValue)
    {
        FieldName = fieldName;
        FieldType = FieldType.Number;
        MinValue = minValue;
        MaxValue = maxValue;
    }

    /// <summary>
    /// Default constructor for initialising a <see cref="FieldType.SingleSelect"/> advanced filter field.
    /// The first item in the array will be the default and perform no filtering.
    /// </summary>
    /// <param name="fieldName">Label and name of the advanced filter field.</param>
    /// <param name="options">Options available to the user. The first option will always be the default filter.</param>
    /// <param name="defaultSelect">The default option that will perform no filter.</param>
    public SearchField(string fieldName, List<string> options, string? defaultSelect)
    {
        FieldName = fieldName;
        FieldType = FieldType.SingleSelect;
        DefaultSelect = defaultSelect ?? options.First();

        if (options.Count == 0 && defaultSelect is null)
        {
            // Prevent an empty Options List.
            defaultSelect = "Anything";
        }

        if (defaultSelect is not null && !options.Contains(defaultSelect))
        {
            options.Insert(0, defaultSelect);
        }

        Options = options;
    }

    /// <summary>
    /// Default constructor for initialising a <see cref="FieldType.MultiSelect"/> advanced filter field.
    /// If there are more than 30 items in <paramref name="options"/>, it becomes a <see cref="FieldType.SingleSelect"/> instead.
    /// </summary>
    /// <param name="fieldName">Label and name of the advanced filter field.</param>
    /// <param name="options">Options available to the user. There must be 32 or less items in the collection.</param>
    public SearchField(string fieldName, IEnumerable<string> options)
    {
        var uniqueItems = options.Distinct();
        if (uniqueItems.Count() > 32)
        {
            FieldName = fieldName;
            FieldType = FieldType.SingleSelect;
            DefaultSelect = "Anything";
            var optionsList = options.ToList();
            optionsList.Insert(0, DefaultSelect);
            Options = optionsList;
            return;
        }

        FieldName = fieldName;
        FieldType = FieldType.MultiSelect;
        Options = uniqueItems;

        var dictionary = new Dictionary<string, uint>();
        uint value = 1;
        foreach (var option in uniqueItems)
        {
            dictionary.Add(option, value);
            value *= 2;
        }

        OptionMapping = dictionary;
    }

    /// <summary>
    /// Default constructor for initialising a <see cref="FieldType.MultiSelect"/> advanced filter field.
    /// </summary>
    /// <param name="fieldName">Label and name of the advanced filter field.</param>
    /// <param name="options">Options mapped to an unsigned integer that is a power 2 (1, 2, 4, etc.). Values will be compared via bitwise.</param>
    public SearchField(string fieldName, IReadOnlyDictionary<string, uint> options)
    {
        FieldName = fieldName;
        FieldType = FieldType.MultiSelect;
        Options = options.Keys;
        OptionMapping = options;
    }

    /// <summary>
    /// The name and label of the <see cref="SearchField"/>.
    /// </summary>
    public string FieldName { get; }

    /// <summary>
    /// The type of the <see cref="SearchField"/>, it determines the rendering and
    /// values the advanced filter field can have.
    /// </summary>
    public FieldType FieldType { get; }

    /// <summary>
    /// The maximum length of a <see cref="FieldType.Text"/> field.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// The minimum value of a <see cref="FieldType.Number"/> field.
    /// </summary>
    public int MinValue { get; }

    /// <summary>
    /// The maximum value of a <see cref="FieldType.Number"/> field.
    /// </summary>
    public int MaxValue { get; }

    /// <summary>
    /// All of the allowable values of <see cref="FieldType.SingleSelect"/> or <see cref="FieldType.MultiSelect"/> field
    /// </summary>
    public IEnumerable<string> Options { get; } = Array.Empty<string>();

    /// <summary>
    /// The option that will perform no filtering for a <see cref="FieldType.SingleSelect"/>.
    /// </summary>
    public string? DefaultSelect { get; }

    /// <summary>
    /// Map of all 
    /// </summary>
    public IReadOnlyDictionary<string, uint>? OptionMapping { get; }
}