using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace VictorianMoneyCounter.ViewModels;
public partial class DenominationRowViewModel : ObservableObject, IIndexedViewModel
{
    public int Index { get; set; }

    [ObservableProperty]
    private string _label = "<not set>";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DecreaseByOneCommand))]
    private Int32 _countValue = 0;

    public DenominationRowViewModel()
    {
        Debug.WriteLine("New DenominationRowViewModel instasntiated: " + this.GetHashCode());
    }

    public void SetLabel(string label) => Label = label;

    private void IncreaseByOne() => CountValue += 1;

    private bool CanDecrease() => CountValue > 0;

    [RelayCommand(CanExecute = nameof(CanDecrease))]
    private void DecreaseByOne()
    {
        if (CountValue > 0) // Method already protected, so this check is not really required
        {
            CountValue -= 1;
        }
    }

}
