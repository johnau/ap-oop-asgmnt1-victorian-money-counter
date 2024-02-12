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

    public DenominationRowViewModel(IWalletManager<Wallet> walletManager)
    {
        _WalletManager = walletManager;
    }
    public void Configure(string walletId, int index, int totalRows, string singularLabel, string pluralLabel)
    {
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

    //[RelayCommand]
    //private void IncreaseByOne() => Quantity += 1; 
    [RelayCommand]
    private void IncreaseByOne()
    {
        var wallet = _WalletManager.UpdateWallet(WalletId, (Denomination)Index, 1);
        Quantity = WalletAccessor.Access(wallet).GetDenominationQuantity((Denomination)Index);
        //Quantity += 1;
    }

    [RelayCommand(CanExecute = nameof(IsNotEmpty))]
    private void DecreaseByOne()
    {
        var wallet = _WalletManager.UpdateWallet(WalletId, (Denomination)Index, -1);
        Quantity = WalletAccessor.Access(wallet).GetDenominationQuantity((Denomination)Index);
        //Quantity -= Quantity > 0 ? 1 : 0; // Method already protected, so this check is not really required
    }

    [RelayCommand(CanExecute = nameof(CanConvertUp))]
    private void MoveUp() => Quantity -= CanConvertUp() ? DenominationInfoFactory.ConvertDenominationUp((Denomination)Index) : 0;

    [RelayCommand(CanExecute = nameof(IsNotEmpty))]
    private void MoveDown() => Quantity -= Quantity > 0 ? 1 : 0;
    private bool IsNotEmpty() => Quantity > 0;
    private bool CanConvertUp() => Quantity >= DenominationInfoFactory.ConvertDenominationUp((Denomination)Index);

}
