namespace Decksteria.Ui.Maui.Shared.Converter;

using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

internal sealed class VerifiedToColourConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return Colors.Red;
        }
        else if (value is bool boolValue)
        {
            return boolValue ? Colors.Green : Colors.Red;
        }

        return Colors.Green;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Color colorValue)
        {
            return colorValue == Colors.LightGreen;
        }

        return false;
    }
}
