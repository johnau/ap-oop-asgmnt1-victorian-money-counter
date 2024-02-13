namespace VictorianMoneyCounter.Views;

public interface IViewModelBacked<Vm>
{
    Vm GetViewModel();
}
