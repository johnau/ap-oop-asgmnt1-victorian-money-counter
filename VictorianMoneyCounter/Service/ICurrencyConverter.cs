using VictorianMoneyCounter.Model.Aggregates;

namespace VictorianMoneyCounter.Service
{
    public interface ICurrencyConverter
    {
        /// <summary>
        /// Convert to the next higher denomination
        /// </summary>
        /// <param name="denomination"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        int ConvertDown(Denomination denomination, int amount = 1);

        /// <summary>
        /// Convert to the next lower denomination
        /// </summary>
        /// <param name="denomination"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        int ConvertUp(Denomination denomination, int amount = 1);

        /// <summary>
        /// Convert between any two denominations
        /// </summary>
        /// <param name="fromDenomination"></param>
        /// <param name="toDenomination"></param>
        /// <param name="ammount"></param>
        /// <returns></returns>
        int Convert(Denomination fromDenomination, Denomination toDenomination, int ammount = 1);
    }
}