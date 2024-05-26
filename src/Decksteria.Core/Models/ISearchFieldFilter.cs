namespace Decksteria.Core.Models;

/// <summary>
/// An interface for filtering cards based on a card property.
/// </summary>
public interface ISearchFieldFilter
{
    /// <summary>
    /// The comparison type selected by the user.
    /// </summary>
    ComparisonType Comparison { get; init; }

    /// <summary>
    /// A <see cref="Models.SearchField"/> provided by the plug-in.
    /// </summary>
    SearchField SearchField { get; init; }

    /// <summary>
    /// The search value provided by the user.
    /// It will be a <see cref="int"/> when it is <see cref="FieldType.Number"/>.
    /// It will be a <see cref="string"/> when it is <see cref="FieldType.Text"/>.
    /// It will be a <see cref="string"/> when it is <see cref="FieldType.SingleSelect"/>.
    /// It will be a <see cref="int"/> when it is <see cref="FieldType.MultiSelect"/>.
    /// </summary>
    object? Value { get; init; }

    /// <summary>
    /// Default <see cref="int" /> filter matching, call this inside the GetCardsAsync if you don't need to do any special filter matching.
    /// Don't call this if you need a different implementation.
    /// </summary>
    /// <param name="cardProperty">The value you specifically want to match.</param>
    /// <returns>A boolean value indicating whether the <paramref name="cardProperty"/> matches the default filter criteria based on the value of the search field.</returns>
    bool MatchesFilter(int cardProperty);

    /// <summary>
    /// Default nullable <see cref="int" /> filter matching, call this inside the GetCardsAsync if you don't need to do any special filter matching.
    /// Don't call this if you need a different implementation.
    /// </summary>
    /// <param name="cardProperty">The value you specifically want to match.</param>
    /// <returns>A boolean value indicating whether the <paramref name="cardProperty"/> matches the default filter criteria based on the value of the search field.</returns>
    bool MatchesFilter(int? cardProperty);

    /// <summary>
    /// Default <see cref="string" /> filter matching, call this inside the GetCardsAsync if you don't need to do any special filter matching.
    /// Don't call this if you need a different implementation.
    /// </summary>
    /// <param name="cardProperty">The value you specifically want to match.</param>
    /// <returns>A boolean value indicating whether the <paramref name="cardProperty"/> matches the default filter criteria based on the value of the search field.</returns>
    bool MatchesFilter(string? cardProperty);

    /// <summary>
    /// Default <see cref="uint" /> filter matching, call this inside the GetCardsAsync if you don't need to do any special filter matching for <see cref="FieldType.MultiSelect"/>.
    /// Don't call this if you need a different implementation.
    /// </summary>
    /// <param name="cardProperty">The value you specifically want to match.</param>
    /// <returns>A boolean value indicating whether the <paramref name="cardProperty"/> matches the default filter criteria based on the value of the search field.</returns>
    bool MatchesFilter(uint cardProperty);
}