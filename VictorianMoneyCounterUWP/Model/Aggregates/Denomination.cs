using System;

namespace VictorianMoneyCounterUWP.Model.Aggregates
{

    /// <summary>
    /// Denominations with int ordering
    /// More useful values could probably have been used
    /// </summary>
    public enum Denomination
    {
        Pound = 5,
        Crown = 4,
        Shilling = 3,
        Penny = 2,
        Farthing = 1
    }

    /// <summary>
    /// Record to extend Denomination Enum
    /// </summary>
    /// <param name="Denomination"></param>
    /// <param name="Singular"></param>
    /// <param name="Plural"></param>
    /// <param name="Symbol"></param>
    public class DenominationInfo
    {
        private readonly int Denomination;
        private readonly string Singular;
        private readonly string Plural;
        private readonly char Symbol;

        public DenominationInfo(int denomination, string singular, string plural, char symbol)
        {
            Denomination = denomination;
            Singular = singular;
            Plural = plural;
            Symbol = symbol;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class DenominationValue
    {
        /// <summary>
        /// Helper method for Denomination Enum (since C# does not seem to allow complex Enums)
        /// </summary>
        /// <param name="denomination"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static DenominationInfo GetDenominationInfo(Denomination denomination)
        {
            switch (denomination)
            {
                case Denomination.Pound:
                    return new DenominationInfo((int)denomination,
                                                denomination.ToString().ToLower(),
                                                denomination.ToString().ToLower() + "s",
                                                '£');
                case Denomination.Crown:
                case Denomination.Shilling:
                case Denomination.Farthing:
                    return new DenominationInfo((int)denomination,
                                                denomination.ToString().ToLower(),
                                                denomination.ToString().ToLower() + "s",
                                                denomination.ToString().ToLower()[0]);
                case Denomination.Penny:
                    return new DenominationInfo((int)denomination,
                                                denomination.ToString().ToLower(),
                                                "pence",
                                                denomination.ToString().ToLower()[0]);

                default:
                    throw new ArgumentException("Invalid Denominiation");
            }
        }

        /// <summary>
        /// Value of given amount of source Denomination in Farthings
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int ValueInFarthings(int amount, Denomination source)
        {
            // 1 pound = 4 crowns
            // 1 crown = 5 shillings
            // 1 shilling = 12 pence
            // 1 penny = 4 farthings

            switch (source)
            {
                case Denomination.Pound:
                    return 4 * 5 * 12 * 4;
                case Denomination.Crown:
                    return 5 * 12 * 4;
                case Denomination.Shilling:
                    return 12 * 4;
                case Denomination.Penny:
                    return 4;
                case Denomination.Farthing:
                    return 1;
                default:
                    throw new ArgumentException("Invalid source Denominiation");
            }   
        }

        /// <summary>
        /// Value of given amount of Farthings in target denomination. 
        /// Returns quantity of target denomination and remainder in Farthings
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static (int WholeNumber, int RemainderFarthings) ValueOfFarthings(int amount, Denomination target)
        {
            int wholeNumber;
            int remainder;
            // 1 pound = 4 crowns
            // 1 crown = 5 shillings
            // 1 shilling = 12 pence
            // 1 penny = 4 farthings
            switch (target)
            {
                case Denomination.Pound:
                    // £1 = 4c * 5s * 12p * 4f
                    wholeNumber = amount / (4 * 5 * 12 * 4);
                    remainder = amount % (4 * 5 * 12 * 4);
                    break;
                case Denomination.Crown:
                    // 1c = 5s * 12p * 4f
                    wholeNumber = amount / (5 * 12 * 4);
                    remainder = amount % (5 * 12 * 4);
                    break;
                case Denomination.Shilling:
                    // 1s = 12p * 4f
                    wholeNumber = amount / (12 * 4);
                    remainder = amount % (12 * 4);
                    break;
                case Denomination.Penny:
                    // 1p = 4f
                    wholeNumber = amount / 4;
                    remainder = amount % 4;
                    break;
                case Denomination.Farthing:
                    wholeNumber = amount;
                    remainder = 0;
                    break;
                default:
                    throw new ArgumentException("Invalid target denomination");
            }

            return (wholeNumber, remainder);
        }
    }
}
