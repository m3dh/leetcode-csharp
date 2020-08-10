namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading;
    using LeetCode.Csharp.Common;
    using Newtonsoft.Json;

    public class Planning2
    {
        // https://leetcode.com/problems/minimum-cost-to-cut-a-stick/
        public int MinCost(int n, int[] cuts)
        {
            // REVIEW: DP based on cuts not n numbers, and use 0, n as boundary.
            int[][] dp = new int[cuts.Length + 2][];
            for (int i = 0; i <= cuts.Length + 1; i++)
            {
                dp[i] = new int[cuts.Length + 2];
                for (int j = 0; j <= cuts.Length + 1; j++) dp[i][j] = -1;
            }

            cuts = cuts.Concat(new[] { 0, n }).OrderBy(c => c).ToArray();
            return MinCostRec(0, cuts.Length - 1, cuts, dp);
        }

        private int MinCostRec(int l, int r, int[] cuts, int[][] dp)
        {
            if (r - l <= 1) return 0;

            if (dp[l][r] >= 0) return dp[l][r];
            else
            {
                int minCost = -1;
                for (int i = l+1; i < r; i++)
                {
                    int cost = MinCostRec(l, i, cuts, dp) + MinCostRec(i, r, cuts, dp) + (cuts[r] - cuts[l]);
                    minCost = minCost < 0 ? cost : Math.Min(minCost, cost);
                }

                minCost = minCost < 0 ? 0 : minCost;
                Console.WriteLine($"L:{l}, R:{r}, C:{minCost}");

                dp[l][r] = minCost;
                return minCost;
            }
        }

        // https://leetcode.com/problems/maximum-number-of-non-overlapping-subarrays-with-sum-equals-target/
        public int MaxNonOverlapping(int[] nums, int target)
        {
            // REVIEW: 用一趟扫描来完成 DP
            Dictionary<int, int> counts = new Dictionary<int, int>(); // The count of last position of given value.
            counts[0] = 0;

            int sum = 0;
            int ret = 0;
            foreach (int num in nums)
            {
                sum += num;

                // REVIEW: sum - key = target
                int key = -target + sum;
                if (counts.TryGetValue(key, out int cnt))
                {
                    ret = Math.Max(cnt + 1, ret);
                }

                counts[sum] = ret;
            }

            return ret;
        }

        public string StoneGameIII(int[] stoneValue)
        {
            var result = StoneGame3Rec(stoneValue, 0, new Dictionary<int, int>());
            return result > 0 ? "Alice" : (result == 0 ? "Tie" : "Bob");
        }

        private int StoneGame3Rec(int[] stoneValues, int idx, Dictionary<int, int> memo)
        {
            if (idx >= stoneValues.Length) return 0;
            if (!memo.TryGetValue(idx, out int val))
            {
                val = int.MinValue;
                int curScore = 0;
                for (int take = 1; take <= 3 && idx + take - 1 < stoneValues.Length; take++)
                {
                    curScore += stoneValues[idx + take - 1];
                    val = Math.Max(val, curScore - StoneGame3Rec(stoneValues, idx + take, memo));
                }

                memo[idx] = val;
            }

            return val;
        }

        public void Run()
        {
            Console.WriteLine(this.MinCost(
                24811,
                new[]
                {
                    409, 8398, 9521, 15901, 13345, 12723, 15849, 23078, 9522, 16862, 2255, 21622, 8351, 9870, 8069, 10200, 21779, 17694, 11383, 2188, 16705, 13192, 1675, 6011, 2598, 22470, 8164, 2642,
                    3391, 596, 21537, 4668, 4524, 13209, 24249
                }));
        }
    }
}
