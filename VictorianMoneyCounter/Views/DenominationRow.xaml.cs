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

    /// <summary>
    /// Primary Constructor
    /// </summary>
    /// <param name="viewModel"></param>
    public DenominationRow(DenominationRowViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
        viewModel.SubscribeToQuantityChange((qty) => UpdateCoins(qty));
    }

    public DenominationRowViewModel GetViewModel() => ViewModel;

    /// <summary>
    /// Adds or removes coins based to match the quantity provided
    /// </summary>
    /// <param name="quantity"></param>
    private void UpdateCoins(int quantity)
    {
        if (quantity > _coins.Count)
        {
            for (int i = 0; i <= quantity-_coins.Count; i++)
            {
                var coin = SpawnCoin();
                _coins.Add(coin);
                DropCoin(coin);
            }
        }
        else if (quantity < _coins.Count)
        {
            for (int i = 0; i <= _coins.Count-quantity; i++)
            {
                var removing = _coins.Last();
                while (RowCanvas.Children.Contains(removing))
                {
                    RowCanvas.Children.Remove(removing);
                }
                _coins.Remove(removing);
            }
        }
    }

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
        var random = new Random();

        // configuration variables
        var animationDuration = 1d; // 1 second
        var fallSpeed = 0.1;        // 0.1 second
        var bounceForce = image.Height * 3; // force of bounce
        var horizontalForce = random.Next(0, (int)image.Width); // sideways force

        var direction = random.Next(0, 2) == 0 ? 1 : -1;
        //var startX = (RowCanvas.ActualWidth - image.Width) / 2;
        var startX = (double)random.Next((int)image.Width * 2, (int)(RowCanvas.ActualWidth - (image.Width*2)));
        var startY = -image.Height;
        var endY = RowCanvas.ActualHeight - image.Height;

        var animationY = new DoubleAnimationUsingKeyFrames();
        var animationX = new DoubleAnimationUsingKeyFrames();

        // manually define the fall. start of fall
        animationY.KeyFrames.Add(new LinearDoubleKeyFrame(startY, TimeSpan.FromSeconds(0)));
        animationX.KeyFrames.Add(new LinearDoubleKeyFrame(startX, TimeSpan.FromSeconds(0)));

        // end of fall
        animationY.KeyFrames.Add(new LinearDoubleKeyFrame(endY, TimeSpan.FromSeconds(fallSpeed)));
        animationX.KeyFrames.Add(new LinearDoubleKeyFrame(startX, TimeSpan.FromSeconds(fallSpeed)));

        // define the bounces
        var locationX = startX;
        for (double t = fallSpeed; t <= animationDuration; t += 0.1)
        {
            var keyTime = TimeSpan.FromSeconds(t);
            var bounceHeight = MathHelpers.CalculateBounceHeight(animationDuration, t, bounceForce);
            var bounceLength = MathHelpers.CalculateBounceLength(animationDuration, t, horizontalForce);
                        
            locationX += bounceLength * direction;
            animationX.KeyFrames.Add(new LinearDoubleKeyFrame(locationX, keyTime));
            animationY.KeyFrames.Add(new LinearDoubleKeyFrame(endY - bounceHeight, keyTime));
        }

        Storyboard.SetTarget(animationX, image);
        Storyboard.SetTargetProperty(animationX, new PropertyPath(Canvas.LeftProperty));
        Storyboard.SetTarget(animationY, image);
        Storyboard.SetTargetProperty(animationY, new PropertyPath(Canvas.TopProperty));

        var storyboard = new Storyboard();
        storyboard.Children.Add(animationX);
        storyboard.Children.Add(animationY);
        storyboard.Begin();
    }

}
