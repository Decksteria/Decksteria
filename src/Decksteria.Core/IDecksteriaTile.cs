namespace Decksteria.Core;

/// <summary>
/// A tile used on the application's selection screen.
/// </summary>
public interface IDecksteriaTile
{
    /// <summary>
    /// The name to be displayed to the user.
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// The icon to display when showing the plug-in or format to the user.
    /// </summary>
    public byte[]? Icon { get; }

    /// <summary>
    /// A brief description of the plug-in or format to the user.
    /// </summary>
    public string Description { get; }
}
