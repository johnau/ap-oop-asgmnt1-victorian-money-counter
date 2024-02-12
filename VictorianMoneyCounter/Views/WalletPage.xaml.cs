using System.Windows;
using System.Windows.Controls;
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.StartupHelpers;
using VictorianMoneyCounter.ViewModels;

namespace VictorianMoneyCounter.Views;

/// <summary>
/// View of a Victorian Wallet
/// 
/// 
/// Cool thing to do, add some crude animation of the coins dropping into their row as they are added
/// Then be able to shake the app and make it shake the coins down the rows
/// 
/// 
/// </summary>
public partial class WalletPage : Page
{
    private readonly IAbstractFactory<DenominationRow> _RowFactory; // DenominationRow can be interfaced

    public WalletPage(WalletPageViewModel viewModel, IAbstractFactory<DenominationRow> rowFactory)
    {
        DataContext = viewModel;
        _RowFactory = rowFactory;
        InitializeComponent();
        Loaded += ConfigurePage;
    }

    private WalletPageViewModel ViewModel => (WalletPageViewModel)DataContext;

    private void ConfigurePage(object sender, RoutedEventArgs e)
    {
        Array _ = Enum.GetValues(typeof(Denomination));
        Array.Reverse(_);
        int _total = _.Length;
        int i = 1;
        foreach (Denomination d in _)
        {
            DenominationRow denominationRow = _RowFactory.Create();
            denominationRow.GetViewModel().Configure(
                denomination:   d,
                walletId:       ViewModel.WalletId, 
                index:          i, 
                totalRows:      _total, 
                singularLabel:  DenominationInfoFactory.GetDenominationInfo(d).Singular, 
                pluralLabel:    DenominationInfoFactory.GetDenominationInfo(d).Plural
                );

            ViewModel.RegisterChildViewModel((int)d, denominationRow.GetViewModel()); // i could also be key
            
            MainLayoutGrid.Children.Add(denominationRow);
            Grid.SetRow(denominationRow, i++);
        }
    }

}
