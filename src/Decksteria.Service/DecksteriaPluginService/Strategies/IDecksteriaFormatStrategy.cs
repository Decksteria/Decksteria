namespace Decksteria.Service.DecksteriaPluginService.Strategies;

using Decksteria.Core;

internal interface IDecksteriaFormatStrategy : IDecksteriaFormat
{
    public void ChangeFormat(IDecksteriaFormat? newFormat);
}