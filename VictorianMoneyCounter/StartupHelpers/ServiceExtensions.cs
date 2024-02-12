using Microsoft.Extensions.DependencyInjection;

namespace VictorianMoneyCounter.StartupHelpers;

public static class ServiceExtensions
{
    public static void AddGenericFactory<T>(this IServiceCollection services)
        where T : class
    {
        services.AddTransient<T>();
        services.AddSingleton<Func<T>>(x => () => x.GetService<T>()!); // Service Locator function for AbstractFactory
        services.AddSingleton<IAbstractFactory<T>, AbstractFactory<T>>();
    }
}
