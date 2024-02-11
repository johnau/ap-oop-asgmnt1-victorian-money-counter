using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.Service;

namespace VictorianMoneyCounter.ViewModels;

public partial class WalletPageViewModel : ObservableObject
{
    private readonly IWalletManager<Wallet> _WalletManager; // Wallet class should either extend a base class that is specified here instead, or Wallet should be the base class and future more advanced wallets should extend from Wallet
    public string WalletId { get; private set; }
    private Dictionary<int, string> _denominationsMap = []; // Mapping denomination rows to denominations in wallet
    public WalletPageViewModel(IWalletManager<Wallet> walletManager)
    {
        _WalletManager = walletManager;
        //_walletId = _WalletManager.CreateNewWallet(); // Idealy don't want to create the wallet here... Wallet creation should be handled elsewhere and the views respond to wallets being created (new windows spin up)

        Wallet wallet = _WalletManager.GetWallet();
        WalletId = wallet.Id;

        Debug.WriteLine($"MainPageViewModel instance: {GetHashCode()} and with wallet: {WalletId}");
    }
}
