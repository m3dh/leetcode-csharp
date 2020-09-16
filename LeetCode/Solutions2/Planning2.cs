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

        // https://leetcode.com/problems/target-sum/
        public int FindTargetSumWays(int[] nums, int S)
        {
            return FindTargetSumWays(nums, 0, 0, S, new Dictionary<string, int>());
        }

        private int FindTargetSumWays(int[] nums, int idx, int curSum, int S, Dictionary<string, int> memo)
        {
            if (idx == nums.Length)
            {
                return curSum == S ? 1 : 0;
            }
            else
            {
                string key = $"{curSum}-{idx}";
                if (!memo.TryGetValue(key, out int cnt))
                {
                    cnt = FindTargetSumWays(nums, idx + 1, curSum - nums[idx], S, memo) + FindTargetSumWays(nums, idx + 1, curSum + nums[idx], S, memo);
                    memo[key] = cnt;
                }

                return cnt;
            }
        }

        // https://leetcode.com/problems/arithmetic-slices-ii-subsequence/solution/
        public int NumberOfArithmeticSlices(int[] A)
        {
            // REVIEW: Count how many two item tuple can we find before item[i].

            // [A.Index -> <Diff, Count>]
            Dictionary<int, int>[] diffCounts = new Dictionary<int, int>[A.Length];
            int ans = 0;
            for (int i = 0; i < A.Length; i++)
            {
                diffCounts[i] = new Dictionary<int, int>();
                for (int j = 0; j < i; j++)
                {
                    long lDelta = (long) A[i] - (long) A[j];
                    if (lDelta > int.MaxValue || lDelta < int.MinValue) continue;

                    int delta = (int) lDelta;

                    // we have found 1 more tuple anyways.
                    diffCounts[i][delta] = 1 + (diffCounts[i].TryGetValue(delta, out int o2) ? o2 : 0);

                    // how many pairs before me? (So all the pairs can be into 3 or more items).
                    int prevSum = diffCounts[j].TryGetValue(delta, out int o1) ? o1 : 0;

                    if (prevSum > 0)
                    {
                        ans += prevSum;
                        diffCounts[i][delta] += prevSum;
                    }
                }
            }

            return ans;
        }

        // https://leetcode.com/problems/maximum-profit-in-job-scheduling/
        public int JobScheduling(int[] startTime, int[] endTime, int[] profit)
        {
            List<Job> jobs = new List<Job>();
            for (int i = 0; i < startTime.Length; i++)
            {
                jobs.Add(new Job
                {
                    StartTime = startTime[i],
                    EndTime = endTime[i],
                    Profit = profit[i]
                });
            }

            jobs = jobs.OrderBy(j => j.EndTime).ToList();

            // dp - 前面的总结外加选定当前的
            int[] dp = new int[jobs.Count];
            dp[0] = jobs[0].Profit; // dp[0] 只有选定一种情况(as profit > 0)

            for (int i = 1; i < dp.Length; i++)
            {
                // Max of <Prev, StartFromMe>
                dp[i] = Math.Max(dp[i - 1], jobs[i].Profit);
                for (int j = i - 1; j >= 0; j--)
                {
                    if (jobs[j].EndTime <= jobs[i].StartTime)
                    {
                        // 因为一路携带，所以比到第一个end < start就可以了
                        // Max of <Prev, StartFromMe, HavingSomeoneAhead>
                        dp[i] = Math.Max(dp[i], dp[j] + jobs[i].Profit);
                        break;
                    }
                }
            }

            return dp.Last();
        }

        private class Job
        {
            public int StartTime { get; set; }
            public int EndTime { get; set; }
            public int Profit { get; set; }
        }

        // https://leetcode.com/problems/expression-add-operators/
        public IList<string> AddOperators(string num, int target)
        {
            List<string> result = new List<string>();
            AddOperatorsRec(target, num, string.Empty, 0, 0, result, true);
            return result;
        }

        // cannot have prefixing '-'
        private void AddOperatorsRec(int target, string num, string path, long cur, long tmp, List<string> ret, bool first)
        {
            // 注意此时无论如何都结束了，只是看一下要不要计入result
            if (string.IsNullOrEmpty(num))
            {
                if ((cur + tmp == target))
                {
                    ret.Add(path);
                }

                return;
            }

            // looping through all the split positions.
            long val = 0;
            for (int len = 1; len <= num.Length; len++)
            {
                // handle case: leading zeros.
                if (len > 1 && val == 0)
                {
                    continue;
                }

                val = val * 10 + num[len - 1] - '0';

                // handle first number (?)
                if (first)
                {
                    AddOperatorsRec(
                        target,
                        num.Substring(len),
                        path + $"{val}",
                        cur + tmp,
                        val,
                        ret,
                        false);
                }
                else
                {
                    // + val
                    // eval previous operator
                    AddOperatorsRec(
                        target,
                        num.Substring(len),
                        path + $"+{val}",
                        cur + tmp,
                        val,
                        ret,
                        false);

                    // * val
                    AddOperatorsRec(
                        target,
                        num.Substring(len),
                        path + $"*{val}",
                        cur,
                        tmp * val,
                        ret,
                        false);

                    // - val
                    AddOperatorsRec(
                        target,
                        num.Substring(len),
                        path + $"-{val}",
                        cur + tmp,
                        -val,
                        ret,
                        false);
                }
            }
        }

        // https://leetcode.com/problems/encode-string-with-shortest-length/
        public string Encode(string s)
        {
            return EncodeZRec(s, new Dictionary<string, string>());
        }

        private string EncodeZRec(string s, Dictionary<string, string> memo)
        {
            if (s.Length <= 4)
            {
                // 2[a].Length = 4
                return s;
            }

            if (!memo.TryGetValue(s, out string best))
            {
                best = s;
                for (int len = 1; len < s.Length; len++)
                {
                    // abcabc ->
                    //  len = 1, repeat = 1

                    // REVIEW: 重复子串也应该递归收缩！
                    string curStr = EncodeZRec(s.Substring(0, len), memo);
                    int repeat = 1;
                    while (len * repeat <= s.Length)
                    {
                        string suffixBest = EncodeZRec(s.Substring(len * repeat), memo);
                        string curBest = repeat == 1
                            ? $"{curStr}{suffixBest}"
                            : $"{repeat}[{curStr}]{suffixBest}";

                        if (curBest.Length < best.Length)
                        {
                            best = curBest;
                        }

                        if (len * (1 + repeat) <= s.Length)
                        {
                            bool flag = true;
                            for (int i = 0; i < len; i++)
                            {
                                if (s[i] != s[len * repeat + i])
                                {
                                    flag = false;
                                    break;
                                }
                            }

                            if (flag)
                            {
                                repeat++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                memo[s] = best;
            }

            return best;
        }

        public void Run()
        {
            // [1,2,3,4,6]
            // [3,5,10,6,9]
            // [20,20,100,70,60]

            this.AddOperators("105", 5).JsonPrint();
        }
    }
}
