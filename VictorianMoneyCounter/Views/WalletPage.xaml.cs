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
/// How do we calculate the total, we start at farthings (smallest), 
/// can use recursion or a loop to convert all farthings to pence (with some farthings remainder)
/// or could use a calculation that works the answer out immediately
/// 
/// 
/// TODO: Separate out some logic from viewmodel of denominationrow that are not part of the assignment
/// so it is easier to see whats going on
/// 
/// </summary>
public partial class WalletPage : Page
{
    private readonly IAbstractFactory<DenominationRow> _DenominationRowFactory; // DenominationRow can be interfaced
    private readonly IAbstractFactory<TotalRow> _TotalRowFactory;

    private WalletPageViewModel ViewModel => (WalletPageViewModel)DataContext;
    public WalletPage(
        WalletPageViewModel viewModel, 
        IAbstractFactory<DenominationRow> denominationRowFactory,
        IAbstractFactory<TotalRow> totalRowFactory)
    {
        DataContext = viewModel;
        _DenominationRowFactory = denominationRowFactory;
        _TotalRowFactory = totalRowFactory;
        InitializeComponent();
        Loaded += ConfigurePage;
    }

    private void ConfigurePage(object sender, RoutedEventArgs e)
    {
        ConfigureDenominationRows();
        ConfigureTotalRows();
    }

    private void ConfigureTotalRows()
    {
        var totalRow = _TotalRowFactory.Create();
    }

    private void ConfigureDenominationRows()
    {
        var _ = Enum.GetValues(typeof(Denomination));
        Array.Reverse(_);
        var _total = _.Length;
        var i = 1;
        foreach (Denomination d in _)
        {
            var denominationRow = _DenominationRowFactory.Create();
            denominationRow.GetViewModel().Configure(
                denomination: d,
                walletId: ViewModel.WalletId,
                index: i,
                totalRows: _total,
                singularLabel: DenominationInfoFactory.GetDenominationInfo(d).Singular,
                pluralLabel: DenominationInfoFactory.GetDenominationInfo(d).Plural
                );

            ViewModel.RegisterChildViewModel((int)d, denominationRow.GetViewModel()); // i could also be key

            MainLayoutGrid.Children.Add(denominationRow);
            Grid.SetRow(denominationRow, i++);
        }
    }

}
