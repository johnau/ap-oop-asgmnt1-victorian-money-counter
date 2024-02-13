using System.Diagnostics;
using VictorianMoneyCounter.Model.Aggregates;

namespace VictorianMoneyCounter.Service;

public class WalletManager : IWalletManager<Wallet>
{
    // Replace Dictionary with repository
    private readonly Dictionary<string, Wallet> _wallets = [];
    private readonly List<Action> _subscribers = [];

    public WalletManager()
    {
        CreateWallet(); // default wallet
    }

    /// <summary>
    /// Create a new Wallet
    /// </summary>
    /// <returns>The ID of the created Wallet</returns>
    /// <exception cref="Exception"></exception>
    public string CreateWallet()
    {
        var wallet = new Wallet();
        if (_wallets.TryAdd(wallet.Id, wallet))
        {
            Debug.WriteLine("Created a new Wallet: " + wallet.Id);
            return wallet.Id;
        } else
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
    public Wallet FindWalletById(string id = "")
    {
        if (_wallets.Count == 0)
            throw new Exception("Internal error"); // There should always be a wallet

        if (id == null || id == string.Empty)
        {
            return _wallets.Values.First<Wallet>();
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

        //Debug.WriteLine("==Wallet Update==");
        NotifySubscribers();

        return wallet;
    }

    /// <summary>
    /// Register a subscriber delegate function to be invoked when the wallet is updated
    /// </summary>
    /// <param name="subscriber"></param>
    public void RegisterSubscriber(Action subscriber) => _subscribers.Add(subscriber);

    /// <summary>
    /// Trigger delegate functions of all subscribers
    /// </summary>
    private void NotifySubscribers()
    {
        // TODO: Optimize notifying subscribers to only notify subscribers that will care
        // ie. no point calling an update on every row if only one row needs to be updated.
        foreach (var subscriber in _subscribers)
            subscriber.Invoke();
    }
}
