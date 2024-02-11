using System.Diagnostics;
using System.Windows.Controls;
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.StartupHelpers;
using VictorianMoneyCounter.ViewModels;

namespace VictorianMoneyCounter.Views;

// This page component needs to load up the Denomination Row objects that are from injection
public partial class WalletPage : Page
{
    private WalletPageViewModel _ViewModel => (WalletPageViewModel)DataContext;
    private readonly IAbstractFactory<DenominationRow> _RowFactory; // Does DenominationRow get interfaced?

    public WalletPage(WalletPageViewModel viewModel, IAbstractFactory<DenominationRow> rowFactory)
    {
        DataContext = viewModel;
        _RowFactory = rowFactory;
        InitializeComponent();
        Loaded += ConfigurePage;
    }

    private void ConfigurePage(object sender, System.Windows.RoutedEventArgs e)
    {
        Array _ = Enum.GetValues(typeof(Denomination));
        int _total = _.Length;
        foreach (Denomination d in _)
        {
            DenominationRow denominationRow = _RowFactory.Create();
            denominationRow.GetViewModel().Configure(
                walletId:       _ViewModel.WalletId, 
                index:          (int)d, 
                totalRows:      _total, 
                singularLabel:  DenominationInfoFactory.GetDenominationInfo(d).Singular, 
                pluralLabel:    DenominationInfoFactory.GetDenominationInfo(d).Plural
                );
            MainLayoutGrid.Children.Add(denominationRow);
            Grid.SetRow(denominationRow, (int)d);
        }
    }
}
