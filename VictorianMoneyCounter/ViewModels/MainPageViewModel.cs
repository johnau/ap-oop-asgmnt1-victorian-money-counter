using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Windows.Automation;

namespace VictorianMoneyCounter.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private Dictionary<String, Int32> _rows = [];

    public MainPageViewModel()
    {
        _rows.Add("pounds", 0);
        _rows.Add("crowns", 0);
        _rows.Add("shillings", 0);
        _rows.Add("pence", 0);
        _rows.Add("farthings", 0);

        Debug.WriteLine("MainPageViewModel constructed");
    }
}
