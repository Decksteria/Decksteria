namespace Decksteria.Core.Data;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// An implementation to read files and download them for use by the application.
/// </summary>
public interface IDecksteriaFileReader
{
    /// <summary>
    /// Used by any <see cref="IDecksteriaGame"/> plug-ins to get a connection string for connecting to a particular (local) database.
    /// </summary>
    /// <param name="fileName">The name of the file that the plug-in needs to be read. Do not include a path.</param>
    /// <param name="connectionProperties">A list of all other database connection properties to be included in the connection string.</param>
    /// <param name="downloadURL">The URL which the raw file can be downloaded from if unavailable.</param>
    /// <param name="md5Checksum">The MD5 checksum that the file should resolve to. If not parsed in, it will presume it was always downloaded correctly.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the execution.</param>
    /// <returns>A connection string used to connect to a database includes the absolute path.</returns>
    public string BuildConnectionString(string fileName, IDictionary<string, string> connectionProperties, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the expected file location for a generic plug-in file.
    /// </summary>
    /// <param name="fileName">The name of the file, not including the file path, but including the file extension.</param>
    /// <returns>The absolute path of the file.</returns>
    public string GetExpectedFileLocation(string fileName);

    /// <summary>
    /// Gets the expected file location for an image file.
    /// </summary>
    /// <param name="fileName">The name of the file, not including the file path, but including the file extension.</param>
    /// <returns>The absolute path of the image.</returns>
    public string GetExpectedImageLocation(string fileName);

    /// <summary>
    /// Used by any <see cref="IDecksteriaGame"/> plug-ins to read local binary files.
    /// </summary>
    /// <param name="fileName">The name of the file that the plug-in needs to be read. Do not include a path.</param>
    /// <param name="downloadURL">The URL which the raw file can be downloaded from if unavailable.</param>
    /// <param name="md5Checksum">The MD5 checksum that the file should resolve to. If not parsed in, it will presume it was always downloaded correctly.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the execution.</param>
    /// <returns>The binary contents of the file requested by the Plug-In as a <see cref="byte[]"/>.</returns>
    public Task<byte[]> ReadByteFileAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Used by any <see cref="IDecksteriaGame"/> plug-ins to read the image files of available cards in the Plug-In.
    /// </summary>
    /// <param name="fileName">The name of the file that the plug-in needs to be read. Do not include a path.</param>
    /// <param name="downloadURL">The URL which the raw file can be downloaded from if unavailable.</param>
    /// <param name="md5Checksum">The MD5 checksum that the file should resolve to. If not parsed in, it will presume it was always downloaded correctly.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the execution.</param>
    /// <returns></returns>
    public Task<byte[]> ReadImageAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reads a http request as a string. Meant to be used for getting the checksum of a file from an online endpoint.
    /// </summary>
    /// <param name="URL">The URL endpoint which the text will be read from.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the execution.</param>
    /// <returns>The string provided by the URL, will return null if it failed to read from the URL for one reason or another.</returns>
    public Task<string?> ReadOnlineTextAsync(string URL, CancellationToken cancellationToken = default);

    /// <summary>
    /// Used by any <see cref="IDecksteriaGame"/> plug-ins to read local text files.
    /// </summary>
    /// <param name="fileName">The name of the file that the plug-in needs to be read. Do not include a path.</param>
    /// <param name="downloadURL">The URL which the raw file can be downloaded from if unavailable.</param>
    /// <param name="md5Checksum">The MD5 checksum that the file should resolve to. If not parsed in, it will presume it was always downloaded correctly.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the execution.</param>
    /// <returns>The contents of the file requested by the Plug-In as a <see cref="string"/>.</returns>
    public Task<string> ReadTextFileAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default);
}
