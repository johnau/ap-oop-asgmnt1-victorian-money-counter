using VictorianMoneyCounterUWP.Model.Aggregates;

namespace VictorianMoneyCounterUWP.ViewModels
{
    public class DenominationRowViewModelConfiguration
    {
        public Denomination Denomination { get; }
        public string WalletId { get; }
        public int Index { get; }
        public int TotalRows { get; }
        public string SingularLabel { get; }
        public string PluralLabel { get; }

        public DenominationRowViewModelConfiguration(Denomination denomination,
                                                    string walletId,
                                                    int index,
                                                    int totalRows,
                                                    string singularLabel,
                                                    string pluralLabel)
        {
            Denomination = denomination;
            WalletId = walletId;
            Index = index;
            TotalRows = totalRows;
            SingularLabel = singularLabel;
            PluralLabel = pluralLabel;
        }
    }

}

