namespace VictorianMoneyCounter.StartupHelpers
{
    public interface IAbstractFactory<T>
    {
        T Create();
    }
}