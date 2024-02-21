using System;

namespace VictorianMoneyCounterUWP.Model.Aggregates
{

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
    public class Wallet
    {
        public int Pounds { get; }
        public int Crowns { get; }
        public int Shillings { get; }
        public int Pence { get; }
        public int Farthings { get; }

        public string Id { get; } = Guid.NewGuid().ToString();

        public Wallet(int pounds = 0, int crowns = 0, int shillings = 0, int pence = 0, int farthings = 0)
        {
            Pounds = pounds;
            Crowns = crowns;
            Shillings = shillings;
            Pence = pence;
            Farthings = farthings;
        }

        public Wallet WithPoundsTransaction(int amount)
        {
            var _ = Pounds + amount;
            if (_ < 0)
                return this; // Ignore illegal operation silently
            //throw new InvalidOperationException("Cannot take more pounds than available.");

            return new Wallet(_, Crowns, Shillings, Pence, Farthings);
        }

        public Wallet WithCrownsTransaction(int amount)
        {
            var _ = Crowns + amount;
            if (_ < 0)
                return this;

            return new Wallet(Pounds, _, Shillings, Pence, Farthings);
        }

        public Wallet WithShillingsTransaction(int amount)
        {
            var _ = Shillings + amount;
            if (_ < 0)
                return this;

            return new Wallet(Pounds, Crowns, _, Pence, Farthings);
        }

        public Wallet WithPenceTransaction(int amount)
        {
            var _ = Pence + amount;
            if (_ < 0)
                return this;

            return new Wallet(Pounds, Crowns, Shillings, _, Farthings);
        }

        public Wallet WithFarthingsTransaction(int amount)
        {
            var _ = Farthings + amount;
            if (_ < 0)
                return this;

            return new Wallet(Pounds, Crowns, Shillings, Pence, _);
        }
    }
}
