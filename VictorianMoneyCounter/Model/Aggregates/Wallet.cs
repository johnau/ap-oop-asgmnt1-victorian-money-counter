namespace VictorianMoneyCounter.Model.Aggregates;

public record Wallet(int Pounds = 0, int Crowns = 0, int Shillings = 0, int Pence = 0, int Farthings = 0)
{
    public string Id { get; } = Guid.NewGuid().ToString();

    public Wallet WithPoundsTransaction(int amount)
    {
        var _ = Pounds + amount;
        if (_ < 0)
            return this; // Ignore illegal operation silently
            //throw new InvalidOperationException("Cannot take more pounds than available.");

        return this with { Pounds = _ };
    }

    public Wallet WithCrownsTransaction(int amount)
    {
        var _ = Crowns + amount;
        if (_ < 0)
            return this;

        return this with { Crowns = _ };
    }

    public Wallet WithShillingsTransaction(int amount)
    {
        var _ = Shillings + amount;
        if (_ < 0)
            return this;

        return this with { Shillings = _ };
    }

    public Wallet WithPenceTransaction(int amount)
    {
        var _ = Pence + amount;
        if (_ < 0)
            return this;

        return this with { Pence = _ };
    }

    public Wallet WithFarthingsTransaction(int amount)
    {
        var _ = Farthings + amount;
        if (_ < 0)
            return this;

        return this with { Farthings = _ };
    }
}