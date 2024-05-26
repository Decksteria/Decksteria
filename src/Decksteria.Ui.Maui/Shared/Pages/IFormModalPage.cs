namespace Decksteria.Ui.Maui.Shared.Pages;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

public interface IFormModalPage<T> : IModalPage<T> where T : Page
{
    bool IsSubmitted { get; }

    Func<T, CancellationToken, Task>? OnSubmitAsync { get; set; }
}
