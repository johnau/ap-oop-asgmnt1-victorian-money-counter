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

App.xaml.cs > OnStartup(StartupEventArgs e) launches MainWindow

MainWindow has instance of MainPage Factory and creates an instance of MainPage 

MainPage is injected with new instance of MainPageViewModel, which in turn is injected with the instance of IWalletManager<Wallet>

MainPageViewModel creates a new Wallet for itself with IWalletManager.

**********
Should the MainPageViewModel be creating the wallet?
Should a wallet being created be an event that is listened to by the view components, and as a result a new page spins up?

When the program starts up, a wallet can be created (always need at least one).  At this point, when the views load up, the MainPageViewModel can grab the first (and only) Wallet in the holder.  (In future if permanent data store is introduced, it can grab the first, and if there are more, other windows  can be automatically opened)

Once the wallet is created it can either announce its creation through message system... could create simple one or work out the one in Mvvm toolkit... 
ie.  
User presses Ctrl + N, New window is created, new MainPage is created.
Then request for new wallet is sent. New wallet is created, message response with ID.

**********

MainPage has an instance of the Row Factory and creates required instances of the DenominationRows, which in turn are each injected with transient instances of DenominationRowViewModel objects.


