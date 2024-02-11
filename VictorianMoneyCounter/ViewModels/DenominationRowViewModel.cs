using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.Utilities;

namespace VictorianMoneyCounter.ViewModels;
public partial class DenominationRowViewModel : ObservableObject, IIndexedViewModel
{
    public string SingularLabel { get; set; } = string.Empty;
    public string PluralLabel { get; set; } = string.Empty;
    public string WalletId { get; set; } = string.Empty;

    [ObservableProperty]
    public int _index;

    [ObservableProperty]
    public int _totalRows;

    [ObservableProperty]
    private string _label = string.Empty; // label for the denomination

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(
        nameof(DecreaseByOneCommand), 
        nameof(MoveUpCommand), 
        nameof(MoveDownCommand))
    ]
    private int _quantity;

    [ObservableProperty]
    public bool _use_ExchangeUp = true;
    
    [ObservableProperty]
    public bool _use_ExchangeDown = true;

    [ObservableProperty]
    public bool _canExchange = true;

    public DenominationRowViewModel()
    {
        Debug.WriteLine("New DenominationRowViewModel instasntiated: " + this.GetHashCode());
    }
    partial void OnIndexChanged(int value) => HandleIndexOrTotalRowsChange();
    partial void OnTotalRowsChanged(int value) => HandleIndexOrTotalRowsChange();
    partial void OnQuantityChanged(int value) => Label = value == 1 ? SingularLabel : PluralLabel;
    partial void OnLabelChanged(string? oldValue, string newValue) => Label = (oldValue == null || !oldValue.Equals(newValue)) ? StringHelpers.CapitalizeFirstLetter(newValue) : Label;

    /// <summary>
    /// This method is used to hide/disable
    /// </summary>
    private void HandleIndexOrTotalRowsChange()
    {
        if (Index == 1)
        {
            Use_ExchangeUp = false;
        }
        if (TotalRows > 0 && Index == TotalRows)
        {
            Use_ExchangeDown = false;
        }
    }

    private bool IsNotEmpty() => Quantity > 0;
    private bool CanConvertUp() => Quantity >= DenominationInfoFactory.ConvertDenominationUp((Denomination)Index);

    [RelayCommand]
    private void IncreaseByOne() => Quantity += 1;

    [RelayCommand(CanExecute = nameof(IsNotEmpty))]
    private void DecreaseByOne() => Quantity -= Quantity > 0 ? 1 : 0; // Method already protected, so this check is not really required

    [RelayCommand(CanExecute = nameof(CanConvertUp))]
    private void MoveUp() => Quantity -= CanConvertUp() ? DenominationInfoFactory.ConvertDenominationUp((Denomination)Index) : 0;

    [RelayCommand(CanExecute = nameof(IsNotEmpty))]
    private void MoveDown() => Quantity -= Quantity > 0 ? 1 : 0;

    public void Configure(string walletId, int index, int totalRows, string singularLabel, string pluralLabel)
    {
        WalletId = walletId;
        Index = index;
        TotalRows = totalRows;
        SingularLabel = singularLabel;
        PluralLabel = pluralLabel;
        Label = pluralLabel;
    }
}
