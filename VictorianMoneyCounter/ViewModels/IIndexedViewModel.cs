namespace VictorianMoneyCounter.ViewModels;


// TODO: Introduce an interface for ViewModels to decouple from the concrete implementations
public interface IIndexedViewModel
{
    int Index { get; set; }
}
