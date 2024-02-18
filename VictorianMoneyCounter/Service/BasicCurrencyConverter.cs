using VictorianMoneyCounter.Model.Aggregates;

namespace VictorianMoneyCounter.Service;

/// <summary>
/// CurrencyConvert implementation to convert denominations between one another
/// </summary>
public class BasicCurrencyConverter : ICurrencyConverter
{
    /// <summary>
    /// Converts from source Denomination to target Denomination, with the given amount.
    /// Converts to lowest common denominator and then to the target Denomination
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public (int quantity, int remainderOriginal, int remainderFarthings) Convert(Denomination source, Denomination target, int amount = 1)
    {
        int quantityFarthings = DenominationValue.ValueInFarthings(amount, source);

        (int targetQuantity, int remainderFarthings) = DenominationValue.ValueOfFarthings(quantityFarthings, target);
        (int remainderOrigignalDenomination, int finalFarthingsRemainder) = DenominationValue.ValueOfFarthings(remainderFarthings, source);

        if (source == Denomination.Farthing)
        {
            finalFarthingsRemainder += remainderOrigignalDenomination;
            remainderOrigignalDenomination = 0;
        }

        return (targetQuantity, remainderOrigignalDenomination, finalFarthingsRemainder);
    }

    /// <summary>
    /// Consolidate denomination quantities into smallest amount of coins
    /// </summary>
    /// <param name="quantities"></param>
    /// <returns></returns>
    public Dictionary<Denomination, int> ConsolidateQuantities(Dictionary<Denomination, int> quantities)
    {
        var consolidatedQuantities = new Dictionary<Denomination, int>();
        var farthings = quantities.Sum(_ => DenominationValue.ValueInFarthings(_.Value, _.Key));
        var max = (int)Enum.GetValues<Denomination>().Max(); // Avoiding i = 5

        //for (int i = max; i >= 2; i--) // loop denominations besides last - does not process farthings (i > 1)
        for (Denomination denomination = Denomination.Pound; denomination >= Denomination.Penny; denomination--)
        {
            (int quantity, _, farthings) = Convert(Denomination.Farthing, denomination, farthings);
            consolidatedQuantities[denomination] = quantity;
        }

        consolidatedQuantities[Denomination.Farthing] = farthings;
        return consolidatedQuantities;
    }

    /// <summary>
    /// [Convenience method] Currency conversion between adjacent denominations (smaller to larger)
    /// 
    /// TODO: Flip the logic of this method, it returns the number of the denomination required to convert up,
    /// rather than returning the quantity of the converted denomination (can then return 0 instead of -1)
    /// and logic will align with other method; "ConvertDown()"
    /// 
    /// </summary>
    /// <param name="denomination"></param>
    /// <param name="amount"></param>
    /// <returns>A positive number of quantity required to convert up, else a negative number</returns>
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
