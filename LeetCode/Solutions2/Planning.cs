
namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Planning
    {
        // 122 - https://leetcode.com/problems/best-time-to-buy-and-sell-stock-ii/
        public int MaxProfit(int[] prices)
        {
            int maxProfit = 0;
            for (int i = 1; i < prices.Length; i++)
            {
                if (prices[i] - prices[i - 1] > 0) maxProfit += (prices[i] - prices[i - 1]);
            }

            return maxProfit;
        }

        // 123 - https://leetcode.com/problems/best-time-to-buy-and-sell-stock-iii/
        public int MaxProfit3(int[] prices)
        {
            int curr_min = Int32.MaxValue;
            int once_max = 0;
            int after_once_max = Int32.MinValue;
            int twice_max = 0;
            foreach (int price in prices)
            {
                // min (start_point) => a start point of having one txn.
                if (price < curr_min) curr_min = price;

                // Get the max profit by doing once txn so far.
                if (price - curr_min > once_max) once_max = price - curr_min;

                // max (once_max[j] - price[j]) => a start point of having two txns.
                if (once_max - price > after_once_max) after_once_max = once_max - price;

                // Get the max profit by doing two txns so far.
                if (after_once_max + price > twice_max) twice_max = after_once_max + price;

                Console.WriteLine($"CM:{curr_min},OM:{once_max},AOM:{after_once_max},TM:{twice_max}");
            }

            return Math.Max(once_max, twice_max);
        }

        public void Run()
        {
            Console.WriteLine(this.MaxProfit3(new int[] { 3, 3, 5, 0, 0, 3, 1, 4 })); // 6
            Console.WriteLine(this.MaxProfit3(new int[] { 1, 2, 3, 4, 5 })); // 4
            Console.WriteLine(this.MaxProfit3(new int[] { 7, 6, 4, 3, 1 })); // 7
        }
    }
}