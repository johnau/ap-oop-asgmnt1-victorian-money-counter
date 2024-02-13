using System.Windows.Controls;
using VictorianMoneyCounter.ViewModels;

namespace VictorianMoneyCounter.Views;

public partial class TotalRow : UserControl
{
    private TotalRowViewModel ViewModel => (TotalRowViewModel)DataContext;
    public TotalRow(TotalRowViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
