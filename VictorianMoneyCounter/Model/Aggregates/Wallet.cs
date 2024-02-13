namespace VictorianMoneyCounter.Model.Aggregates;

/// <summary>
/// Wallet Record object
/// Immutable, Builder pattern
/// Illegal transactions are silently ignored, ie. a result to take 5 when there is only 4 will result in no change.
/// </summary>
/// <param name="Pounds"></param>
/// <param name="Crowns"></param>
/// <param name="Shillings"></param>
/// <param name="Pence"></param>
/// <param name="Farthings"></param>
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