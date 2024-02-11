using System.Windows;
using VictorianMoneyCounter.Views;

namespace VictorianMoneyCounter;

public partial class MainWindow : Window
{
    private readonly MainPage MainPage;

    public MainWindow(MainPage mainPage)
    {
        MainPage = mainPage;
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        AddChild(MainPage);
    }
}