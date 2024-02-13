using VictorianMoneyCounter.Model.Aggregates;

namespace VictorianMoneyCounter.Service;

public interface IWalletManager<T>
{
    string CreateWallet();

    T FindWallet(string id = "");

    T UpdateWallet(string id, Denomination denomination, int valueChange);

    bool RemoveWallet(string id);

    void RegisterSubscriber(string key, Action subscriber);

    void UnregisterSubscribers(string key);
}