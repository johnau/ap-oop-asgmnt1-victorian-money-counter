namespace VictorianMoneyCounter.Model.Aggregates;

public enum Denomination
{
    Pound = 5,
    Crown = 4,
    Shilling = 3,
    Penny = 2,
    Farthing = 1
}

public readonly struct DenominationInfo(int index, string singular, string plural)
{
    public int Index { get; } = index;
    public string Singular { get; } = singular;
    public string Plural { get; } = plural;
}

public static class DenominationInfoFactory
{
    public static DenominationInfo GetDenominationInfo(Denomination denomination) => denomination switch
    {
        Denomination.Pound 
        or Denomination.Crown 
        or Denomination.Shilling 
        or Denomination.Farthing => new DenominationInfo(
                                            index:      (int)denomination, 
                                            singular:   denomination.ToString().ToLower(), 
                                            plural:     denomination.ToString().ToLower() + "s"
        ),
        Denomination.Penny => new DenominationInfo(
                                            index:      (int)denomination, 
                                            singular:   "penny", 
                                            plural:     "pence"
        ),
        _ => throw new ArgumentException("Unrecognized Denomination"),
    };
}