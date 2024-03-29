# C# OOP Assignment 1: Victorian Money Counter

- Main Assignment project: /VictorianMoneyCounter
- Mvvm Toolkit test: /MvvmExample
- CDI test: /WpfApp1 + /WpfApp1ClassLibrary

-----------------------

## Features:
- Press Ctrl + N to create a new wallet
- Long press implemented on buttons for quickly incrementing quantities
- Coin animations
- Shake window to shake coins down the denominations -> coming soon
- Ready for a persistence layer to replace current in-memory store

-------------------------

## Notes:
Experimenting with: 
- Community.Toolkit.Mvvm (Model-View-ViewModel pattern)
- Microsoft.Extensions.DependencyInjection (+ .Hosting) (CDI/IoC) <note: part of the .net framework, but not part of .net core>
- Microsoft.Xaml.Behaviors.Wpf (Handle secondary mouse captures)
- WPF Animations/Storyboards
- XAML Bindings
- XAML Resource Styles, Page, UserControl components
- XAML Grid, StackPanel, Canvas

----------------

## TO-DO:
- Testing
- WalletManager

----------------
### Application Flow:
Entry point is in `App.xaml.cs` . `OnStartup(e)`, container configuration in `App()` - create containered object graph

`App.xaml.cs` . `OnStartup(e)` requests `MainWindow` from Container (can't avoid) and launches `MainWindow`

`MainWindow` code behind is injected with instance of `IAbstractFactory<WalletPage>` and creates an instance of `WalletPage` 

`WalletPage` is injected with new instance of `WalletPageViewModel`, which in turn is injected with the Singleton instance of `IWalletManager<Wallet>`, new `Wallet` is created to display on the page (not ideal - but ok)

`WalletPage` has an instance of the `IAbstractFactory<DenominationRow>` and `IAbstractFactory<TotalRow>` and creates required instances. These instances have dependency on the Transient `DenominationRowViewModel`s and `TotalRowViewModel`.

-------------------
### VictorianMoneyCounter.Model namespace/layer

The `Wallet` model is implemented as an immutable c# `record`, and includes simple protections to prevent illegal operations/results.
A `WalletAccessor` helper class has also been included for some basic interactions/adaptions to the `Wallet`

-------------------
### VictorianMoneyCounter.Service namespace/layer

`ICurrencyConverter` and `IWalletManager` and concrete implementations

-------------------
### VictorianMoneyCounter.StartupHelpers namespace

Boilerplate for .Net Dependency Injection for XAML/WPF UserControls/Pages/Windows injection

-------------------

### CommunityToolkit.Mvvm

ViewModel properties for binding are annotated with the ObservableProperty attribute, generating appropriate code for the property to use for Binding from XAML views.  RelayCommands are used on functions required for Binding from XAML views, CanExecute is employed to restrict access to controls to suit project brief.

