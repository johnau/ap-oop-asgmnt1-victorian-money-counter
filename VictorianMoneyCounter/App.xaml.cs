using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Windows;
using VictorianMoneyCounter.Model.Aggregates;
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
                services.AddGenericFactory<Wallet>();
                services.AddSingleton<MainWindow>();
                services.AddTransient<MainPageViewModel>();
                services.AddGenericFactory<MainPage>();
                services.AddTransient<DenominationRowViewModel>();
                services.AddUserControlFactory<DenominationRow>();
                //services.AddTransient<IDataAccess, DataAccess>();
            })
            .Build();
        Debug.WriteLine("Registered object graph");
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

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
