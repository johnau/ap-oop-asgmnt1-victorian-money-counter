using VictorianMoneyCounter.Model.Aggregates;

namespace VictorianMoneyCounter.ViewModels;

public record DenominationRowViewModelConfiguration(Denomination Denomination, 
                                                string WalletId, 
                                                int Index, 
                                                int TotalRows, 
                                                string SingularLabel, 
                                                string PluralLabel) { }
