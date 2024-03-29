﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.StartupHelpers;
using VictorianMoneyCounter.ViewModels;

namespace VictorianMoneyCounter.Views;

/// <summary>
/// View of a Victorian Wallet
///
/// TODO: Separate out some logic from viewmodel of denominationrow that are not part of the assignment
/// so it is easier to see whats going on
/// 
/// </summary>
public partial class WalletPage : Page, IViewModelBacked<WalletPageViewModel>
{
    private readonly IAbstractFactory<DenominationRow> _DenominationRowFactory; // DenominationRow can be interfaced
    private readonly IAbstractFactory<TotalRow> _TotalRowFactory;

    private WalletPageViewModel ViewModel => (WalletPageViewModel)DataContext;

    /// <summary>
    /// Primary constructor
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="denominationRowFactory"></param>
    /// <param name="totalRowFactory"></param>
    public WalletPage(WalletPageViewModel viewModel, 
                        IAbstractFactory<DenominationRow> denominationRowFactory,
                        IAbstractFactory<TotalRow> totalRowFactory)
    {
        DataContext = viewModel;
        _DenominationRowFactory = denominationRowFactory;
        _TotalRowFactory = totalRowFactory;
        InitializeComponent();
        Loaded += ConfigureWindow;
        Loaded += ConfigureTotalRow;
        Loaded += ConfigureDenominationRows;
        Loaded += ConfigureShortcuts;
        Unloaded += Cleanup;
    }

    /// <summary>
    /// Configure window title based on wallet data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ConfigureWindow(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);
        if (window != null) window.Title += $" - {ViewModel.WalletId}";
    }

    /// <summary>
    /// Configure / Build the Total row (top of view)
    /// </summary>
    private void ConfigureTotalRow(object sender, RoutedEventArgs e)
    {
        var totalRow = _TotalRowFactory.Create();
        var config = new BasicViewModelConfiguration(ViewModel.WalletId);
        totalRow.GetViewModel().Configure(config);

        ViewModel.RegisterChildViewModel(totalRow.GetViewModel());
        
        MainLayoutGrid.Children.Add(totalRow);
        Grid.SetRow(totalRow, 0);
    }

    /// <summary>
    /// Configure / Build the five Denomination rows (rest of the view)
    /// </summary>
    private void ConfigureDenominationRows(object sender, RoutedEventArgs e)
    {
        var _ = Enum.GetValues(typeof(Denomination));
        Array.Reverse(_);
        var _total = _.Length;
        var i = 1;
        foreach (Denomination d in _)
        {
            var denominationRow = _DenominationRowFactory.Create();
            var config = new DenominationRowViewModelConfiguration(Denomination: d,
                                                                WalletId: ViewModel.WalletId,
                                                                Index: i,
                                                                TotalRows: _total,
                                                                SingularLabel: DenominationValue.GetDenominationInfo(d).Singular,
                                                                PluralLabel: DenominationValue.GetDenominationInfo(d).Plural);
            denominationRow.GetViewModel().Configure(config);

            ViewModel.RegisterChildViewModel(denominationRow.GetViewModel()); // var i could also be key

            MainLayoutGrid.Children.Add(denominationRow);
            Grid.SetRow(denominationRow, i++);
        }
    }

    /// <summary>
    /// Configure Keyboard shortcuts for other functions
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ConfigureShortcuts(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);
        window.KeyDown += (s, e) =>
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.N) // Ctrl + N
            {
                if (ViewModel.NewWalletCommand.CanExecute(null))
                    ViewModel.NewWalletCommand.Execute(null);
            }
        };
    }
    
    public WalletPageViewModel GetViewModel() => ViewModel;

    /// <summary>
    /// Cleanup and Unregister subscriptions
    /// </summary>
    private void Cleanup(object sender, RoutedEventArgs e)
    {
        ViewModel.UnregisterChildViewModels();
    }
}
