using System;

namespace VictorianMoneyCounterUWP.Model.Aggregates
{

    /// <summary>
    /// Helper class to interact with Wallet record
    /// </summary>
    public class WalletAccessor
    {
        public static WalletAccessor Access(Wallet wallet)
        {
            return new WalletAccessor(wallet);
        }

        private readonly Wallet _wallet;
        protected WalletAccessor(Wallet wallet)
        {
            _wallet = wallet;
        }

        public int GetDenominationQuantity(Denomination denomination)
        {
            switch (denomination)
            {
                case Denomination.Pound:
                    return _wallet.Pounds;
                case Denomination.Crown:
                    return _wallet.Crowns;
                case Denomination.Shilling:
                    return _wallet.Shillings;
                case Denomination.Penny:
                    return _wallet.Pence;
                case Denomination.Farthing:
                    return _wallet.Farthings;
                default:
                    throw new ArgumentException("Invalid Denominiation");
            }
        }

        public Wallet UpdateDenominationQuantity(Denomination denomination, int changeAmount)
        {
            switch (denomination)
            {
                case Denomination.Pound:
                    return _wallet.WithPoundsTransaction(changeAmount);
                case Denomination.Crown:
                    return _wallet.WithCrownsTransaction(changeAmount);
                case Denomination.Shilling:
                    return _wallet.WithShillingsTransaction(changeAmount);
                case Denomination.Penny:
                    return _wallet.WithPenceTransaction(changeAmount);
                case Denomination.Farthing:
                    return _wallet.WithFarthingsTransaction(changeAmount);
                default:
                    throw new ArgumentException("Invalid Denominiation");
            }
        }
    }
}
