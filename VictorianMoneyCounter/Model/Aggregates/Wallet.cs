using System.Diagnostics;

namespace VictorianMoneyCounter.Model.Aggregates;

// Re-implement the Wallet class as immutable record class
public class Wallet
{
    public string Id { get; init; }
    public Int32 Pounds { get; private set; }
    public Int32 Crowns { get; private set; }
    public Int32 Shillings { get; private set; }
    public Int32 Pence { get; private set; }
    public Int32 Farthings { get; private set; }

    public Wallet(int pounds = 0, int crowns = 0, int shillings = 0, int pence = 0, int farthings = 0)
    {
        Id = Guid.NewGuid().ToString();
        Pounds = pounds;
        Crowns = crowns;
        Shillings = shillings;
        Pence = pence;
        Farthings = farthings;
    }

    public void AddPound() => Pounds++;
    public void AddCrown() => Crowns++;
    public void AddShilling() => Shillings++;
    public void AddPenny() => Pence++;
    public void AddFarthing() => Farthings++;
    public void RemovePound()
    {
        if (Pounds <= 0) throw new Exception("Insufficient quantity");
        Pounds--;
    }
    public void RemoveCrown()
    {
        if (Crowns <= 0) throw new Exception("Insufficient quantity");
        Crowns--;
    }
    public void RemoveShillings()
    {
        if (Shillings <= 0) throw new Exception("Insufficient quantity");
        Shillings--;
    }
    public void RemovePence()
    {
        if (Pence <= 0) throw new Exception("Insufficient quantity");
        Pence--;
    }
    public void RemoveFarthings()
    {
        if (Farthings <= 0) throw new Exception("Insufficient quantity");
        Farthings--;
    }
}
