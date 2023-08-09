namespace Decksteria.Core;

using Decksteria.Core.Models;

public interface IDecksteriaImport
{
    public string Name { get; }

    public string FileType { get; }

    public string Label { get; }

    public Decklist LoadDecklist(MemoryStream memoryStream);
}