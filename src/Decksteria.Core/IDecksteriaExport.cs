﻿namespace Decksteria.Core;

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Models;

/// <summary>
/// Represents a valid import command to convert another file type into a Decksteria <see cref="Decklist"/>.
/// </summary>
public interface IDecksteriaExport
{
    /// <summary>
    /// The file extension the export function is designed to handle.
    /// </summary>
    public string FileType { get; }

    /// <summary>
    /// The label displayed to the user for selecting an export option.
    /// </summary>
    public string Label { get; }

    /// <summary>
    /// Converts the <see cref="Decklist"/> into a different file format.
    /// </summary>
    /// <param name="decklist">The decklist created by the user.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the execution.</param>
    /// <returns>A memory stream that contains the structure the file should be saved as.</returns>
    public Task<MemoryStream> SaveDecklistAsync(Decklist decklist, IDecksteriaFormat currentFormat, CancellationToken cancellationToken = default!);
}