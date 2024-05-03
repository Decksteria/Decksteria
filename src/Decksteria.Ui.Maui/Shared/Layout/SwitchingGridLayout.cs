namespace Decksteria.Ui.Maui.Shared.Layout;

using Microsoft.Maui.Layouts;
using UraniumUI.Layouts;

internal sealed class SwitchingGridLayout : GridLayout
{
    protected override ILayoutManager CreateLayoutManager()
    {
        return new SwitchingGridLayoutManager(this);
    }
}
