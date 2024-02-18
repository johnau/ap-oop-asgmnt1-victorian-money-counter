using VictorianMoneyCounter.Model.Aggregates;
using VictorianMoneyCounter.Service;

namespace VictorianMoneyCounter.XunitTests;

public class BasicCurrencyConverterTests
{
    [Theory]
    [InlineData(Denomination.Pound, Denomination.Crown, 1, 4, 0, 0)]
    [InlineData(Denomination.Pound, Denomination.Shilling, 1, 20, 0, 0)]
    [InlineData(Denomination.Pound, Denomination.Penny, 1,240, 0, 0)]
    [InlineData(Denomination.Pound, Denomination.Farthing, 1, 960, 0, 0)]
    
    [InlineData(Denomination.Farthing, Denomination.Pound, 960, 1, 0, 0)]
    [InlineData(Denomination.Penny, Denomination.Pound, 240, 1, 0, 0)]
    [InlineData(Denomination.Shilling, Denomination.Pound, 20, 1, 0, 0)]
    [InlineData(Denomination.Crown, Denomination.Pound, 4, 1, 0, 0)]
    
    [InlineData(Denomination.Farthing, Denomination.Penny, 5, 1, 0, 1)]
    [InlineData(Denomination.Farthing, Denomination.Shilling, 50, 1, 0, 2)]
    [InlineData(Denomination.Farthing, Denomination.Crown, 244, 1, 0, 4)]
    [InlineData(Denomination.Farthing, Denomination.Pound, 968, 1, 0, 8)]

    [InlineData(Denomination.Penny, Denomination.Crown, 65, 1, 5, 0)]
    public void Convert_WillReturnCorrectValues(Denomination source, Denomination target, int amount, int expectedTargetQuantity, int expectedRemainderOriginal, int expectedRemainderFarthings)
    {
        var converter = new BasicCurrencyConverter();
        var (quantity, remainderOriginal, remainderFarthings) = converter.Convert(source, target, amount);

        Assert.Equal(expectedTargetQuantity, quantity);
        Assert.Equal(expectedRemainderOriginal, remainderOriginal);
        Assert.Equal(expectedRemainderFarthings, remainderFarthings);
    }

    [Fact]
    public void ConsolidateQuantities_WillReturnCorrectDictionary()
    {
        var converter = new BasicCurrencyConverter();
        var quantities = new Dictionary<Denomination, int>()
            {
                { Denomination.Pound, 1 },
                { Denomination.Crown, 10 },
                { Denomination.Shilling, 23 },
                { Denomination.Penny, 32 },
                { Denomination.Farthing, 68 }
            };
        var result = converter.ConsolidateQuantities(quantities);

        Assert.Equal(4, result[Denomination.Pound]);
        Assert.Equal(3, result[Denomination.Crown]);
        Assert.Equal(2, result[Denomination.Shilling]);
        Assert.Equal(1, result[Denomination.Penny]);
        Assert.Equal(0, result[Denomination.Farthing]);
    }

    [Theory]
    [InlineData(1, 10, 23, 32, 68, 4, 3, 2, 1, 0)]
    public void ConsolidateQuantities_WillReturnCorrectDictionary2(int pd, int cr, int sh, int py, int ft, int ex_pd, int ex_cr, int ex_sh, int ex_py, int ex_ft)
    {
        var converter = new BasicCurrencyConverter();
        var quantities = new Dictionary<Denomination, int>()
            {
                { Denomination.Pound, pd },
                { Denomination.Crown, cr },
                { Denomination.Shilling, sh },
                { Denomination.Penny, py },
                { Denomination.Farthing, ft }
            };
        var result = converter.ConsolidateQuantities(quantities);

        Assert.Equal(ex_pd, result[Denomination.Pound]);
        Assert.Equal(ex_cr, result[Denomination.Crown]);
        Assert.Equal(ex_sh, result[Denomination.Shilling]);
        Assert.Equal(ex_py, result[Denomination.Penny]);
        Assert.Equal(ex_ft, result[Denomination.Farthing]);
    }

    /// <summary>
    /// This test will need to flip at some point, the logic of the tested method (ConvertUp()) will be reversed
    /// </summary>
    /// <param name="denomination"></param>
    /// <param name="amount"></param>
    /// <param name="expectedValue"></param>
    [Theory]
    [InlineData(Denomination.Crown, 1, 4)]
    [InlineData(Denomination.Shilling, 1, 5)]
    [InlineData(Denomination.Penny, 1, 12)]
    [InlineData(Denomination.Farthing, 1, 4)]

    [InlineData(Denomination.Crown, 2, 8)]
    [InlineData(Denomination.Shilling, 2, 10)]
    [InlineData(Denomination.Penny, 2, 24)]
    [InlineData(Denomination.Farthing, 2, 8)]
    public void ConvertUp_WillReturnCorrectValue(Denomination denomination, int amount, int expectedValue)
    {
        var converter = new BasicCurrencyConverter();
        var result = converter.ConvertUp(denomination, amount);

        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData(Denomination.Pound, 1, 4)]
    [InlineData(Denomination.Crown, 1, 5)]
    [InlineData(Denomination.Shilling, 1, 12)]
    [InlineData(Denomination.Penny, 1, 4)]

    [InlineData(Denomination.Pound, 2, 8)]
    [InlineData(Denomination.Crown, 2, 10)]
    [InlineData(Denomination.Shilling, 2, 24)]
    [InlineData(Denomination.Penny, 2, 8)]
    public void ConvertDown_WillReturnCorrectValue(Denomination denomination, int amount, int expectedValue)
    {
        var converter = new BasicCurrencyConverter();
        var result = converter.ConvertDown(denomination, amount);

        Assert.Equal(expectedValue, result);
    }
}
