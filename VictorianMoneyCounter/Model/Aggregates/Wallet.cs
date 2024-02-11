namespace VictorianMoneyCounter.Model.Aggregates;

public class Wallet
{

    public string Id { get; private set; }
    public Int32 Pounds { get; private set; }
    public Int32 Crowns { get; private set; }
    public Int32 Shillings { get; private set; }
    public Int32 Pence { get; private set; }
    public Int32 Farthing { get; private set; }



}
