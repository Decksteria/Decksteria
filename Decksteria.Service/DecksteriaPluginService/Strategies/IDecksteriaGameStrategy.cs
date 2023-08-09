namespace Decksteria.Service.DecksteriaPluginService.Strategies;

using Decksteria.Core;
using System.Collections.Generic;

internal interface IDecksteriaGameStrategy : IDecksteriaGame
{
    void ChangePlugIn(IDecksteriaGame? newPlugIn);
}