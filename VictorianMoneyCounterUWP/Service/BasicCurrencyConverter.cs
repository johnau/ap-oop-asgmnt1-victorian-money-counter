using System;
using System.Collections.Generic;
using System.Linq;
using VictorianMoneyCounterUWP.Model.Aggregates;

namespace VictorianMoneyCounterUWP.Service
{

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

            var farthings = quantities.Sum(_ => DenominationValue.ValueInFarthings(_.Value, _.Key)); // get total value of all denominations in farthings
            
            int max = 0;
            foreach (var v in Enum.GetValues(typeof(Denomination)))
            {
                max = Math.Max(max, (int)v);
            }

            //for (int i = max; i >= 2; i--) // loop denominations besides last - does not process farthings (i > 1)
            for (Denomination denomination = Denomination.Pound; denomination >= Denomination.Penny; denomination--)
            {
                (int quantity, int _, int farthingsRemainder) = Convert(Denomination.Farthing, denomination, farthings);
                farthings = farthingsRemainder;
                consolidatedQuantities[denomination] = quantity;
            }

            consolidatedQuantities[Denomination.Farthing] = farthings;
            return consolidatedQuantities;
        }

        /// <summary>
        /// [Convenience method] Currency conversion cost between adjacent denominations (smaller to larger)
        /// 
        /// Note: Renamed to CostToConvertUp as it did not make sense,
        /// TODO: Reimplement ConvertUp method for completeness
        /// 
        /// </summary>
        /// <param name="denomination"></param>
        /// <param name="amount"></param>
        /// <returns>A positive number of quantity required to convert up, else a negative number</returns>
        public int CostToConvertUp(Denomination denomination, int amount = 1)
        {
            //return amount * denomination switch
            //{
            //    Denomination.Pound => -1,   // Probably a better value to use...
            //    Denomination.Crown => 4,    // 4 crowns convert up to 1 pound
            //    Denomination.Shilling => 5, // 5 shillings convert up to 1 crown
            //    Denomination.Penny => 12,   // 12 pence convert up to 1 shilling
            //    Denomination.Farthing => 4, // 4 farthings convert up to 1 penny            
            //    _ => -1,                    // Probably a better value to use...
            //};


            switch (denomination)
            {
                case Denomination.Pound:
                    return -1;
                case Denomination.Crown:
                    return 4;
                case Denomination.Shilling:
                    return 5;
                case Denomination.Penny:
                    return 12;
                case Denomination.Farthing:
                    return -1;
                default:
                    throw new ArgumentException("Invalid Denominiation");
            }
        }

        /// <summary>
        /// [Convenience method] Currency conversion between adjacent denominations (smaller to larger)
        /// </summary>
        /// <param name="denomination"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public int ConvertDown(Denomination denomination, int amount = 1)
        {
            switch (denomination)
            {
                case Denomination.Pound:
                    return 4;
                case Denomination.Crown:
                    return 5;
                case Denomination.Shilling:
                    return 12;
                case Denomination.Penny:
                    return 4;
                case Denomination.Farthing:
                    return 0;
                default:
                    throw new ArgumentException("Invalid Denominiation");
            }

            //return amount * denomination switch
            //{
            //    Denomination.Pound => 4,    // 1 pound converts down to 4 crowns
            //    Denomination.Crown => 5,    // 1 crown converts down to 5 shillings
            //    Denomination.Shilling => 12,// 1 shilling converts down to 12 pence
            //    Denomination.Penny => 4,    // 1 penny converts down to 4 farthings
            //    Denomination.Farthing => 0,
            //    _ => 0,
            //};
        }
    }

}
