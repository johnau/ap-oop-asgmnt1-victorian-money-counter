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
/// TODO: We need inter-row communication (basically an update trigger)
///     by each row getting a reference to the row adjacent, but this is quite limited
///     by a message bus (would be cool to implement)
///     by a delegate function that is passed to all
///     by a reference to wallet manager, and a pub-sub pattern with the manager about updates.  On update, it would call an update function on neccessary rows. or just on everything...
/// </summary>
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
