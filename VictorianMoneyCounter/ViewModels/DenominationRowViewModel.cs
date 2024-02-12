using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.Service;
using VictorianMoneyCounter.Utilities;

namespace VictorianMoneyCounter.ViewModels;

/// <summary>
/// ViewModel for DenominationRow UserControl.
/// Partial class extends CommunityToolkit.Mvvm.ComponentModel.ObservableObject to provide ObservableProperty
/// </summary>
public partial class DenominationRowViewModel : ObservableObject, IIndexedViewModel
{
    private readonly IWalletManager<Wallet> _WalletManager;
    private readonly ICurrencyConverter _CurrencyConverter;

    public Denomination Denomination { get; set; }
    public string SingularLabel { get; set; } = string.Empty;
    public string PluralLabel { get; set; } = string.Empty;
    public string WalletId { get; set; } = string.Empty;

    [ObservableProperty]
    public int _index;

    [ObservableProperty]
    public int _totalRows;

    [ObservableProperty]
    private string _label = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DecreaseByOneCommand), nameof(MoveUpCommand), nameof(MoveDownCommand))]
    private int _quantity;

    [ObservableProperty]
    public bool _use_ExchangeUp = true;
    
    [ObservableProperty]
    public bool _use_ExchangeDown = true;

    [ObservableProperty]
    public bool _canExchange = true;

    public DenominationRowViewModel(IWalletManager<Wallet> walletManager, ICurrencyConverter currencyConverter)
    {
        _WalletManager = walletManager;
        _CurrencyConverter = currencyConverter;
    }
    public void Configure(Denomination denomination, string walletId, int index, int totalRows, string singularLabel, string pluralLabel)
    {
        Denomination = denomination;
        WalletId = walletId;
        Index = index;
        TotalRows = totalRows;
        SingularLabel = singularLabel;
        PluralLabel = pluralLabel;
        Label = pluralLabel;
    }

    partial void OnIndexChanged(int value) => HandleIndexOrTotalRowsChange();
    partial void OnTotalRowsChanged(int value) => HandleIndexOrTotalRowsChange();
    partial void OnQuantityChanged(int value) => Label = value == 1 ? SingularLabel : PluralLabel;
    partial void OnLabelChanged(string? oldValue, string newValue) => Label = (oldValue == null || !oldValue.Equals(newValue)) ? StringHelpers.CapitalizeFirstLetter(newValue) : Label;

    /// <summary>
    /// This method is used to permanently disable the ability to exchange up or down for this row.
    /// </summary>
    private void HandleIndexOrTotalRowsChange()
    {
        if (Index == 1) Use_ExchangeUp = false;
        if (TotalRows > 0 && Index == TotalRows) Use_ExchangeDown = false;
    }

    [RelayCommand]
    private void IncreaseByOne()
    {
        var wallet = _WalletManager.UpdateWallet(WalletId, Denomination, 1);
        UpdateQuantityFromWallet(wallet);
    }

    [RelayCommand(CanExecute = nameof(IsNotEmpty))]
    private void DecreaseByOne()
    {
        // Could pre-check Wallet denomination balance, but IsNotEmpty() is effectively performing that task
        var wallet = _WalletManager.UpdateWallet(WalletId, Denomination, -1); // Throws exception
        UpdateQuantityFromWallet(wallet);
    }

    [RelayCommand(CanExecute = nameof(CanConvertUp))]
    private void MoveUp()
    {
        var requiredQuantity = _CurrencyConverter.ConvertUp(Denomination);
        _WalletManager.UpdateWallet(WalletId, Denomination, -requiredQuantity); // take out req'd qty from wallet
        var wallet = _WalletManager.UpdateWallet(WalletId, Denomination+1, 1);
        UpdateQuantityFromWallet(wallet);
    }

    [RelayCommand(CanExecute = nameof(IsNotEmpty))]
    private void MoveDown()
    {
        // Could pre-check Wallet denomination balance, but IsNotEmpty() is effectively performing that task
        _WalletManager.UpdateWallet(WalletId, Denomination, -1); // take out 1 of the denomination to convert down
        var convertedQuantity = _CurrencyConverter.ConvertDown(Denomination);
        var wallet = _WalletManager.UpdateWallet(WalletId, Denomination-1, convertedQuantity);
        UpdateQuantityFromWallet(wallet);
    }

    /// <summary>
    /// Temporary helper method to update the 'Quantity' ObservableProperty -> eventually shift to message system
    /// </summary>
    /// <param name="wallet"></param>
    private void UpdateQuantityFromWallet(Wallet wallet) => Quantity = WalletAccessor.Access(wallet).GetDenominationQuantity(Denomination);
    
    /// <summary>
    /// Helper function for RelayCommand CanExecute
    /// </summary>
    /// <returns>True if Quantity is greater than 0, else False</returns>
    private bool IsNotEmpty() => Quantity > 0;

    /// <summary>
    /// Helper function for RelayCommand CanExecute
    /// </summary>
    /// <returns>True if Quantity is greater than or equal to required amount for converting to larger denomination</returns>
    private bool CanConvertUp() => Quantity >= _CurrencyConverter.ConvertUp(Denomination);

}
