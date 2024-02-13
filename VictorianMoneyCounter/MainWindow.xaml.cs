using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using VictorianMoneyCounter.StartupHelpers;
using VictorianMoneyCounter.Views;

namespace VictorianMoneyCounter;

public partial class MainWindow : Window
{
    private readonly IAbstractFactory<WalletPage> _WalletPageFactory;

    public MainWindow(IAbstractFactory<WalletPage> walletPageFactory)
    {
        _WalletPageFactory = walletPageFactory;
        InitializeComponent();
        Loaded += MainWindow_Loaded;
        //LocationChanged += Window_LocationChanged;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        AddChild(_WalletPageFactory.Create());
    }

    //private Point? lastPosition = null;
    //private int shakeCount = 0;
    //private double shakeThreshold = 5d;
    //private bool lastDirectionHorizontal = false; // Initially assume vertical movement
    //private int lastXSign = 0;
    //private int lastYSign = 0;
    //private void Window_LocationChanged(object? sender, EventArgs e)
    //{
    //    var currentPosition = new Point(Left, Top);

    //    if (lastPosition == null)
    //    {
    //        lastPosition = currentPosition;
    //        return;
    //    }

    //    var deltaX = currentPosition.X - lastPosition.Value.X;
    //    var deltaY = currentPosition.Y - lastPosition.Value.Y;

    //    bool isHorizontalMovement = Math.Abs(deltaX) > Math.Abs(deltaY);
    //    Debug.WriteLine($"horizontal={isHorizontalMovement}");
        
    //    int currentXSign = Math.Sign(deltaX);
    //    int currentYSign = Math.Sign(deltaY);
    //    if ((lastXSign != 0 && currentXSign != 0 && lastXSign != currentXSign) ||
    //        (lastYSign != 0 && currentYSign != 0 && lastYSign != currentYSign))
    //    {
    //        Debug.WriteLine("Direction changed?");
    //    }

    //    if (Math.Abs(deltaX) > shakeThreshold || Math.Abs(deltaY) > shakeThreshold)
    //    {
    //        // Check if movement is roughly opposite of last direction
    //        if ((Math.Abs(deltaX) > shakeThreshold && Math.Abs(deltaY) < shakeThreshold && currentXSign != lastXSign) ||
    //            (Math.Abs(deltaY) > shakeThreshold && Math.Abs(deltaX) < shakeThreshold && currentYSign != lastYSign))
    //        {
    //            Debug.WriteLine($"Shake count increased: {shakeCount}");
    //            shakeCount++;
    //        }
    //    }
    //    else
    //    {
    //        shakeCount = 0;
    //    }

    //    if (shakeCount >= 50)
    //    {
    //        Debug.WriteLine("Window shake detected!");
    //    }

    //    lastPosition = currentPosition;
    //    lastDirectionHorizontal = isHorizontalMovement;
    //}
}