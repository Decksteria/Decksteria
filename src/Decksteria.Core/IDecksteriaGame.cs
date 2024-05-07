namespace Decksteria.Core;

using System.Collections.Generic;

/// <summary>
/// Represents the actual plug-in of the Assembly.
/// The name of the plug-in will be the name of the class that inherits this interface.
/// Make sure to choose a name that you likely won't change in the future and is unique
/// to your plug-in.
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