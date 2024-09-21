namespace Decksteria.Core;

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Models;

/// <summary>
/// Represents a valid import command to convert another file type into a Decksteria <see cref="Decklist"/>.
/// </summary>
public interface IDecksteriaImport
{
    /// <summary>
    /// The file extension the import function is designed to handle.
    /// </summary>
    public string FileType { get; }

    /// <summary>
    /// The label displayed to the user for selecting an import option.
    /// </summary>
    public string Label { get; }

    /// <summary>
    /// Converts a different file format into a <see cref="Decklist"/>.
    /// </summary>
    /// <param name="memoryStream">The <see cref="MemoryStream"/> containing the data from a file.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the execution.</param>
    /// <returns>The <see cref="Decklist"/> created from loading the file format.</returns>
    public Task<Decklist> LoadDecklistAsync(MemoryStream memoryStream, IDecksteriaFormat currentFormat, CancellationToken cancellationToken = default!);
}