using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.Service;

namespace VictorianMoneyCounter.ViewModels;

public partial class WalletPageViewModel : ObservableObject
{
    private readonly IWalletManager<Wallet> _WalletManager;
    public string WalletId { get; private set; }
    
    public WalletPageViewModel(IWalletManager<Wallet> walletManager)
    {
        _WalletManager = walletManager;
        WalletId = _WalletManager.FindWallet().Id;
    }

    public void Configure(string walletId)
    {

    }

    public void RegisterChildViewModel(int key, IUpdatableViewModel updatableViewModel)
    {
        _WalletManager.RegisterSubscriber(() => updatableViewModel.Update());
    }

    [RelayCommand]
    private void NewWallet()
    {
        Debug.WriteLine("Will create new wallet here");
        // create new wallet (get id)
        // create new ChildWindow (need factory)
        // create new WalletPage
        // Set Wallet Page to Child Window
        // Show Child window
    }  
}
