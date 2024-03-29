﻿using CommunityToolkit.Mvvm.ComponentModel;
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.Service;

namespace VictorianMoneyCounter.ViewModels;

public partial class TotalRowViewModel : ObservableObject, IUpdatableViewModel, IConfigurableViewModel<BasicViewModelConfiguration>
{
    private readonly IWalletManager<Wallet> _WalletManager;
    private readonly ICurrencyConverter _CurrencyConverter;
    public string WalletId { get; private set; } = string.Empty;

    [ObservableProperty]
    private string _totalString = "£0 0c 0s 0d 0f";
    
    /// <summary>
    /// Primary constructor
    /// </summary>
    /// <param name="walletManager"></param>
    /// <param name="currencyConverter"></param>
    public TotalRowViewModel(IWalletManager<Wallet> walletManager, ICurrencyConverter currencyConverter)
    {
        _WalletManager = walletManager;
        _CurrencyConverter = currencyConverter;
    }

    public void Configure(BasicViewModelConfiguration config)
    {
        WalletId = config.WalletId;
    }

    /// <summary>
    /// Call to force an update on the model
    /// </summary>
    public void Update()
    {
        var wallet = _WalletManager.FindWallet(WalletId);
        UpdateQuantitiesFromWallet(wallet);
        //Debug.WriteLine($">> {Denomination}={Quantity}");
    }

    /// <summary>
    /// Temporary helper method to update the 'Quantity' ObservableProperty -> eventually shift to message system
    /// </summary>
    /// <param name="wallet"></param>
    private void UpdateQuantitiesFromWallet(Wallet wallet)
    {
        var quantities = new Dictionary<Denomination, int>();
        var _ = Enum.GetValues(typeof(Denomination));
        Array.Reverse(_);
        foreach (Denomination d in _)
        {
            var q = WalletAccessor.Access(wallet).GetDenominationQuantity(d);
            quantities.Add(d, q);
        }

        // consolidate quantities dictionary to largest denominations
        var consolidated = _CurrencyConverter.ConsolidateQuantities(quantities);

        TotalString = string.Join(" ", consolidated.Select((kvp, index) =>
            index == 0 ? $"{DenominationValue.GetDenominationInfo(kvp.Key).Symbol}{kvp.Value}" :
                            $"{kvp.Value}{DenominationValue.GetDenominationInfo(kvp.Key).Symbol}"));

        var totalItems = consolidated.Values.Sum();
        if (totalItems == 0)
        {
            TotalString += " :(";
        }
    }

}
