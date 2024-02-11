using Microsoft.Extensions.DependencyInjection;

namespace VictorianMoneyCounter.StartupHelpers;

public static class ServiceExtensions
{
    public static void AddFormFactory<TForm>(this IServiceCollection services) 
        where TForm : class
    {
        services.AddTransient<TForm>();
        services.AddSingleton<Func<TForm>>(x => () => x.GetService<TForm>()!);
        services.AddSingleton<IAbstractFactory<TForm>, AbstractFactory<TForm>>();
    }

    public static void AddUserControlFactory<TUserControl>(this IServiceCollection services)
        where TUserControl : class
    {
        services.AddTransient<TUserControl>();
        services.AddSingleton<Func<TUserControl>>(x => () => x.GetService<TUserControl>()!);
        services.AddSingleton<IAbstractFactory<TUserControl>, AbstractFactory<TUserControl>>();
    }

    public static void AddGenericFactory<T>(this IServiceCollection services)
        where T : class
    {
        services.AddTransient<T>();
        services.AddSingleton<Func<T>>(x => () => x.GetService<T>()!);
        services.AddSingleton<IAbstractFactory<T>, AbstractFactory<T>>();
    }
}
