namespace Decksteria.Ui.Maui.Shared.Converter;

using System;
using System.Drawing;
using System.Globalization;
using Microsoft.Maui.Controls;
using UraniumUI.Icons.FontAwesome;

internal sealed class VerifiedToColourConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return Color.Red;
        }
        else if (value is bool boolValue)
        {
            return boolValue ? Color.Green : Color.Red;
        }

        return Color.Green;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Color colorValue)
        {
            return colorValue == Color.LightGreen;
        }

        return false;
    }
}
