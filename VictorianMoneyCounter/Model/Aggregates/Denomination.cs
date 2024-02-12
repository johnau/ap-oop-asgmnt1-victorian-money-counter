namespace VictorianMoneyCounter.Model.Aggregates;

public enum Denomination
{
    Pound = 1,
    Crown = 2,
    Shilling = 3,
    Penny = 4,
    Farthing = 5
}

public readonly struct DenominationInfo(int index, string singular, string plural)
{
    public int Index { get; } = index;
    public string Singular { get; } = singular;
    public string Plural { get; } = plural;
}

public static class DenominationInfoFactory
{
    public static DenominationInfo GetDenominationInfo(Denomination denomination)
    {
        switch (denomination)
        {
            case Denomination.Pound:
            case Denomination.Crown:
            case Denomination.Shilling:
            case Denomination.Farthing:
                return new DenominationInfo((int)denomination, denomination.ToString().ToLower(), denomination.ToString().ToLower() + "s");
            case Denomination.Penny:
                return new DenominationInfo((int)denomination, "penny", "pence");
            default:
                throw new ArgumentException("Unrecognized Denomination");

        }
    }
    /// <summary>
    /// 1 pound = 4 crowns
    /// 1 crown = 5 shillings
    /// 1 shilling = 12 pence
    /// 1 penny = 4 farthings
    /// </summary>
    /// <param name="denomination"></param>
    /// <returns>The cost of converting up the current denomination</returns>
    /// <exception cref="ArgumentException"></exception>
    public static int ConvertDenominationUp(Denomination denomination)
    {
        switch (denomination)
        {
            case Denomination.Pound:
                return 9999;
            case Denomination.Crown:
                return 4; // 4 crowns convert up to 1 pound
            case Denomination.Shilling:
                return 5; // 5 shillings convert up to 1 crown
            case Denomination.Penny:
                return 12; // 12 pence convert up to 1 shilling
            case Denomination.Farthing:
                return 4; // 4 farthings convert up to 1 penny            
            default:
                return 9999;
                //throw new ArgumentException("Unrecognized Denomination");

        }
    }

    /// <summary>
    /// 1 pound = 4 crowns
    /// 1 crown = 5 shillings
    /// 1 shilling = 12 pence
    /// 1 penny = 4 farthings
    /// </summary>
    /// <param name="denomination"></param>
    /// <returns>The ammount of the smaller denomination received when converting down</returns>
    /// <exception cref="ArgumentException"></exception>
    public static int ConvertDenominationDown(Denomination denomination)
    {
        switch (denomination)
        {
            case Denomination.Pound:
                return 4; // 1 pound converts down to 4 crowns
            case Denomination.Crown:
                return 5; // 1 crown converts down to 5 shillings
            case Denomination.Shilling:
                return 12; // 1 shilling converts down to 12 pence
            case Denomination.Penny:
                return 4; // 1 penny converts down to 4 farthings
            case Denomination.Farthing:
                return 0;
            default:
                return 0;
                //throw new ArgumentException("Unrecognized Denomination");

        }
    }
}