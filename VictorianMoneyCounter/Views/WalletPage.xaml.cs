using System.Windows;
using System.Windows.Controls;
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.StartupHelpers;
using VictorianMoneyCounter.ViewModels;

namespace VictorianMoneyCounter.Views;

public partial class WalletPage : Page
{
    private WalletPageViewModel _ViewModel => (WalletPageViewModel)DataContext;
    private readonly IAbstractFactory<DenominationRow> _RowFactory; // DenominationRow can be interfaced

    public WalletPage(WalletPageViewModel viewModel, IAbstractFactory<DenominationRow> rowFactory)
    {
        DataContext = viewModel;
        _RowFactory = rowFactory;
        InitializeComponent();
        Loaded += ConfigurePage;
    }

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
                walletId:       _ViewModel.WalletId, 
                index:          i, 
                totalRows:      _total, 
                singularLabel:  DenominationInfoFactory.GetDenominationInfo(d).Singular, 
                pluralLabel:    DenominationInfoFactory.GetDenominationInfo(d).Plural
                );
            MainLayoutGrid.Children.Add(denominationRow);
            Grid.SetRow(denominationRow, i++); 
        }
    }
}
