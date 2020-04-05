
namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Planning
    {
        // 123 - https://leetcode.com/problems/best-time-to-buy-and-sell-stock-iii/
        public int MaxProfit3(int[] prices)
        {
            int after_zero_max = Int32.MinValue;
            int once_max = 0;
            int after_once_max = Int32.MinValue;
            int twice_max = 0;
            foreach (int price in prices)
            {
                // min (start_point) => a start point of having one txn.
                if (-price > after_zero_max) after_zero_max = -price;

                // Get the max profit by doing once txn so far.
                if (price + after_zero_max > once_max) once_max = price + after_zero_max;

                // max (once_max[j] - price[j]) => a start point of having two txns.
                if (once_max - price > after_once_max) after_once_max = once_max - price;

                // Get the max profit by doing two txns so far.
                if (after_once_max + price > twice_max) twice_max = after_once_max + price;

                // Console.WriteLine($"CM:{curr_min},OM:{once_max},AOM:{after_once_max},TM:{twice_max}");
            }

            return Math.Max(once_max, twice_max);
        }

        // 122 - https://leetcode.com/problems/best-time-to-buy-and-sell-stock-ii/
        public int MaxProfit(int[] prices)
        {
            // REVIEW: 局部性思维
            int maxProfit = 0;
            for (int i = 1; i < prices.Length; i++)
            {
                if (prices[i] - prices[i - 1] > 0) maxProfit += (prices[i] - prices[i - 1]);
            }

            return maxProfit;
        }

        // 188 - https://leetcode.com/problems/best-time-to-buy-and-sell-stock-iv/
        public int MaxProfit(int k, int[] prices)
        {
            // In which case you're actually allowed to do infinite transactions.
            if (k > (prices.Length + 1) / 2) return MaxProfit(prices);

            // DP of doing k txns.
            int[] maxDp = new int[k + 1];

            // DP of before doing k txns.
            int[] befDp = new int[k + 1];
            for (int i = 0; i <= k; i++)
            {
                befDp[i] = int.MinValue;
            }

            int ret = 0;
            foreach (int price in prices)
            {
                for (int txn = 1; txn <= k; txn++)
                {
                    // Get MAX after prev txn val => to get start point for next txn.
                    if (maxDp[txn - 1] - price > befDp[txn]) befDp[txn] = maxDp[txn - 1] - price;

                    // Get the max profit by doing txns.
                    if (befDp[txn] + price > maxDp[txn]) maxDp[txn] = befDp[txn] + price;

                    // Get the result.
                    if (maxDp[txn] > ret) ret = maxDp[txn];
                }
            }

            return ret;
        }

        public void Run()
        {
            Console.WriteLine(this.MaxProfit(2, new int[] { 2, 4, 1 })); // 2
            Console.WriteLine(this.MaxProfit(2, new int[] { 3, 2, 6, 5, 0, 3 })); // 7
        }
    }
}