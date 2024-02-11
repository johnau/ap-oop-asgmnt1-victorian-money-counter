using System.Windows;
using VictorianMoneyCounter.StartupHelpers;
using VictorianMoneyCounter.Views;

namespace VictorianMoneyCounter;

public partial class MainWindow : Window
{
    private readonly IAbstractFactory<MainPage> _MainPageFactory;

    public MainWindow(IAbstractFactory<MainPage> mainPageFactory)
    {
        _MainPageFactory = mainPageFactory;
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        AddChild(_MainPageFactory.Create());
    }
}