using System.Collections.Generic;
using System;
using System.Diagnostics;
using VictorianMoneyCounterUWP.Model.Aggregates;

namespace VictorianMoneyCounterUWP.Service
{

    public class WalletManager : IWalletManager<Wallet>
    {
        // Replace Dictionary with repository
        private readonly Dictionary<string, Wallet> _wallets;
        //private readonly List<Action> _subscribers = [];
        private readonly Dictionary<string, List<Action>> _subscribers; // subscribers stored against walletId

        public WalletManager()
        {
            _wallets = new Dictionary<string, Wallet>();
            _subscribers = new Dictionary<string, List<Action>>();
            CreateWallet(); // create a default wallet - or check for existing wallets from repository
        }

        /// <summary>
        /// Create a new Wallet
        /// </summary>
        /// <returns>The ID of the created Wallet</returns>
        /// <exception cref="Exception"></exception>
        public string CreateWallet()
        {
            var wallet = new Wallet();
            if (_wallets.TryAdd(wallet.Id, wallet) &&
                _subscribers.TryAdd(wallet.Id, new List<Action>()))
            {
                Debug.WriteLine("Created a new Wallet: " + wallet.Id);
                return wallet.Id;
            }
            else
            {
                throw new Exception("Failed to create new wallet");
            }
        }

        /// <summary>
        /// Finds a Wallet by Id or if no Id provided, returns the first wallet in the Dict.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Wallet FindWallet(string id = "")
        {
            if (_wallets.Count == 0)
                throw new Exception("Internal error"); // There should always be a wallet

            if (id == null || id == string.Empty)
            {
                //return _wallets.Values.First<Wallet>();
                foreach (var w in _wallets)
                {
                    return w.Value;
                }
            }

            if (!_wallets.TryGetValue(id, out var wallet))
                throw new Exception("No wallet with that ID");
            else
                return wallet;
        }

        /// <summary>
        /// Destroy a Wallet
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if the wallet was successfully removed, False if not</returns>
        public bool RemoveWallet(string id)
        {
            if (!_wallets.ContainsKey(id))
                return false;

            return _wallets.Remove(id);
        }

        /// <summary>
        /// Create a transaction on a Wallet by ID, Denomination, and transaction value (+ve/-ve).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="denomination"></param>
        /// <param name="changeAmount"></param>
        /// <returns>Immutable Wallet Instance with updated values or Error</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Wallet UpdateWallet(string id, Denomination denomination, int changeAmount)
        {
            if (!_wallets.TryGetValue(id, out var wallet))
                throw new InvalidOperationException($"Wallet ID: {id} is not recognized");

            if (WalletAccessor.Access(wallet).GetDenominationQuantity(denomination) + changeAmount < 0)
                throw new InvalidOperationException($"Can not complete transaction of {denomination}: Insufficient balance!");

            return SaveWallet(WalletAccessor.Access(wallet).UpdateDenominationQuantity(denomination, changeAmount));
        }

        /// <summary>
        /// This method would eventually encompass interactions with other service or repository for persistence,
        /// Currently updates the Dictionary holder in the WalletManager service class.
        /// </summary>
        /// <param name="wallet"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private Wallet SaveWallet(Wallet wallet)
        {
            if (!_wallets.ContainsKey(wallet.Id))
            {
                throw new InvalidOperationException($"Wallet ID: {wallet.Id} is not recognized");
            }

            _wallets[wallet.Id] = wallet;

            NotifySubscribers(wallet.Id);

            return wallet;
        }

        /// <summary>
        /// Register a subscriber delegate function to be invoked when the wallet is updated.
        /// </summary>
        /// <param name="subscriber"></param>
        //public void RegisterSubscriber(string walletId, Action subscriber) => _subscribers.Add(subscriber);
        public void RegisterSubscriber(string walletId, Action subscriber)
        {
            if (!_subscribers.TryGetValue(walletId, out var value))
                throw new InvalidOperationException($"Unrecognized Wallet ID: {walletId}");

            value.Add(subscriber);
        }

        /// <summary>
        /// Unregister subscriber delegate functions for Wallet ID
        /// </summary>
        /// <param name="walletId"></param>
        public void UnregisterSubscribers(string walletId)
        {
            if (!_subscribers.ContainsKey(walletId))
                // Wallet does not exist
                return;

            _subscribers.Remove(walletId);
        }

        /// <summary>
        /// Trigger delegate functions of all subscribers
        /// </summary>
        private void NotifySubscribers(string walletId)
        {
            // TODO: Optimize notifying subscribers to only notify subscribers that will care
            // ie. no point calling an update on every row if only one row needs to be updated.
            //
            // subscribers should be grouped by wallet, right now all wallets are updating on any wallet change

            //foreach (var subscriber in _subscribers)
            //    subscriber.Invoke();

            if (!_subscribers.TryGetValue(walletId, out var subscribersList))
                throw new InvalidOperationException("Unrecognized Wallet ID");

            subscribersList.ForEach(s => s.Invoke());
        }
    }
}
