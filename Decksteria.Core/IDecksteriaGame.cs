namespace Decksteria.Core;

/// <summary>
/// Represents the Game Plug-In of the Assembly.
/// </summary>
public interface IDecksteriaGame : IDecksteriaTile
{
    /// <summary>
    /// A list of all the formats and their deck-building rulesets available by the Plug-In.
    /// </summary>
    public IEnumerable<IDecksteriaFormat> Formats { get; }

    /// <summary>
    /// A list of all supported Import file types and how to perform them.
    /// </summary>
    public IEnumerable<IDecksteriaImport> Importers { get; }

    /// <summary>
    /// A list of all supported Export file types and how to perform them.
    /// </summary>
    public IEnumerable<IDecksteriaExport> Exporters { get; }
}