namespace Decksteria.Ui.Maui.Shared.Models;

using System;
using Decksteria.Core;

public record FormatTile(string GameName, FormatDetails Format)
{
    public string Name { get; init; } = Format.Name;

    public string DisplayName { get; init; } = Format.DisplayName;

    public byte[]? IconImg { get; init; } = Format.IconImg;

    public string? IconSrc { get; init; } = Format.IconSrc;

    public string DeckDirectory { get; init; } = $"{GameName}\\{Format.Name}";
}