# Assignment 1: Victorian Money Counter

Notes:
Experimenting with Community.Toolkit.Mvvm
Experimenting with Microsoft.Extensions.DependencyInjection (+ .Hosting)

- Using Factory pattern and Generics registered in Container for instances of WPF components (Window, Page, UserControl)
- ViewModels are registered as Transients for fresh instantiation for each class with the dependency
- Other components are simply registered as Transients or Singletons for now.

Application Flow:
Entry point is in App.xaml.cs > App()

Object graph is registered to Container

App.xaml.cs > OnStartup(StartupEventArgs e) creates Wallet with IWalletManager and launches MainWindow

MainWindow code behind is injected with instance of IAbstractFactory<WalletPage> and creates an instance of WalletPage 

WalletPage is injected with new instance of WalletPageViewModel, which in turn is injected with the `Singleton` instance of IWalletManager<Wallet>

WalletPageViewModel accesses Wallet from IWalletManager (currently by grabbing the first and only wallet - multiple wallets not yet handled properly).

WalletPage has an instance of the IAbstractFactory<DenominationRow> and creates required instances of the DenominationRows(5), which in turn are each injected with transient instances of DenominationRowViewModel objects.

-------------------
## Model layer

The Wallet model is implemented as an immutable record, and includes simple protections to prevent illegal operations/results.
A WalletAccessor helper class has also been included for some basic interactions/adaptions to the Wallet record

-------------------
## Service layer

ICurrencyConverter and IWalletManager implementations

*** IValueCalculator service may also live here - required for next step

-------------------
## StartupHelpers

Some boilerplate for .Net Dependency Injection

-------------------

## CommunityToolkit.Mvvm

ViewModel properties for binding are annotated with the ObservableProperty attribute, generating appropriate code for the property to use for Binding from XAML views.  RelayCommands are used on functions required for Binding from XAML views, CanExecute is employed to restrict access to controls to suit project brief.

