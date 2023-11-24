namespace Decksteria.Core;

/// <summary>
/// A tile used on the application's selection screen.
/// </summary>
public interface IDecksteriaTile
{
    public string Name { get; }

    public string DisplayName { get; }

    public byte[]? Icon { get; }

    public string Description { get; }
}
