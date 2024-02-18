
using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.Service;

namespace VictorianMoneyCounter.XunitTests;

public class WalletManagerTests
{
    [Fact]
    public void CreateWallet_x_WillSucceed()
    {
        var walletManager = new WalletManager();
        var walletId = walletManager.CreateWallet();

        Assert.NotNull(walletId);
    }

    [Fact]
    public void FindWallet_WithValidId_WillSucceed()
    {
        var walletManager = new WalletManager();
        var walletId = walletManager.CreateWallet();
        var wallet = walletManager.FindWallet(walletId);

        Assert.NotNull(wallet);
    }

    [Fact]
    public void FindWallet_GetFirst_WillSucceed()
    {
        var walletManager = new WalletManager();
        walletManager.CreateWallet();
        var wallet = walletManager.FindWallet();

        Assert.NotNull(wallet);
    }

    [Fact]
    public void FindWallet_WithInvalidId_WillFail()
    {
        var walletManager = new WalletManager();
        Assert.ThrowsAny<Exception>(() => walletManager.FindWallet("invalid_id"));
    }

    [Fact]
    public void RemoveWallet_WithValidId_WillRemove()
    {
        var walletManager = new WalletManager();
        var walletId = walletManager.CreateWallet();
        var removed = walletManager.RemoveWallet(walletId);

        Assert.True(removed);
    }

    [Fact]
    public void RemoveWallet_WithInvalidId_WillFail()
    {
        var walletManager = new WalletManager();
        var removed = walletManager.RemoveWallet("invalid_id");

        Assert.False(removed);
    }

    [Fact]
    public void UpdateWallet_WithPounds_WillSucceed()
    {
        var walletManager = new WalletManager();
        var walletId = walletManager.CreateWallet();
        var wallet = walletManager.UpdateWallet(walletId, Denomination.Pound, 1_000_000);

        Assert.Equal(1_000_000, wallet.Pounds);
    }

    [Fact]
    public void UpdateWallet_WithInvalidWalletId_WillFail()
    {
        var walletManager = new WalletManager();

        Assert.Throws<InvalidOperationException>(() => walletManager.UpdateWallet("invalid_id", Denomination.Pound, 1_000_000));
    }

    [Fact]
    public void UpdateWallet_WithWithdrawLessThanBalance_WillSucceed()
    {
        var walletManager = new WalletManager();
        var walletId = walletManager.CreateWallet();
        walletManager.UpdateWallet(walletId, Denomination.Pound, 1_000_000);
        walletManager.UpdateWallet(walletId, Denomination.Pound, -5);

        Assert.Equal(1_000_000 - 5, walletManager.FindWallet(walletId).Pounds);
    }

    [Fact]
    public void UpdateWallet_WithWithdrawExceedingBalance_WillFail()
    {
        var walletManager = new WalletManager();
        var walletId = walletManager.CreateWallet();
        walletManager.UpdateWallet(walletId, Denomination.Pound, 5);

        Assert.Throws<InvalidOperationException>(() => walletManager.UpdateWallet(walletId, Denomination.Pound, -1_000_000));
    }

}
