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
                services.AddTransient<ICurrencyConverter, BasicCurrencyConverter>();

                services.AddSingleton<MainWindow>();
                services.AddGenericFactory<ChildWindow>(); // Child window is same as MainWindow, could be removed
                
                services.AddTransient<WalletPageViewModel>();
                services.AddGenericFactory<WalletPage>();

                services.AddTransient<TotalRowViewModel>();
                services.AddGenericFactory<TotalRow>();

                services.AddTransient<DenominationRowViewModel>();
                services.AddGenericFactory<DenominationRow>(); // Factory Interf
            })
            .Build();
        Debug.WriteLine("Registered object graph");
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var walletManager = AppHost.Services.GetRequiredService<IWalletManager<Wallet>>();
        walletManager.CreateWallet();

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
