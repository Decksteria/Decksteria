namespace Decksteria.Core.Models;

/// <summary>
/// Represents what comparison the user wants to perform on a
/// particular advanced filter field.
/// </summary>
public enum ComparisonType
{
    /// <summary>
    /// <para>Used for <see cref="FieldType.Text"/>, <see cref="FieldType.Number"/>, or
    /// <see cref="FieldType.SingleSelect"/>, or <see cref="FieldType.MultiSelect"/> if the property is exactly equal
    /// to the user input
    /// (<see cref="SearchFieldFilter.Value"/> == <see cref="object"/>).
    /// </para>
    /// </summary>
    Equals,
    /// <summary>
    /// <para>
    /// Used for <see cref="FieldType.Text"/>, <see cref="FieldType.Number"/>, or
    /// <see cref="FieldType.SingleSelect"/>, if the property is not equal to the the
    /// user input 
    /// (<see cref="SearchFieldFilter.Value"/> !=<see cref="object"/>).
    /// </para>
    /// </summary>
    NotEquals,
    /// <summary>
    /// <para>
    /// Used for <see cref="FieldType.Text"/> if the property contains
    /// the user input (<see cref="string.Contains(SearchFieldFilter.Value)"/>.
    /// </para>
    /// <para>
    /// Used for <see cref="FieldType.MultiSelect"/> if the property includes any of the user inputs
    /// (<see cref="int"/> &amp; <see cref="SearchFieldFilter.Value"/> &gt; 0).
    /// </para>
    /// </summary>
    Contains,
    /// <summary>
    /// <para>
    /// Used for <see cref="FieldType.Text"/> if the property does
    /// not contain the user input (!<see cref="string.Contains(SearchFieldFilter.Value)"/>.
    /// </para>
    /// <para>
    /// Used for <see cref="FieldType.MultiSelect"/> if the property includes none of the user inputs
    /// (<see cref="int"/> &amp; <see cref="SearchFieldFilter.Value"/> == 0).
    /// </para>
    /// </summary>
    NotContains,
    /// <summary>
    /// Used for <see cref="FieldType.Text"/> if the property starts
    /// with the value string.
    /// </summary>
    StartsWith,
    /// <summary>
    /// Used for <see cref="FieldType.Text"/> if the property does not
    /// start with with the value string.
    /// </summary>
    EndsWith,
    /// <summary>
    /// Used for <see cref="FieldType.Number"/> if the property is greater than the user input.
    /// </summary>
    GreaterThan,
    /// <summary>
    /// <para>
    /// Used for <see cref="FieldType.Number"/> if the property is greater or equal to than the user input.
    /// </para>
    /// <para>
    /// Used for <see cref="FieldType.MultiSelect"/> to see if the property does not contain
    /// all of the user inputs
    /// (<see cref="int"/> &amp; <see cref="SearchFieldFilter.Value"/> >= <see cref="SearchFieldFilter.Value"/>).
    /// </para>
    /// </summary>
    GreaterThanOrEqual,
    /// <summary>
    /// <para>
    /// Used for <see cref="FieldType.Number"/> if the property is less than the user input.
    /// </para>
    /// <para>
    /// Used for <see cref="FieldType.MultiSelect"/> to see if the property does not contain
    /// all of the user inputs
    /// (<see cref="int"/> &amp; <see cref="SearchFieldFilter.Value"/> == 0).
    /// </para>
    /// </summary>
    LessThan,
    /// <summary>
    /// Used for <see cref="FieldType.Number"/> if the property is less or equal to than the user input.
    /// </summary>
    LessThanOrEqual
}