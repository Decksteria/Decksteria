namespace Decksteria.Ui.Maui.Shared.Layout;

using System.ComponentModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

internal sealed class AdaptiveGrid : Grid
{
    public int RowCount
    {
        get => (int) GetValue(RowCountProperty);
        set => SetValue(RowCountProperty, value);
    }

    public static readonly BindableProperty RowCountProperty = BindableProperty.Create(
        nameof(RowCount), typeof(int), typeof(AdaptiveGrid),
        defaultValue: 2,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is AdaptiveGrid grid)
            {
                grid.SetRowDefinitions(!IsLandscape(grid.Width, grid.Height, grid.DimensionSwitchSize, grid.Orientation));
            }
        });

    public int ColumnCount
    {
        get => (int) GetValue(ColumnCountProperty);
        set => SetValue(ColumnCountProperty, value);
    }

    public static readonly BindableProperty ColumnCountProperty = BindableProperty.Create(
        nameof(ColumnCount), typeof(int), typeof(AdaptiveGrid),
        defaultValue: 2,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is AdaptiveGrid grid)
            {
                grid.SetColumnDefinitions(IsLandscape(grid.Width, grid.Height, grid.DimensionSwitchSize, grid.Orientation));
            }
        });

    /// <summary>
    /// If the orientation is horizontal by default, it will switch to a vertical Grid orientation if the width is smaller than this property.
    /// If the orientation is vertical by default, it will switch to a horizontal Grid orientation if the height is smaller than this property.
    /// </summary>
    public double DimensionSwitchSize
    {
        get => (double) GetValue(DimensionSwitchSizeProperty);
        set => SetValue(DimensionSwitchSizeProperty, value);
    }

    /// <summary>Bindable property for <see cref="DimensionSwitchSize"/>.</summary>
    public static readonly BindableProperty DimensionSwitchSizeProperty = BindableProperty.Create(nameof(DimensionSwitchSize), typeof(double),
        typeof(Grid), 500d, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is AdaptiveGrid grid)
            {
                grid.SetColumnDefinitions(IsLandscape(grid.Width, grid.Height, grid.DimensionSwitchSize, grid.Orientation));
                grid.SetRowDefinitions(!IsLandscape(grid.Width, grid.Height, grid.DimensionSwitchSize, grid.Orientation));
            }
        });

    public StackOrientation Orientation
    {
        get => (StackOrientation) GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>Bindable property for <see cref="Orientation"/>.</summary>
    public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation),
        typeof(StackLayout), StackOrientation.Horizontal, propertyChanged:  (bindable, oldValue, newValue) =>
        {
            if (bindable is AdaptiveGrid grid)
            {
                grid.InvalidateMeasure();
            }
        });

    protected override ILayoutManager CreateLayoutManager()
    {
        return base.CreateLayoutManager();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        var isLandscape = IsLandscape(width, height, DimensionSwitchSize, Orientation);
        SetColumnDefinitions(!isLandscape);
        SetRowDefinitions(isLandscape);
        base.OnSizeAllocated(width, height);
    }

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

    [TypeConverter(typeof(GridLengthTypeConverter))]
    public GridLength ColumnGridLength { get => (GridLength) GetValue(ColumnGridLengthProperty); set => SetValue(ColumnGridLengthProperty, value); }

    public static readonly BindableProperty ColumnGridLengthProperty = BindableProperty.Create(
        nameof(ColumnGridLength), typeof(GridLength), typeof(AdaptiveGrid), GridLength.Auto,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is AdaptiveGrid grid)
            {
                grid.SetColumnDefinitions(!IsLandscape(grid.Width, grid.Height, grid.DimensionSwitchSize, grid.Orientation));
            }
        });

    [TypeConverter(typeof(GridLengthTypeConverter))]
    public GridLength RowGridLength { get => (GridLength) GetValue(RowGridLengthProperty); set => SetValue(RowGridLengthProperty, value); }

    public static readonly BindableProperty RowGridLengthProperty = BindableProperty.Create(
        nameof(RowGridLength), typeof(GridLength), typeof(AdaptiveGrid), GridLength.Auto,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is AdaptiveGrid grid)
            {
                grid.SetRowDefinitions(IsLandscape(grid.Width, grid.Height, grid.DimensionSwitchSize, grid.Orientation));
            }
        });

    private static bool IsLandscape(double width, double height, double switchSize, StackOrientation stackOrientation)
    {
        return stackOrientation switch
        {
            StackOrientation.Vertical => height < switchSize,
            _ => width > switchSize
        };
    }
}
