using System.Diagnostics;
using VictorianMoneyCounter.Model.Aggregates;

namespace VictorianMoneyCounter.Service;

public class WalletManager : IWalletManager<Wallet>
{
    public Dictionary<string, Wallet> _wallets { get; private set; } = [];

    public WalletManager()
    {
        CreateNewWallet(); // create a default wallet
    }

    public string CreateNewWallet()
    {
        Wallet wallet = new();
        _wallets.Add(wallet.Id, wallet);

        Debug.WriteLine("Created a new Wallet: " + wallet.Id);

        return wallet.Id;
    }

    public Wallet GetWallet(string id)
    {
        return _wallets[id];
    }

    public Wallet GetWallet()
    {
        if (_wallets.Count == 0) throw new Exception("Internal error"); // There should always be a wallet

        return _wallets.Values.First<Wallet>();
    }
}
