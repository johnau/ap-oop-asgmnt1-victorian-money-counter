using System.Linq;

namespace VictorianMoneyCounterUWP.Utilities
{
    public static class StringHelpers
    {
        public static string CapitalizeFirstLetter(string input)
        {
            if (input == string.Empty) return input;
            //return input?.First().ToString().ToUpper() + input?[1..].ToLower();
            var ucaseFirst = input?.First().ToString().ToUpper();
            var rest = input?.Substring(1).ToLower();
            return ucaseFirst + rest;

        }
    }
}

