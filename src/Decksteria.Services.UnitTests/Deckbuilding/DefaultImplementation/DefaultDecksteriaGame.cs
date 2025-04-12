namespace Decksteria.Services.UnitTests.Deckbuilding.DefaultImplementation;

using System.Collections.Generic;
using Decksteria.Core;


internal sealed class DefaultDecksteriaGame : IDecksteriaGame
{
    public IEnumerable<IDecksteriaFormat> Formats {get;set;} = [new DefaultDecksteriaFormat()];

    public IEnumerable<IDecksteriaImport> Importers => [];

    public IEnumerable<IDecksteriaExport> Exporters => [];

    public string DisplayName => "Default Game";

    public byte[]? Icon => null;

    public string Description => string.Empty;
}
