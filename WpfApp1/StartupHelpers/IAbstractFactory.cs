namespace WpfApp1.StartupHelpers
{
    public interface IAbstractFactory<T>
    {
        T Create();
    }
}