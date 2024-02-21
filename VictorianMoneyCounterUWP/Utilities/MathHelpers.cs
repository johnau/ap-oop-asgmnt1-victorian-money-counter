using System;

namespace VictorianMoneyCounterUWP.Utilities
{
    public class MathHelpers
    {
        public static double CalculateBounceHeight(double totalTime, double currentTime, double totalHeight)
        {
            double amplitude = totalHeight * 0.25;
            double frequency = 2 * Math.PI / totalTime;

            double bounceHeight = amplitude * Math.Sin(frequency * currentTime);
            return Math.Max(bounceHeight, 0);
        }

        public static double CalculateBounceLength(double totalTime, double currentTime, double maxWidth)
        {
            double ratio = currentTime / totalTime;
            double bounceLength = maxWidth * (1 - Math.Pow(ratio, 2));
            return Math.Max(bounceLength, 0); // Ensure non-negative value
        }
    }

}

