using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using VictorianMoneyCounter.ViewModels;

namespace VictorianMoneyCounter.Views;

public partial class DenominationRow : UserControl, IDenominationRow<DenominationRowViewModel>
{
    private DenominationRowViewModel ViewModel => (DenominationRowViewModel) DataContext;
    
    public DenominationRow()
    {
        InitializeComponent();
    }

    public DenominationRow(DenominationRowViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    public DenominationRowViewModel GetViewModel()
    {
        return ViewModel;
    }

    private void Button_MouseDown(object sender, MouseButtonEventArgs e)
    {
        Debug.WriteLine($"Mouse Down on: {sender.GetType()}");
    }

    private void Button_MouseUp(object sender, MouseButtonEventArgs e)
    {
        Debug.WriteLine($"Mouse Up on: {sender.GetType()}");
    }

    private void Button_LostMouseCapture(object sender, MouseEventArgs e)
    {

    }
}
