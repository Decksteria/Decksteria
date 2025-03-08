namespace Decksteria.Ui.Maui.Services.CardImageService;

using System.Threading;
using System.Threading.Tasks;

public interface IDecksteriaCardImageService
{
    /// <summary>
    /// Downloads the card image and also retrieves its file location.
    /// </summary>
    /// <param name="fileName">The name of the file, not including the file path, but including the file extension.</param>
    /// <param name="downloadURL">The </param>
    /// <param name="md5Checksum"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The absolute path of the image.</returns>
    public Task<string> GetCardImageLocationAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the expected file location for a card image file.
    /// </summary>
    /// <param name="fileName">The name of the file, not including the file path, but including the file extension.</param>
    /// <returns>The absolute path of the image.</returns>
    public string GetExpectedCardImageLocation(string fileName);
}