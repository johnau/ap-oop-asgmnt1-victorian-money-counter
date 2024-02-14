namespace VictorianMoneyCounter.Model.Aggregates;

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
public record DenominationInfo(int Denomination, string Singular, string Plural, char Symbol) { }

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
    public static DenominationInfo GetDenominationInfo(Denomination denomination) => denomination switch
    {
        Denomination.Pound => new DenominationInfo((int)denomination,
                                                        denomination.ToString().ToLower(),
                                                        denomination.ToString().ToLower() + "s",
                                                        '£'
        ),
        Denomination.Crown or
        Denomination.Shilling or
        Denomination.Farthing => new DenominationInfo((int)denomination,
                                                        denomination.ToString().ToLower(), 
                                                        denomination.ToString().ToLower() + "s",
                                                        denomination.ToString().ToLower()[0]
        ),
        Denomination.Penny => new DenominationInfo((int)denomination,
                                                        denomination.ToString().ToLower(), 
                                                        "pence",
                                                        denomination.ToString().ToLower()[0]
        ),
        _ => throw new ArgumentException("Unrecognized Denomination"),
    };

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
        return amount * source switch
        {
            Denomination.Pound => 4 * 5 * 12 * 4,  // 4c * 5s * 12p * 4f
            Denomination.Crown => 5 * 12 * 4,      // 5s * 12p * 4f
            Denomination.Shilling => 12 * 4,       // 12p * 4f
            Denomination.Penny => 4,               // 4f
            Denomination.Farthing => 1,
            _ => throw new ArgumentException("Invalid source denomination"),
        };
    }

    /// <summary>
    /// Value of given amount of Farthings in target denomination
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static (int WholeNumber, int Remainder) ValueOfFarthings(int amount, Denomination target)
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