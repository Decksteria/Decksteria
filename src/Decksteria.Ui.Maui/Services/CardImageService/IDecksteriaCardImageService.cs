namespace Decksteria.Ui.Maui.Services.CardImageService;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// A service used for downloading and retrieving card images.
/// </summary>
public interface IDecksteriaCardImageService
{
    /// <summary>
    /// Deletes all images downloaded by the application.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used to cancel the execution.</param>
    /// <returns></returns>
    public Task DeleteAllImagesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads the card image and also retrieves its file location.
    /// </summary>
    /// <param name="fileName">The name of the file, not including the file path, but including the file extension.</param>
    /// <param name="downloadURL">The URL of which to download the image from.</param>
    /// <param name="md5Checksum">A checksum to check if the image should be re-downloaded.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the execution.</param>
    /// <returns>The absolute path of the image.</returns>
    public Task<string> GetCardImageLocationAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the expected file location for a card image file.
    /// </summary>
    /// <param name="fileName">The name of the file, not including the file path, but including the file extension.</param>
    /// <returns>The absolute path of the image.</returns>
    public string GetExpectedCardImageLocation(string fileName);
}