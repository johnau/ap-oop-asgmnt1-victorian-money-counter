using System.Windows;
using WpfApp1ClassLibrary;

namespace WpfApp1;

public partial class ChildForm : Window
{
    private readonly IDataAccess _dataAccess;

    public ChildForm(IDataAccess dataAccess)
    {
        InitializeComponent();
        this._dataAccess = dataAccess;
        data.Text = _dataAccess.GetData();
    }
}
