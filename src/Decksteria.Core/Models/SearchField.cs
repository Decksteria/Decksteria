namespace Decksteria.Core.Models;

using System;
using System.Collections.Generic;

public record SearchField
{
    /// <summary>
    /// Default constructor for initialising a text-based advanced filter field.
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
    /// Default constructor for initialising a number-based advanced filter field.
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
    /// Default constructor for initialising a selection-based advanced filter field.
    /// It will always be a multi-select field.
    /// </summary>
    /// <param name="fieldName">Label and name of the advanced filter field.</param>
    /// <param name="options">Options available to the user. The first option will always be the default filter.</param>
    public SearchField(string fieldName, IEnumerable<string> options)
    {
        FieldName = fieldName;
        FieldType = FieldType.Selection;
        Options = options;
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
    /// All of the allowable values of <see cref="FieldType.Selection"/> field
    /// </summary>
    public IEnumerable<string> Options { get; } = Array.Empty<string>();
}