using VictorianMoneyCounter.Model.Aggregates;

namespace VictorianMoneyCounter.Service;

public class BasicCurrencyConverter : ICurrencyConverter
{
    /// <summary>
    /// Currency conversion from smaller to larger denomination
    /// </summary>
    /// <param name="denomination"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public int ConvertUp(Denomination denomination, int amount = 1)
    {
        return amount * denomination switch
        {
            Denomination.Pound => -1, // Probably a better value to use...
            Denomination.Crown => 4,    // 4 crowns convert up to 1 pound
            Denomination.Shilling => 5, // 5 shillings convert up to 1 crown
            Denomination.Penny => 12,   // 12 pence convert up to 1 shilling
            Denomination.Farthing => 4, // 4 farthings convert up to 1 penny            
            _ => -1, // Probably a better value to use...
        };
    }

    /// <summary>
    /// Currency conversion from larger to smaller denomination
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

    public int Convert(Denomination fromDenomination, Denomination toDenomination, int ammount = 1)
    {
        throw new NotImplementedException();
    }
}
