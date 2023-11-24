namespace Decksteria.Core.Data;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IDecksteriaFileReader
{
    /// <summary>
    /// Used by any <see cref="IDecksteriaGame"/> Plug-Ins to read local text files.
    /// </summary>
    /// <param name="fileName">The file that needs to be read. Do not include the path.</param>
    /// <param name="downloadURL">The URL which the raw file can be downloaded from if unavailable.</param>
    /// <returns>The contents of the file requested by the Plug-In as a <see cref="string"/>.</returns>
    public Task<string> ReadTextFileAsync(string fileName, string downloadURL);

    /// <summary>
    /// Used by any <see cref="IDecksteriaGame"/> Plug-Ins to read local binary files.
    /// </summary>
    /// <param name="fileName">The file that needs to be read. Do not include the path.</param>
    /// <param name="downloadURL">The URL which the raw file can be downloaded from if unavailable.</param>
    /// <returns>The binary contents of the file requested by the Plug-In as a <see cref="byte[]"/>.</returns>
    public Task<byte[]> ReadByteFileAsync(string fileName, string downloadURL);

    /// <summary>
    /// Used by any <see cref="IDecksteriaGame"/> Plug-Ins to get the absolute path of a specific file.
    /// </summary>
    /// <param name="fileName">The file whose Absolute Path is being requested.</param>
    /// <param name="downloadURL">The URL which the raw file can be downloaded from if unavailable.</param>
    /// <returns>The absolute path of the file.</returns>
    public string GetFileLocation(string fileName, string downloadURL);

    /// <summary>
    /// Used by any <see cref="IDecksteriaGame"/> Plug-Ins to get a connection string for connecting to a particular database.
    /// </summary>
    /// <param name="fileName">The database file being requested.</param>
    /// <param name="connectionProperties">A list of all other Database Connection Properties to be included in the Connection String.</param>
    /// <param name="downloadURL">The URL which the raw file can be downloaded from if unavailable.</param>
    /// <returns>A connection string used to connect to a database includes the absolute path.</returns>
    public string BuildConnectionString(string fileName, IDictionary<string, string> connectionProperties, string downloadURL);
}
