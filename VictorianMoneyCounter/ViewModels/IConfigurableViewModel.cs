namespace VictorianMoneyCounter.ViewModels;

public interface IConfigurableViewModel<C>
{
    void Configure(C configuration);
}
