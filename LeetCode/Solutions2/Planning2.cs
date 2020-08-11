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

        public int MergeStones(int[] stones, int k)
        {
            // REVIEW: 重做一遍
            if (stones == null || stones.Length <= 1)
            {
                return 0;
            }

            if (stones.Length < k || (stones.Length - 1) % (k - 1) != 0)
            {
                return -1;
            }

            int[] prefixSum = new int[stones.Length + 1];
            for (int i = 0; i < stones.Length; i++)
            {
                prefixSum[i + 1] = prefixSum[i] + stones[i];
            }

            int[,] d = new int[stones.Length, stones.Length];

            for (int length = k; length <= stones.Length; length++)
            {
                for (int start = 0; start <= stones.Length - length; start++)
                {
                    int end = start + length - 1;
                    d[start, end] = int.MaxValue;

                    for (int mid = start; mid < end; mid += (k - 1))
                    {
                        d[start, end] = Math.Min(d[start, end], d[start, mid] + d[mid + 1, end]);
                    }

                    if ((end - start) % (k - 1) == 0)
                    {
                        d[start, end] += prefixSum[end + 1] - prefixSum[start];
                    }
                }
            }

            return d[0, stones.Length - 1];
        }

        public void Run()
        {
            Console.WriteLine(this.MaxNonOverlapping(new[] { -1, 3, 5, 1, 4, 2, -9 }, 6));
        }
    }
}
