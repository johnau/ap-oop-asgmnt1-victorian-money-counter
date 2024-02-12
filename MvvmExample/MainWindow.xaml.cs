using System.Diagnostics;
using System.Windows;

namespace MvvmExample;

public partial class MainWindow : Window
{
    private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

    public MainWindow()
    {
        DataContext = new MainWindowViewModel();
        InitializeComponent();
    }

}