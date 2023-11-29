namespace Decksteria.Ui.Maui.Shared.Models;

using System;
using Decksteria.Core;

public readonly struct FormatDetails
{
    public FormatDetails(IDecksteriaFormat format)
    {
        Name = format.Name;
        DisplayName = format.DisplayName;

        if (format.Icon != null)
        {
            var imgString = Convert.ToBase64String(format.Icon);
            IconImg = format.Icon;
            IconSrc = $"data:image/png;base64,{imgString}";
        }
    }

    public string Name { get; init; }

    public string DisplayName { get; init; }

    public byte[]? IconImg { get; init; }

    public string? IconSrc { get; init; }
}