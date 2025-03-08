namespace Decksteria.Ui.Maui.Services.FileLocator;

using System.Collections.Generic;

internal interface IDecksteriaFileLocator
{
    /// <summary>
    /// Gets a list of all directories that are used to store card images.
    /// </summary>
    /// <returns>A list of all directories used to store card images.</returns>
    public IEnumerable<string> GetAllCardImageDirectories();

    /// <summary>
    /// Get the expected location of a card image based on its file name.
    /// </summary>
    /// <param name="fileName">The file name for the image.</param>
    /// <returns>The expected absolute path for the image.</returns>
    public string GetExpectedCardImageLocation(string fileName);

    /// <summary>
    /// Get the expected location of a file based on its file name.
    /// </summary>
    /// <param name="fileName">The file name for the file.</param>
    /// <returns>The expected absolute path for the file.</returns>
    public string GetExpectedFileLocation(string fileName);

    /// <summary>
    /// Get the expected location of an image based on its file name.
    /// </summary>
    /// <param name="fileName">The file name for the image.</param>
    /// <returns>The expected absolute path for the image.</returns>
    public string GetExpectedImageLocation(string fileName);
}