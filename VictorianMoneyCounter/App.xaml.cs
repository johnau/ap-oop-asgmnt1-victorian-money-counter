using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Windows;
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.Service;
using VictorianMoneyCounter.StartupHelpers;
using VictorianMoneyCounter.ViewModels;
using VictorianMoneyCounter.Views;

namespace VictorianMoneyCounter;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IWalletManager<Wallet>, WalletManager>();
                //services.AddGenericFactory<Wallet>();
                
                services.AddSingleton<MainWindow>();
                services.AddFormFactory<ChildWindow>(); // Child Window is essentially the same as MainWindow
                
                services.AddTransient<WalletPageViewModel>();
                services.AddGenericFactory<WalletPage>(); // Factory for WalletPage so multiple wallet views can be displayed

                services.AddTransient<DenominationRowViewModel>();
                services.AddGenericFactory<DenominationRow>(); // Factory for Denomination Row as many will be needed
                //services.AddTransient<IDataAccess, DataAccess>();
            })
            .Build();
        Debug.WriteLine("Registered object graph");
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        IWalletManager<Wallet> walletManager = AppHost.Services.GetRequiredService<IWalletManager<Wallet>>();
        string id = walletManager.CreateNewWallet();

        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        startupForm.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }

}
