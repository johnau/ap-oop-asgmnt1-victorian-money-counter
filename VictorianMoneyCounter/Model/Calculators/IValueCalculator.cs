using VictorianMoneyCounter.Model.Aggregates;

namespace VictorianMoneyCounter.Model.Calculators
{
    public interface IValueCalculator
    {
        Wallet CalculateTotalValue(Wallet wallet);
    }
}