using System.Windows.Controls;
using VictorianMoneyCounter.ViewModels;

namespace VictorianMoneyCounter.Views;

public partial class TotalRow : UserControl, IViewModelBacked<TotalRowViewModel>
{
    private TotalRowViewModel ViewModel => (TotalRowViewModel)DataContext;
    public TotalRow(TotalRowViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    public TotalRowViewModel GetViewModel() => ViewModel;
}
