namespace Decksteria.Core;

public interface IDecksteriaGame : IDecksteriaTile
{
    public IEnumerable<IDecksteriaFormat> Formats { get; }

    public IEnumerable<IDecksteriaImport> Importers { get; }

    public IEnumerable<IDecksteriaExport> Exporters { get; }
}