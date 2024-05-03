namespace Decksteria.Ui.Maui.Shared.Layout;

using System.Diagnostics;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

internal class SwitchingGridLayoutManager(SwitchingGridLayout switchingGridLayout) : GridLayoutManager(switchingGridLayout)
{
    private readonly SwitchingGridLayout _layout = switchingGridLayout;

    public override Size ArrangeChildren(Rect bounds)
    {
        var padding = _layout.Padding;
        var top = bounds.Top + padding.Top;
        var left = bounds.Left + padding.Left;
        var orientation = Grid.Width < Grid.Height ? DisplayOrientation.Portrait : DisplayOrientation.Landscape;

        var rows = _layout.RowCount;
        var columns = _layout.ColumnCount;

        // Calculate Dimensions for each cell
        var matrix = new Rect[rows][];

        var x = left;
        for (var i = 0; i < rows; i++)
        {
            matrix[i] = new Rect[columns];
            var rowWidth = Grid.ColumnDefinitions[i].Width.Value;

            var y = top;
            for (var j = 0; j < columns; j++)
            {
                var columnHeight = Grid.RowDefinitions[j].Height.Value;
                matrix[i][j] = new Rect(x, y, rowWidth, columnHeight);
                y += columnHeight;
            }

            x += rowWidth;
        }

        foreach (var child in _layout.Children)
        {
            // Reverse Row and Column if Landscape
            var (childRow, childColumn) = orientation switch
            {
                DisplayOrientation.Landscape => (_layout.GetRow(child), _layout.GetColumn(child)),
                DisplayOrientation.Portrait => (_layout.GetColumn(child), _layout.GetRow(child)),
                _ => throw new UnreachableException("SwitchingGridLayout Orientation was not defined.")
            };
            child.Arrange(matrix[childRow][childColumn]);
        }

        return new Size(Grid.Width, Grid.Height);
    }
}
