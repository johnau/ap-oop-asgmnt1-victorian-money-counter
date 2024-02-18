using VictorianMoneyCounter.Model.Aggregates;

namespace VictorianMoneyCounter.XunitTests;

/// <summary>
/// Naming tests conventions from Microsoft Best practices
/// The name of your test should consist of three parts: The name of the method being tested. The scenario under which it's being tested. The expected behavior when the scenario is invoked.
/// </summary>
public class WalletTests
{
    [Fact]
    public void WithPoundsTransaction_AddPounds_WillSucceed()
    {
        var wallet = new Wallet()
        {
            Pounds = 10,
        };
        var result = wallet.WithPoundsTransaction(1);
        Assert.Equal(11, result.Pounds);
    }

    [Fact]
    public void WithPoundsTransaction_SubtractPounds_WillSucceed()
    {
        var wallet = new Wallet()
        {
            Pounds = 10,
        };
        var result = wallet.WithPoundsTransaction(-1);
        Assert.Equal(9, result.Pounds);
    }

    [Fact]
    public void WithPoundsTransaction_SubtractPoundsNegative_WillFailSilentlyAsExpected()
    {
        var wallet = new Wallet()
        {
            Pounds = 10,
        };
        var result = wallet.WithPoundsTransaction(-11);
        Assert.Equal(10, result.Pounds);
    }

    [Fact]
    public void WithCrownsTransaction_AddCrowns_WillSucceed()
    {
        var wallet = new Wallet() { Crowns = 10 };
        var result = wallet.WithCrownsTransaction(1);
        Assert.Equal(11, result.Crowns);
    }

    [Fact]
    public void WithCrownsTransaction_SubtractCrowns_WillSucceed()
    {
        var wallet = new Wallet() { Crowns = 10 };
        var result = wallet.WithCrownsTransaction(-1);
        Assert.Equal(9, result.Crowns);
    }

    [Fact]
    public void WithCrownsTransaction_SubtractCrownsNegative_WillFailSilentlyAsExpected()
    {
        var wallet = new Wallet() { Crowns = 10 };
        var result = wallet.WithCrownsTransaction(-11);
        Assert.Equal(10, result.Crowns);
    }

    [Fact]
    public void WithShillingsTransaction_AddShillings_WillSucceed()
    {
        var wallet = new Wallet() { Shillings = 10 };
        var result = wallet.WithShillingsTransaction(1);
        Assert.Equal(11, result.Shillings);
    }

    [Fact]
    public void WithShillingsTransaction_SubtractShillings_WillSucceed()
    {
        var wallet = new Wallet() { Shillings = 10 };
        var result = wallet.WithShillingsTransaction(-1);
        Assert.Equal(9, result.Shillings);
    }

    [Fact]
    public void WithShillingsTransaction_SubtractShillingsNegative_WillFailSilentlyAsExpected()
    {
        var wallet = new Wallet() { Shillings = 10 };
        var result = wallet.WithShillingsTransaction(-11);
        Assert.Equal(10, result.Shillings);
    }

    [Fact]
    public void WithPenceTransaction_AddPence_WillSucceed()
    {
        var wallet = new Wallet() { Pence = 10 };
        var result = wallet.WithPenceTransaction(1);
        Assert.Equal(11, result.Pence);
    }

    [Fact]
    public void WithPenceTransaction_SubtractPence_WillSucceed()
    {
        var wallet = new Wallet() { Pence = 10 };
        var result = wallet.WithPenceTransaction(-1);
        Assert.Equal(9, result.Pence);
    }

    [Fact]
    public void WithPenceTransaction_SubtractPenceNegative_WillFailSilentlyAsExpected()
    {
        var wallet = new Wallet() { Pence = 10 };
        var result = wallet.WithPenceTransaction(-11);
        Assert.Equal(10, result.Pence);
    }

    [Fact]
    public void WithFarthingsTransaction_AddFarthings_WillSucceed()
    {
        var wallet = new Wallet() { Farthings = 10 };
        var result = wallet.WithFarthingsTransaction(1);
        Assert.Equal(11, result.Farthings);
    }

    [Fact]
    public void WithFarthingsTransaction_SubtractFarthings_WillSucceed()
    {
        var wallet = new Wallet() { Farthings = 10 };
        var result = wallet.WithFarthingsTransaction(-1);
        Assert.Equal(9, result.Farthings);
    }

    [Fact]
    public void WithFarthingsTransaction_SubtractFarthingsNegative_WillFailSilentlyAsExpected()
    {
        var wallet = new Wallet() { Farthings = 10 };
        var result = wallet.WithFarthingsTransaction(-11);
        Assert.Equal(10, result.Farthings);
    }
}