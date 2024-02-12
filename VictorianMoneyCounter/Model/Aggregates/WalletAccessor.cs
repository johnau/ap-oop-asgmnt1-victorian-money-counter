namespace VictorianMoneyCounter.Model.Aggregates;

/// <summary>
/// Helper class to interact with Wallet record
/// </summary>
public class WalletAccessor
{
    public static WalletAccessor Access(Wallet wallet)
    {
        return new WalletAccessor(wallet);
    }

    private readonly Wallet _wallet;
    protected WalletAccessor(Wallet wallet)
    {
        _wallet = wallet;
    }

    public int GetDenominationQuantity(Denomination denomination)
    {
        return denomination switch
        {
            Denomination.Pound => _wallet.Pounds,
            Denomination.Crown => _wallet.Crowns,
            Denomination.Shilling => _wallet.Shillings,
            Denomination.Penny => _wallet.Pence,
            Denomination.Farthing => _wallet.Farthings,
            _ => throw new ArgumentException("Unrecognized denomination")
        };
    }

    public Wallet UpdateDenominationQuantity(Denomination denomination, int changeAmount)
    {
        return denomination switch
        {
            Denomination.Pound => _wallet.WithPoundsTransaction(changeAmount),
            Denomination.Crown => _wallet.WithCrownsTransaction(changeAmount),
            Denomination.Shilling => _wallet.WithShillingsTransaction(changeAmount),
            Denomination.Penny => _wallet.WithPenceTransaction(changeAmount),
            Denomination.Farthing => _wallet.WithFarthingsTransaction(changeAmount),
            _ => throw new InvalidOperationException($"Unrecognized denomination: {denomination}"),
        };
    }
}
