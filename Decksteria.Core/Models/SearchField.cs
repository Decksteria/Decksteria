namespace Decksteria.Core.Models;

public record SearchField
{
    /// <summary>
    /// Default Constructor for initialising a text-based Search Field.
    /// </summary>
    /// <param name="fieldName">Label and Name of the Search Field.</param>
    /// <param name="length">Maximum length of the text field.</param>
    public SearchField(string fieldName, int? length = null)
    {
        FieldName = fieldName;
        FieldType = FieldType.Text;
        Length = length ?? 255;
    }

    /// <summary>
    /// Default Constructor for initialising a number-based Search Field.
    /// </summary>
    /// <param name="fieldName">Label and Name of the Search Field.</param>
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
    /// Default Constructor for initialising a Selection-based Search Field.
    /// </summary>
    /// <param name="fieldName">Label and Name of the Search Field.</param>
    /// <param name="options">Options available to the user..</param>
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
    /// The type of the <see cref="SearchField"/>, it determines the valid values the Search Field can have.
    /// </summary>
    public FieldType FieldType { get; }

    /// <summary>
    /// The maximum length of a <see cref="FieldType.Text"/> field
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// The minimum value of a <see cref="FieldType.Number"/> field
    /// </summary>
    public int MinValue { get; }

    /// <summary>
    /// The maximum value of a <see cref="FieldType.Number"/> field
    /// </summary>
    public int MaxValue { get; }

    /// <summary>
    /// All of the allowable values of <see cref="FieldType.Selection"/> field
    /// </summary>
    public IEnumerable<string> Options { get; } = Array.Empty<string>();
}