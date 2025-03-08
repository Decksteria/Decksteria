namespace Decksteria.Ui.Maui.Shared.Layouts;

using System.ComponentModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

internal sealed class AdaptiveGrid : Grid
{
    public int RowCount
    {
        get => (int) GetValue(RowCountProperty);
        set => SetValue(RowCountProperty, value);
    }

    public static readonly BindableProperty RowCountProperty = BindableProperty.Create(
        nameof(RowCount),
        typeof(int),
        typeof(AdaptiveGrid),
        defaultValue: 2,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is AdaptiveGrid grid)
            {
                grid.SetRowDefinitions(!grid.HorizontalDisplay);
            }
        });

    public int ColumnCount
    {
        get => (int) GetValue(ColumnCountProperty);
        set => SetValue(ColumnCountProperty, value);
    }

    public static readonly BindableProperty ColumnCountProperty = BindableProperty.Create(
        nameof(ColumnCount),
        typeof(int),
        typeof(AdaptiveGrid),
        defaultValue: 2,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is AdaptiveGrid grid)
            {
                grid.SetColumnDefinitions(grid.HorizontalDisplay);
            }
        });

    /// <summary>
    /// Switches to a vertical Grid orientation if the width is smaller than this property.
    /// </summary>
    public double OrientationSwitchWidth
    {
        get => (double) GetValue(OrientationSwitchWidthProperty);
        set => SetValue(OrientationSwitchWidthProperty, value);
    }

    /// <summary>Bindable property for <see cref="OrientationSwitchWidth"/>.</summary>
    public static readonly BindableProperty OrientationSwitchWidthProperty = BindableProperty.Create(
        nameof(OrientationSwitchWidth),
        typeof(double),
        typeof(Grid),
        500d,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is AdaptiveGrid grid)
            {
                grid.SetColumnDefinitions(grid.HorizontalDisplay);
                grid.SetRowDefinitions(!grid.HorizontalDisplay);
            }
        });

    public bool HorizontalDisplay => IsLandscape(Width, OrientationSwitchWidth);

    [TypeConverter(typeof(GridLengthTypeConverter))]
    public GridLength ColumnGridLength
    {
        get => (GridLength) GetValue(ColumnGridLengthProperty);
        set => SetValue(ColumnGridLengthProperty, value);
    }

    public static readonly BindableProperty ColumnGridLengthProperty = BindableProperty.Create(
        nameof(ColumnGridLength),
        typeof(GridLength),
        typeof(AdaptiveGrid),
        GridLength.Auto,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is AdaptiveGrid grid)
            {
                grid.SetColumnDefinitions(!grid.HorizontalDisplay);
            }
        });

    [TypeConverter(typeof(GridLengthTypeConverter))]
    public GridLength RowGridLength
    {
        get => (GridLength) GetValue(RowGridLengthProperty);
        set => SetValue(RowGridLengthProperty, value);
    }

    public static readonly BindableProperty RowGridLengthProperty = BindableProperty.Create(
        nameof(RowGridLength),
        typeof(GridLength),
        typeof(AdaptiveGrid),
        GridLength.Auto,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is AdaptiveGrid grid)
            {
                grid.SetRowDefinitions(grid.HorizontalDisplay);
            }
        });

    protected override void OnSizeAllocated(double width, double height)
    {
        var isLandscape = IsLandscape(width, OrientationSwitchWidth);
        SetColumnDefinitions(!isLandscape);
        SetRowDefinitions(isLandscape);
        base.OnSizeAllocated(width, height);
    }

    private static bool IsLandscape(double width, double switchSize) => width > switchSize;

    private void SetColumnDefinitions(bool isPortrait)
    {
        ColumnDefinitions.Clear();
        if (isPortrait)
        {
            return;
        }

        for (var i = 0; i < ColumnCount; i++)
        {
            ColumnDefinitions.Add(new ColumnDefinition(ColumnGridLength));
        }
    }

    private void SetRowDefinitions(bool isLandscape)
    {
        RowDefinitions.Clear();
        if (isLandscape)
        {
            return;
        }

        for (var i = 0; i < RowCount; i++)
        {
            RowDefinitions.Add(new RowDefinition(RowGridLength));
        }
    }
}
