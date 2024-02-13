using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.Service;
using VictorianMoneyCounter.StartupHelpers;
using VictorianMoneyCounter.Views;

namespace VictorianMoneyCounter.ViewModels;

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

    public void RegisterChildViewModel(int key, IUpdatableViewModel updatableViewModel)
    {
        _WalletManager.RegisterSubscriber(() => updatableViewModel.Update());
    }

    [RelayCommand]
    private void NewWallet()
    {
        Debug.WriteLine("Will create new wallet here");
        // create new wallet (get id)
        var newWalletId = _WalletManager.CreateWallet();
        // create new ChildWindow (need factory)
        var window = _WindowFactory.Create();
        // create new WalletPage
        var walletPage = _WalletPageFactory.Create();
        // configure new WalletPage
        var viewModelConfig = new BasicViewModelConfiguration(newWalletId);
        var viewModel = walletPage.GetViewModel();
        viewModel.Configure(viewModelConfig);
        // Set Wallet Page to Child Window
        window.Loaded += (sender, args) => window.Content = walletPage;
        // Show Child window
        window.Show();
    }  
}
