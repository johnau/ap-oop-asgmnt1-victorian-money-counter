using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.Service;
using VictorianMoneyCounter.StartupHelpers;
using VictorianMoneyCounter.Views;

namespace VictorianMoneyCounter.ViewModels;

/// <summary>
/// View Model for Wallet Page
/// 
/// TO DO: Need to implement an OnClose event that triggers
/// </summary>
public partial class WalletPageViewModel : ObservableObject, IConfigurableViewModel<BasicViewModelConfiguration>
{
    private readonly IWalletManager<Wallet> _WalletManager;
    private readonly IAbstractFactory<ChildWindow> _WindowFactory;
    private readonly IAbstractFactory<WalletPage> _WalletPageFactory;
    public string WalletId { get; private set; }
    
    public WalletPageViewModel(IWalletManager<Wallet> walletManager, 
                                IAbstractFactory<ChildWindow> windowFactory,
                                IAbstractFactory<WalletPage> walletPageFactory)
    {
        _WalletManager = walletManager;
        _WindowFactory = windowFactory;
        _WalletPageFactory = walletPageFactory;
        WalletId = _WalletManager.FindWallet().Id;
    }

    public void Configure(BasicViewModelConfiguration config)
    {
        WalletId = config.WalletId;
    }

    public void RegisterChildViewModel(IUpdatableViewModel updatableViewModel)
    {
        _WalletManager.RegisterSubscriber(WalletId, () => updatableViewModel.Update());
    }

    [RelayCommand]
    public void UnregisterChildViewModels()
    {
        _WalletManager.UnregisterSubscribers(WalletId);
    }

    [RelayCommand]
    private void NewWallet()
    {
        var newWalletId = _WalletManager.CreateWallet();
        var window = _WindowFactory.Create();
        var walletPage = _WalletPageFactory.Create();

        walletPage.GetViewModel().Configure(new BasicViewModelConfiguration(newWalletId));
        
        window.Loaded += (sender, args) => window.Content = walletPage;
        window.Show();
    }  
}
