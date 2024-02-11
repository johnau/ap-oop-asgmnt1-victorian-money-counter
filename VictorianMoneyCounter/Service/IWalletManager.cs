namespace VictorianMoneyCounter.Service;

public interface IWalletManager<T>
{
    Dictionary<string, T> _wallets { get; }

    string CreateNewWallet();

    T GetWallet(string id);

    T GetWallet();
}