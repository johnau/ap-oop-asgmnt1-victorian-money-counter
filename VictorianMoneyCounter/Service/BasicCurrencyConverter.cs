using System.Diagnostics;
using VictorianMoneyCounter.Model.Aggregates;

namespace VictorianMoneyCounter.Service;

/// <summary>
/// This class should be refactored.
/// The switch statements can be shifted out to a static class
/// The typical Converter pattern method is the useful one.
/// </summary>
public class BasicCurrencyConverter : ICurrencyConverter
{
    public (int quantity, int remainderOriginal, int remainderFarthings) Convert(Denomination fromDenomination, Denomination toDenomination, int amount = 1)
    {
        int quantityFarthings = DenominationInfoFactory.ConvertToFarthings(amount, fromDenomination);

        (int targetQuantity, int remainderFarthings) = DenominationInfoFactory.ConvertFromFarthings(quantityFarthings, toDenomination);
        (int remainderOrigignalDenomination, int finalFarthingsRemainder) = DenominationInfoFactory.ConvertFromFarthings(remainderFarthings, fromDenomination);

        if (fromDenomination == Denomination.Farthing)
        {
            finalFarthingsRemainder += remainderOrigignalDenomination;
            remainderOrigignalDenomination = 0;
        }

        return (targetQuantity, remainderOrigignalDenomination, finalFarthingsRemainder);
    }

    public Dictionary<Denomination, int> ConsolidateQuantities(Dictionary<Denomination, int> quantities)
    {
        var consolidatedQuantities = new Dictionary<Denomination, int>();
        var farthings = quantities.Sum(_ => DenominationInfoFactory.ConvertToFarthings(_.Value, _.Key));
        var max = (int)Enum.GetValues<Denomination>().Max(); // Avoiding i = 5

        for (int i = max; i > 1; i--)
        {
            Denomination denomination = (Denomination)i;
            (int quantity, _, farthings) = Convert(Denomination.Farthing, denomination, farthings);
            consolidatedQuantities[denomination] = quantity;
        }

        consolidatedQuantities[Denomination.Farthing] = farthings;

        return consolidatedQuantities;
    }

    /// <summary>
    /// [Convenience method] Currency conversion between adjacent denominations (smaller to larger)
    /// </summary>
    /// <param name="denomination"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public int ConvertUp(Denomination denomination, int amount = 1)
    {
        return amount * denomination switch
        {
            Denomination.Pound => -1,   // Probably a better value to use...
            Denomination.Crown => 4,    // 4 crowns convert up to 1 pound
            Denomination.Shilling => 5, // 5 shillings convert up to 1 crown
            Denomination.Penny => 12,   // 12 pence convert up to 1 shilling
            Denomination.Farthing => 4, // 4 farthings convert up to 1 penny            
            _ => -1,                    // Probably a better value to use...
        };
    }

    /// <summary>
    /// [Convenience method] Currency conversion between adjacent denominations (smaller to larger)
    /// </summary>
    /// <param name="denomination"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public int ConvertDown(Denomination denomination, int amount = 1)
    {
        return amount * denomination switch
        {
            Denomination.Pound => 4,    // 1 pound converts down to 4 crowns
            Denomination.Crown => 5,    // 1 crown converts down to 5 shillings
            Denomination.Shilling => 12,// 1 shilling converts down to 12 pence
            Denomination.Penny => 4,    // 1 penny converts down to 4 farthings
            Denomination.Farthing => 0,
            _ => 0,
        };
    }
}
