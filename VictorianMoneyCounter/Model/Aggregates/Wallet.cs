namespace VictorianMoneyCounter.Model.Aggregates;

public record class Wallet
{

    public string? Id { get; init; }
    public Int32 Pounds { get; private set; }
    public Int32 Crowns { get; private set; }
    public Int32 Shillings { get; private set; }
    public Int32 Pence { get; private set; }
    public Int32 Farthing { get; private set; }


}
