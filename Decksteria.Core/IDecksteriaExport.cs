namespace Decksteria.Core;

using Decksteria.Core.Models;

public interface IDecksteriaExport
{
    public string Name { get; }

    public string FileType { get; }

    public string Label { get; }

    public MemoryStream SaveDecklist(Decklist decklist);
}