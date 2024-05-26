namespace Decksteria.Ui.Maui.Shared.Pages;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

public interface IModalPage<T> where T : Page
{
    Func<T, CancellationToken, Task>? OnPopAsync { get; set; }
}
