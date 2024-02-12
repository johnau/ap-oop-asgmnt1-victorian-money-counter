using VictorianMoneyCounter.Model.Aggregates;

namespace VictorianMoneyCounter.Service;

public interface IWalletManager<T>
{
    string CreateWallet();

    T FindWalletById(string id = "");

    T UpdateWallet(string id, Denomination denomination, int valueChange);

    bool RemoveWallet(string id);
}