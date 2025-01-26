namespace Decksteria.Core.Data;

/// <summary>
/// Details used by the application retrieve an image.
/// </summary>
/// <param name="FileNameWithExtension">The image file to save to and read from. You must include the file extension, but do not include the path.</param>
/// <param name="FileSourceUrl">The URL used by the Decksteria Application to find the image file.</param>
public sealed record DecksteriaImage(string FileNameWithExtension, string FileSourceUrl);