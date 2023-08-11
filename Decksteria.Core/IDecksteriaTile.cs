namespace Decksteria.Core;

public interface IDecksteriaTile
{
    public string Name { get; }

    public string DisplayName { get; }

    public byte[]? Icon { get; }

    public string Description { get; }
}
