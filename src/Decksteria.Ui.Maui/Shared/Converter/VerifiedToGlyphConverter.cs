namespace Decksteria.Ui.Maui.Shared.Converter;

using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using UraniumUI.Icons.FontAwesome;

internal sealed class VerifiedToGlyphConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return Solid.CircleXmark;
        }
        else if (value is bool boolValue)
        {
            return boolValue ? Regular.CircleCheck : Solid.CircleXmark;
        }

        return Regular.CircleCheck;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString() == Regular.CircleCheck;
    }
}
