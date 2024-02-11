using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.Service;

namespace VictorianMoneyCounter.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly IWalletManager<Wallet> _WalletManager; // Wallet class should either extend a base class that is specified here instead, or Wallet should be the base class and future more advanced wallets should extend from Wallet
    private readonly string _walletId;
    private Dictionary<int, string> _denominationsMap = []; // Mapping denomination rows to denominations in wallet
    public MainPageViewModel(IWalletManager<Wallet> walletManager)
    {
        Debug.WriteLine("MainPageViewModel instance: " + GetHashCode());

        _WalletManager = walletManager;
        _walletId = _WalletManager.CreateNewWallet(); // Idealy don't want to create the wallet here... Wallet creation should be handled elsewhere and the views respond to wallets being created (new windows spin up)

        //Wallet wallet = _WalletManager.GetWallet();
        //_walletId = wallet.Id;
    }
}
