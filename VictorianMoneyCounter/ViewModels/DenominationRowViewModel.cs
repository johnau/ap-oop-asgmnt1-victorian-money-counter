using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Threading;
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.Service;
using VictorianMoneyCounter.Utilities;

namespace VictorianMoneyCounter.ViewModels;

/// <summary>
/// ViewModel for DenominationRow UserControl.
/// Partial class extends CommunityToolkit.Mvvm.ComponentModel.ObservableObject to provide ObservableProperty
/// </summary>
public partial class DenominationRowViewModel : ObservableObject, IIndexedViewModel, IUpdatableViewModel, IConfigurableViewModel<DenominationRowViewModelConfiguration>
{
    private readonly IWalletManager<Wallet> _WalletManager;
    private readonly ICurrencyConverter _CurrencyConverter;
    private readonly DispatcherTimer _actionHoldTimer;
    public Denomination Denomination { get; set; }
    public string SingularLabel { get; set; } = string.Empty;
    public string PluralLabel { get; set; } = string.Empty;
    public string WalletId { get; set; } = string.Empty;

    [ObservableProperty]
    public int _index;

    [ObservableProperty]
    private string _label = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DecreaseByOneCommand), nameof(ConvertUpCommand), nameof(ConvertDownCommand))]
    private int _quantity;

    [ObservableProperty]
    public bool _use_ExchangeUp = true;
    
    [ObservableProperty]
    public bool _use_ExchangeDown = true;

    [ObservableProperty]
    public bool _canExchange = true;

    public IRelayCommand? _actionHeld;

    private List<Action<int>> _quantitySubscribers = [];

    /// <summary>
    /// Primary constructor
    /// </summary>
    /// <param name="walletManager"></param>
    /// <param name="currencyConverter"></param>
    public DenominationRowViewModel(IWalletManager<Wallet> walletManager, ICurrencyConverter currencyConverter)
    {
        _WalletManager = walletManager;
        _CurrencyConverter = currencyConverter;
        _actionHoldTimer = new()
        {
            Interval = TimeSpan.FromMilliseconds(25)
        };
        _actionHoldTimer.Tick += TimerTickAction;
    }

    public void Configure(DenominationRowViewModelConfiguration configuration)
    {
        Denomination = configuration.Denomination;
        WalletId = configuration.WalletId;
        Index = configuration.Index;
        SingularLabel = configuration.SingularLabel;
        PluralLabel = configuration.PluralLabel;
        Label = configuration.PluralLabel;

        if (Index == 1) Use_ExchangeUp = false;
        if (configuration.TotalRows > 0 && Index == configuration.TotalRows) Use_ExchangeDown = false;
    }

    public void RegisterSubscriberToQuantity(Action<int> action) => _quantitySubscribers.Add(action);

    partial void OnQuantityChanged(int value) => Label = value == 1 ? SingularLabel : PluralLabel;

    partial void OnLabelChanged(string? oldValue, string newValue) => Label = (oldValue == null || !oldValue.Equals(newValue)) ? StringHelpers.CapitalizeFirstLetter(newValue) : Label;

    /// <summary>
    /// Increase the quantity of this row by one
    /// </summary>
    [RelayCommand]
    private void IncreaseByOne()
    {
        _WalletManager.UpdateWallet(WalletId, Denomination, 1);
    }

    /// <summary>
    /// Decrease the quantity of this row by one
    /// </summary>
    [RelayCommand(CanExecute = nameof(IsNotEmpty))]
    private void DecreaseByOne()
    {
        // Could pre-check Wallet denomination balance, but IsNotEmpty() is effectively performing that task
        _WalletManager.UpdateWallet(WalletId, Denomination, -1); // Throws exception
    }

    /// <summary>
    /// Converts the denomination to the next higher denomination
    /// /// Mvvm.Input.RelayCommand for binding access with view
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanConvertUp))]
    private void ConvertUp()
    {
        var requiredQuantity = _CurrencyConverter.ConvertUp(Denomination);
        _WalletManager.UpdateWallet(WalletId, Denomination, -requiredQuantity); // Take out req'd quantity from the Wallet
        _WalletManager.UpdateWallet(WalletId, Denomination+1, 1);
    }

    /// <summary>
    /// Converts the denomination to the next lower denomination.
    /// Mvvm.Input.RelayCommand for binding access with view
    /// </summary>
    [RelayCommand(CanExecute = nameof(IsNotEmpty))]
    private void ConvertDown()
    {
        // Could pre-check Wallet denomination balance, but IsNotEmpty() is effectively performing that task
        _WalletManager.UpdateWallet(WalletId, Denomination, -1); // Take out 1 of the denomination to exchange for next lower denomination
        var convertedQuantity = _CurrencyConverter.ConvertDown(Denomination);
        _WalletManager.UpdateWallet(WalletId, Denomination-1, convertedQuantity);
    }

    /// <summary>
    /// Command used by all buttons to activate "action hold"
    /// </summary>
    /// <param name="command"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [RelayCommand(IncludeCancelCommand = true)] // As an async task, do we AllowConcurrentTransactions = true, IncludeCancelCommand = true (HoldActionCancelCommand created) and provide Cancellation token
    private async Task HoldAction(IRelayCommand command, CancellationToken token)
    {
        try
        {
            await Task.Delay(250, token);
            _actionHoldTimer.Start();
            _actionHeld = command;
        }
        catch (OperationCanceledException) 
        {
            ReleaseAction(command);
        }
    }

    /// <summary>
    /// Command used by all buttons to deactivate "action hold"
    /// </summary>
    /// <param name="command"></param>
    [RelayCommand]
    private void ReleaseAction(IRelayCommand command)
    {
        _actionHoldTimer.Stop();
        _actionHeld = null;
    }

    /// <summary>
    /// Helper function for RelayCommand CanExecute
    /// </summary>
    /// <returns>True if Quantity is greater than 0, else False</returns>
    private bool IsNotEmpty() => Quantity > 0; // Should this ref the wallet instead of Quantity

    /// <summary>
    /// Helper function for RelayCommand CanExecute
    /// </summary>
    /// <returns>True if Quantity is greater than or equal to required amount for converting to larger denomination</returns>
    private bool CanConvertUp() => Quantity > 0 && Quantity >= _CurrencyConverter.ConvertUp(Denomination); // Should this ref the wallet instead of Quantity

    /// <summary>
    /// Call to force an update on the model
    /// </summary>
    public void Update()
    {
        var wallet = _WalletManager.FindWallet(WalletId);
        Quantity = WalletAccessor.Access(wallet).GetDenominationQuantity(Denomination);
        _quantitySubscribers.ForEach(subscriber => subscriber.Invoke(Quantity));
    }

    /// <summary>
    /// Method run on each tick of the DispatcherTimer
    /// 
    /// TODO: Increase the speed of the action after some period of continuous running
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TimerTickAction(object? sender, EventArgs e)
    {
        if (_actionHeld != null && _actionHeld.CanExecute(null))
        {
            _actionHeld.Execute(null);
        }
    }
}
