using System.Windows;
using VictorianMoneyCounter.StartupHelpers;
using VictorianMoneyCounter.Views;

namespace VictorianMoneyCounter;

public partial class MainWindow : Window
{
    private readonly IAbstractFactory<WalletPage> _WalletPageFactory;

    public MainWindow(IAbstractFactory<WalletPage> walletPageFactory)
    {
        _WalletPageFactory = walletPageFactory;
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        AddChild(_WalletPageFactory.Create());
    }
}