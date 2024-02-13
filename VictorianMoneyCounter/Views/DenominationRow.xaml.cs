using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using VictorianMoneyCounter.Utilities;
using VictorianMoneyCounter.ViewModels;

namespace VictorianMoneyCounter.Views;

public partial class DenominationRow : UserControl, IViewModelBacked<DenominationRowViewModel>
{
    private DenominationRowViewModel ViewModel => (DenominationRowViewModel) DataContext;
    private readonly List<Image> _coins = [];

    public DenominationRow()
    {
        InitializeComponent();
    }

    public DenominationRow(DenominationRowViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
        viewModel.RegisterSubscriberToQuantity((qty) => UpdateCoins(qty));
        //Loaded += DenominationRow_Loaded;
    }

    private void UpdateCoins(int quantity)
    {
        if (quantity > _coins.Count)
        {
            // add coins
            for (int i = 0; i < quantity - _coins.Count; i++)
            {
                var coin = SpawnCoin();
                _coins.Add(coin);
                DropCoin(coin);
            }
        }
        else if (quantity < _coins.Count)
        {
            for (int i = 0; i < _coins.Count - quantity; i++)
            {
                var removing = _coins.Last();
                _coins.Remove(removing);
                RowCanvas.Children.Remove(removing);
            }
        }
    }

    public DenominationRowViewModel GetViewModel() => ViewModel;


    /// <summary>
    /// Spawns a Coin above the canvas
    /// </summary>
    /// <returns></returns>
    private Image SpawnCoin()
    {
        Image image = new()
        {
            Width = 20,
            Height = 20,
            Source = new BitmapImage(new Uri("pack://application:,,,/Assets/coin.png"))
        };

        Canvas.SetLeft(image, RowCanvas.ActualWidth / 2);
        Canvas.SetTop(image, -image.Height);
        RowCanvas.Children.Add(image);

        return image;
    }

    /// <summary>
    /// Create animation of Coin dropping into the row
    /// </summary>
    /// <param name="image"></param>
    private void DropCoin(Image image)
    {
        Random random = new();

        var animationDuration = 1d; // 1 second
        var fallSpeed = 0.1; // 0.1 second
        var dir = random.Next(0, 2) == 0 ? 1 : -1;
        var startX = (RowCanvas.ActualWidth - image.Width) / 2;
        var startY = -image.Height;
        var finalY = RowCanvas.ActualHeight - image.Height;

        var animationY = new DoubleAnimationUsingKeyFrames();
        var animationX = new DoubleAnimationUsingKeyFrames();

        // define the fall
        animationY.KeyFrames.Add(new LinearDoubleKeyFrame(startY, TimeSpan.FromSeconds(0)));
        animationY.KeyFrames.Add(new LinearDoubleKeyFrame(finalY, TimeSpan.FromSeconds(fallSpeed)));

        animationX.KeyFrames.Add(new LinearDoubleKeyFrame(startX, TimeSpan.FromSeconds(0)));
        animationX.KeyFrames.Add(new LinearDoubleKeyFrame(startX, TimeSpan.FromSeconds(fallSpeed)));

        // define the bounces
        var maxHeight = image.Height * 3;
        var horizontalForce = random.Next(0, (int)image.Width / 2);
        var x = startX;
        for (double t = fallSpeed; t <= animationDuration; t += 0.05)
        {
            var bounceHeight = MathHelpers.CalculateBounceHeight(animationDuration, t, maxHeight); // was (finalY + image.Height)
            var bounceLength = MathHelpers.CalculateBounceLength(animationDuration, t, horizontalForce);
            var keyTime = TimeSpan.FromSeconds(t);
            animationY.KeyFrames.Add(new LinearDoubleKeyFrame(finalY - bounceHeight, keyTime));
            x += bounceLength * dir;
            animationX.KeyFrames.Add(new LinearDoubleKeyFrame(x, keyTime));
        }

        Storyboard.SetTarget(animationY, image);
        Storyboard.SetTargetProperty(animationY, new PropertyPath(Canvas.TopProperty));
        Storyboard.SetTarget(animationX, image);
        Storyboard.SetTargetProperty(animationX, new PropertyPath(Canvas.LeftProperty));

        var storyboard = new Storyboard();
        storyboard.Children.Add(animationY);
        storyboard.Children.Add(animationX);
        storyboard.Begin();
    }

}
