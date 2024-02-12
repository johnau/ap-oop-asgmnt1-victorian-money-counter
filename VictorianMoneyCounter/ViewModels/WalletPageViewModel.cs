using CommunityToolkit.Mvvm.ComponentModel;
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.Service;

namespace VictorianMoneyCounter.ViewModels;

public partial class WalletPageViewModel : ObservableObject
{
    private readonly IWalletManager<Wallet> _WalletManager;

    public WalletPageViewModel(IWalletManager<Wallet> walletManager)
    {
        _WalletManager = walletManager;
        WalletId = _WalletManager.FindWalletById().Id;
    }

    public string WalletId { get; private set; }

    public void RegisterChildViewModel(int key, IUpdatableViewModel updatableViewModel)
    {
        _WalletManager.RegisterSubscriber(() => updatableViewModel.Update());
    }
}
