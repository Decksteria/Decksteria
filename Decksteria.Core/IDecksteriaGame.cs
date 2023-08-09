namespace Decksteria.Core;

public interface IDecksteriaGame
{
    public string Name { get; }

    public string DisplayName { get; }

    public byte[]? Icon { get; }

    public IEnumerable<IDecksteriaFormat> Formats { get; }

    public IEnumerable<IDecksteriaImport> Importers { get; }

    public IEnumerable<IDecksteriaExport> Exporters { get; }
}