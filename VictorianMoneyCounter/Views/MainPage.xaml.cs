using System.Windows.Controls;
using VictorianMoneyCounter.StartupHelpers;
using VictorianMoneyCounter.ViewModels;

namespace VictorianMoneyCounter.Views;

// This page component needs to load up the Denomination Row objects that are from injection
public partial class MainPage : Page
{
    private MainPageViewModel _ViewModel => (MainPageViewModel)DataContext;
    private readonly IAbstractFactory<DenominationRow> _RowFactory;

    public MainPage(MainPageViewModel viewModel, IAbstractFactory<DenominationRow> rowFactory)
    {
        DataContext = viewModel;
        _RowFactory = rowFactory;
        InitializeComponent();
        Loaded += MainPage_Loaded;
    }

    private void MainPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        // this is still just view code, but now the issue is linking with data without having data here
        // eventually want to loosen this up to handle any number of rows for other assets
        for (int i = 1; i <= 5; i++)
        {
            DenominationRow denominationRow = _RowFactory.Create();
            denominationRow.GetViewModel().Index = i;
            MainLayoutGrid.Children.Add(denominationRow);
            Grid.SetRow(denominationRow, i);
        }
    }
}
