using CommunityToolkit.Mvvm.ComponentModel;

namespace VictorianMoneyCounter.ViewModels;

public partial class TotalRowViewModel : ObservableObject
{

    [ObservableProperty]
    private string _totalString = "£0 0c 0s 0d 0f";
    public TotalRowViewModel()
    {
    }
}
