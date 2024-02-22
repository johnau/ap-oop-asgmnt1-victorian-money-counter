using VictorianMoneyCounter.Model.Aggregates;

namespace VictorianMoneyCounter.Service
{
    public interface ICurrencyConverter
    {
        /// <summary>
        /// Convert between any two denominations
        /// </summary>
        /// <param name="fromDenomination"></param>
        /// <param name="toDenomination"></param>
        /// <param name="ammount"></param>
        /// <returns></returns>
        (int quantity, int remainderOriginal, int remainderFarthings) Convert(Denomination fromDenomination, Denomination toDenomination, int amount = 1);

        /// <summary>
        /// Use to consolidate values to highest denominations possible
        /// </summary>
        /// <param name="quantities"></param>
        /// <returns></returns>
        Dictionary<Denomination, int> ConsolidateQuantities(Dictionary<Denomination, int> quantities);

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
        int CostToConvertUp(Denomination denomination, int amount = 1);
    }
}