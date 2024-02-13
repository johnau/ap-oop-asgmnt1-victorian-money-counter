using System.Linq.Expressions;

namespace VictorianMoneyCounter.Model.Aggregates;

public enum Denomination
{
    Pound = 5,
    Crown = 4,
    Shilling = 3,
    Penny = 2,
    Farthing = 1
}

public readonly struct DenominationInfo(int denomination, string singular, string plural, char symbol)
{
    public int Denomination { get; } = denomination;
    public string Singular { get; } = singular;
    public string Plural { get; } = plural;
    public char Symbol { get; } = symbol;
}

public static class DenominationInfoFactory
{
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
}