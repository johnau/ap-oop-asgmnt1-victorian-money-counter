using System.Diagnostics;

namespace VictorianMoneyCounter.Model.Aggregates;

public record Wallet(int Pounds = 0, int Crowns = 0, int Shillings = 0, int Pence = 0, int Farthings = 0)
{
    public string Id { get; } = Guid.NewGuid().ToString();

    public Wallet WithPoundsTransaction(int amount)
    {
        var _ = Pounds + amount;
        if (_ < 0)
            throw new InvalidOperationException("Cannot take more pounds than available.");

        return this with { Pounds = _ };
    }

    public Wallet WithCrownsTransaction(int amount)
    {
        var _ = Crowns + amount;
        if (_ < 0)
            throw new InvalidOperationException("Cannot take more crowns than available.");

        return this with { Crowns = _ };
    }

    public Wallet WithShillingsTransaction(int amount)
    {
        var _ = Shillings + amount;
        if (_ < 0)
            throw new InvalidOperationException("Cannot take more shillings than available.");

        return this with { Shillings = _ };
    }

    public Wallet WithPenceTransaction(int amount)
    {
        var _ = Pence + amount;
        if (_ < 0)
            throw new InvalidOperationException("Cannot take more pence than available.");
        return this with { Pence = _ };
    }

    public Wallet WithFarthingsTransaction(int amount)
    {
        var _ = Farthings + amount;
        if (_ < 0)
            throw new InvalidOperationException("Cannot take more farthings than available.");
        return this with { Farthings = _ };
    }
}

// Re-implement the Wallet class as immutable record class
//public class Wallet(int Pounds = 0, int Crowns = 0, int shillings = 0, int pence = 0, int farthings = 0)
//{
//    public string Id { get; init; } = Guid.NewGuid().ToString();
//    public int Pounds { get; private set; } = Pounds;
//    public int Crowns { get; private set; } = Crowns;
//    public int Shillings { get; private set; } = shillings;
//    public int Pence { get; private set; } = pence;
//    public int Farthings { get; private set; } = farthings;

//    public void AddPound() => Pounds++;
//    public void AddCrown() => Crowns++;
//    public void AddShilling() => Shillings++;
//    public void AddPenny() => Pence++;
//    public void AddFarthing() => Farthings++;
//    public void RemovePoundOrThrow()
//    {
//        if (Pounds <= 0) throw new Exception("Insufficient quantity");
//        Pounds--;
//    }
//    public void RemoveCrownOrThrow()
//    {
//        if (Crowns <= 0) throw new Exception("Insufficient quantity");
//        Crowns--;
//    }
//    public void RemoveShillingsOrThrow()
//    {
//        if (Shillings <= 0) throw new Exception("Insufficient quantity");
//        Shillings--;
//    }
//    public void RemovePenceOrThrow()
//    {
//        if (Pence <= 0) throw new Exception("Insufficient quantity");
//        Pence--;
//    }
//    public void RemoveFarthingsOrThrow()
//    {
//        if (Farthings <= 0) throw new Exception("Insufficient quantity");
//        Farthings--;
//    }
//}
