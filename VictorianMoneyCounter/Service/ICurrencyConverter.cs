using VictorianMoneyCounter.Model.Aggregates;

namespace VictorianMoneyCounter.Service
{
    public interface ICurrencyConverter
    {
        int ConvertDown(Denomination denomination, int amount = 1);
        int ConvertUp(Denomination denomination, int amount = 1);
    }
}