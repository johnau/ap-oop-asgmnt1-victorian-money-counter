namespace VictorianMoneyCounterUWP.ViewModels
{
    public interface IConfigurableViewModel<C>
    {
        void Configure(C configuration);
    }
}

