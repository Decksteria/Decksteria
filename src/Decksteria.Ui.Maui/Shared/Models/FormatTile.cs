namespace Decksteria.Ui.Maui.Shared.Models;

using Decksteria.Services.PlugInFactory.Models;

public record FormatTile(string GameName, FormatDetails Format)
{
    public string Name { get; init; } = Format.Name;

    public string DisplayName { get; init; } = Format.DisplayName;

    public byte[]? IconImg { get; init; } = Format.IconImg;

    public string? IconSrc { get; init; } = Format.IconSrc;

    public string DeckDirectory { get; init; } = $"{GameName}\\{Format.Name}";
}