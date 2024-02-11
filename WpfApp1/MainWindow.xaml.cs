using System.Windows;
using WpfApp1.StartupHelpers;
using WpfApp1ClassLibrary;

namespace WpfApp1;

public partial class MainWindow : Window
{
    private readonly IDataAccess _dataAccess;
    private readonly IAbstractFactory<ChildForm> _factory;

    public MainWindow(IDataAccess dataAccess, IAbstractFactory<ChildForm> factory)
    {
        _dataAccess = dataAccess;
        this._factory = factory;
        InitializeComponent();
    }

    private void getData_Click(object sender, RoutedEventArgs e)
    {
        data.Text = _dataAccess.GetData();
    }

    private void showChildForm_Click(object sender, RoutedEventArgs e)
    {
        _factory.Create().Show();
    }

}