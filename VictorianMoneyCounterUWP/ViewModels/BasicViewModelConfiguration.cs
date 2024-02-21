
namespace VictorianMoneyCounter.ViewModels
{
    public class BasicViewModelConfiguration
    {
        private string WalletId { get; }

        public BasicViewModelConfiguration(string walletId)
        {
            WalletId = walletId;
        }
    }
}

