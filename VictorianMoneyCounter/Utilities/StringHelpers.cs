using System.Diagnostics;

namespace VictorianMoneyCounter.Utilities;

public static class StringHelpers
{
    public static string CapitalizeFirstLetter(string input)
    {
        if (input == string.Empty) return input;
        return input?.First().ToString().ToUpper() + input?[1..].ToLower();
    }

}
