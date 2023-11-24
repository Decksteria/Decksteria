namespace Decksteria.Service.DecksteriaPluginService.Strategies;

using Decksteria.Core;

internal interface IDecksteriaGameStrategy : IDecksteriaGame
{
    void ChangePlugIn(IDecksteriaGame? newPlugIn);
}