namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using LeetCode.Csharp.Common;
    using Newtonsoft.Json;

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

        // 32 - https://leetcode.com/problems/longest-valid-parentheses/
        public int LongestValidParentheses(string s)
        {
            // REVIEW: 状态转移 -> Another solution in Arrays
            // DP: Find previous matching parenthese and migrate max values.
            int[] dp = new int[s.Length];
            for (int i = 1; i < s.Length; i++)
            {
                if (s[i] == ')')
                {
                    // EX: (()) -> dp[i-1] = 2 -> from 3 to 0
                    if (dp[i - 1] > 0 && (i - dp[i - 1] - 1 >= 0) && s[i - dp[i - 1] - 1] == '(')
                    {
                        // Connecting previous valid substring, and try connect the prev-prev one with newly included (.
                        dp[i] = dp[i - 1] + ((i - dp[i - 1] - 2) >= 0 ? dp[i - dp[i - 1] - 2] + 2 : 2);
                    }
                    else if (s[i - 1] == '(')
                    {
                        // Connecting the prev one valid substring (w/ newly included ().
                        dp[i] = i - 2 >= 0 ? dp[i - 2] + 2 : 2;
                    }
                }
            }

            return dp.Any() ? dp.Max() : 0;
        }

        // 689 - https://leetcode.com/problems/maximum-sum-of-3-non-overlapping-subarrays/
        public int[] MaxSumOfThreeSubarrays(int[] nums, int k)
        {
            int[][] dp = new int[3][]; // Max sum by having i sub-arrays 'till current index.
            int[][] maxIndex = new int[3][]; // (Ending) index of the ith array that makes the max value.
            int[] sums = new int[nums.Length]; // Sum of k numbers 'till current index.

            for (int i = 0; i < 3; i++)
            {
                dp[i] = new int[nums.Length];
                maxIndex[i] = new int[nums.Length];
            }

            int currSum = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                currSum += nums[i];
                if (i >= k - 1) // k = 2, [0,1].
                {
                    sums[i] = currSum;
                    currSum -= nums[i - (k - 1)];
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = (i + 1) * k - 1; j < nums.Length; j++)
                {
                    int prevIthMax = i == 0 ? 0 : dp[i - 1][j - k];

                    if (j == 0 || prevIthMax + sums[j] > dp[i][j - 1])
                    {
                        dp[i][j] = prevIthMax + sums[j];
                        maxIndex[i][j] = j;
                    }
                    else
                    {
                        // Moving.
                        dp[i][j] = dp[i][j - 1];
                        maxIndex[i][j] = maxIndex[i][j - 1];
                    }
                }
            }

            int thirdIndex = 0;
            int thirdMax = Int32.MinValue;
            for (int i = 0; i < nums.Length; i++)
            {
                if (dp[2][i] > thirdMax)
                {
                    thirdMax = dp[2][i];
                    thirdIndex = i;
                }
            }

            int secondIndex = maxIndex[1][thirdIndex - k];
            int firstIndex = maxIndex[0][secondIndex - k];

            return new[]
            {
                firstIndex - k + 1,
                secondIndex - k + 1,
                thirdIndex - k + 1
            };
        }


        // 1143 - https://leetcode.com/problems/longest-common-subsequence/
        public int LongestCommonSubsequence(string text1, string text2)
        {
            // REVIEW: 想清楚状态转移
            int[][] dp = new int[text1.Length][];
            for (int i = 0; i < text1.Length; i++) dp[i] = new int[text2.Length];

            for (int i = 0; i < text1.Length; i++)
            {
                for (int j = 0; j < text2.Length; j++)
                {
                    int ll = i == 0 ? 0 : dp[i - 1][j];
                    int lr = i == 0 || j == 0 ? 0 : dp[i - 1][j - 1];
                    int rr = j == 0 ? 0 : dp[i][j - 1];

                    if (text1[i] == text2[j])
                    {
                        // Could only from x-1, y-1; x-1,y or x,y-1 cannot stand for previous status.
                        dp[i][j] = lr + 1;
                    }
                    else
                    {
                        dp[i][j] = Math.Max(ll, rr);
                    }
                }
            }

            return dp[text1.Length - 1][text2.Length - 1];
        }

        public int LongestCommonSubstring(String A, String B)
        {
            int[][] dp = new int[A.Length][];
            for (int i = 0; i < A.Length; i++) dp[i] = new int[B.Length];

            int maxLen = 0;
            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < B.Length; j++)
                {
                    if (A[i] == B[j])
                    {
                        dp[i][j] = i == 0 || j == 0 ? 0 : (dp[i - 1][j - 1] + 1);
                        maxLen = Math.Max(maxLen, dp[i][j]);
                    }
                }
            }

            return maxLen;
        }

        // 486 - https://leetcode.com/problems/predict-the-winner/
        public bool PredictTheWinnerDfs(int[] nums)
        {
            return this.PickNumbers(nums, 0, nums.Length - 1) >= 0;
        }

        private int PickNumbers(int[] nums, int l, int r)
        {
            // REVIEW: nums[i] - this.PickNums(...) => This is the key to convert this into min-max DP.
            // TURN is not needed since we can do:
            // pick[0] - (pick[1] - (pick[2] ...) ) -> pick[0] - pick[1] + pick[2]...
            if (l == r) return nums[l];
            int pl = nums[l] - this.PickNumbers(nums, l + 1, r);
            int pr = nums[r] - this.PickNumbers(nums, l, r - 1);
            return Math.Max(pl, pr);
        }

        // -> DP 
        public bool PredictTheWinner(int[] nums)
        {
            int[][] dp = new int[nums.Length][];
            for (int i = 0; i < nums.Length; i++) dp[i] = new int[nums.Length];

            for (int l = nums.Length - 1; l >= 0; l--)
            {
                for (int r = l + 1; r < nums.Length; r++)
                {
                    dp[l][r] = Math.Max(nums[l] - dp[l - 1][r], nums[r] - dp[l][r - 1]);
                }
            }

            return dp[0][nums.Length - 1] >= 0;
        }

        // 464 - https://leetcode.com/problems/can-i-win/
        public bool CanIWin(int maxChoosableInteger, int desiredTotal)
        {
            // Memorize: can it win with current left numbers & left total.
            // Every min-max DPs has the fact of double reverses ( -(-1) = 1, !(false) = true )

            if (maxChoosableInteger >= desiredTotal) return true;
            if ((maxChoosableInteger + 1.0) / 2.0 * maxChoosableInteger < desiredTotal) return false;

            char[] status = new char[maxChoosableInteger];
            for (int i = 0; i < maxChoosableInteger; i++) status[i] = 'X';
            return this.CanIWinInner(status, maxChoosableInteger, desiredTotal, new Dictionary<string, bool>());
        }

        private bool CanIWinInner(char[] status, int maxInt, int desiredTotal, Dictionary<string, bool> memo)
        {
            string statusKey = new string(status);
            if (memo.ContainsKey(statusKey)) return memo[statusKey];

            // Try all the numbers.
            for (int i = 1; i <= maxInt; i++)
            {
                bool found = false;
                if (status[i - 1] != 'S')
                {
                    status[i - 1] = 'S';

                    if (i >= desiredTotal || !this.CanIWinInner(status, maxInt, desiredTotal - i, memo))
                    {
                        found = true;
                    }

                    status[i - 1] = 'X';

                    if (found)
                    {
                        // Found a way to win in this given status.
                        memo[statusKey] = true;
                        return true;
                    }
                }
            }

            // Have tried all possibilities and cannot win.
            memo[statusKey] = false;
            return false;
        }

        // >>> Few related problems to LCS

        // 516 - https://leetcode.com/problems/longest-palindromic-subsequence/
        public int LongestPalindromeSubseq(string s)
        {
            // Only palindrome sub-sequence could use this way (reversing, not sub-strings).
            return this.LongestCommonSubsequence(s, new string(s.Reverse().ToArray()));
        }

        // 583 - https://leetcode.com/problems/delete-operation-for-two-strings/
        public int MinDistance1(string word1, string word2)
        {
            int lcsLen = this.LongestCommonSubsequence(word1, word2);
            return word1.Length + word2.Length - 2 * lcsLen;
        }

        // <<< End

        // 375 - https://leetcode.com/problems/guess-number-higher-or-lower-ii/
        public int GetMoneyAmount(int n)
        {
            // 简化问题为, get min(max(calc(l,x-1), calc(x+1,r)) + x) for each of x l~r recursively.
            // Optimize by memorize.
            Dictionary<string, int> memo = new Dictionary<string, int>();
            return GetMoneyAmountInner(1, n, memo);
        }

        private int GetMoneyAmountInner(int l, int r, Dictionary<string, int> memo)
        {
            if (l == r)
            {
                return 0;
            }

            if (r - l <= 1)
            {
                return l; // When length == 2, price (min) is the left val.
            }

            string key = $"{l}-{r}";
            if (memo.ContainsKey(key)) return memo[key];

            int min = Int32.MaxValue;
            for (int i = l + 1; i < r; i++)
            {
                int subAmounts = Math.Max(GetMoneyAmountInner(l, i - 1, memo), GetMoneyAmountInner(i + 1, r, memo)) + i;
                min = Math.Min(subAmounts, min);
            }

            // Console.WriteLine($"L{l},R{r},MIN:{min}");
            memo[key] = min;
            return min;
        }

        // 1049 - https://leetcode.com/problems/last-stone-weight-ii/
        public int LastStoneWeightII(int[] stones)
        {
            // REVIEW: Idea: find the closest possible sum to (sum_all / 2).
            int sum = 0;
            foreach (int stone in stones) sum += stone;

            if (sum == 0) return 0;
            int target = (sum + 1) / 2;
            bool[] dp = new bool[target + 1];
            dp[0] = true;

            int currMax = 0;
            foreach (int stone in stones)
            {
                int iterTarget = Math.Min(target, Math.Max(stone, currMax + stone));
                for (int i = iterTarget; i >= stone; i--)
                {
                    if (dp[i - stone])
                    {
                        dp[i] = true;
                        if (i > currMax) currMax = i;
                    }
                }

                // Console.WriteLine($"IT:{iterTarget}, CM:{currMax}, S:{stone}");
            }

            return Math.Abs(sum - currMax * 2);
        }

        public int LastStoneWeightII_2(int[] stones)
        {
            // REVIEW: A 0-1 knapsack version. Actually worse than my own but it's general.
            int sum = 0;
            foreach (int stone in stones) sum += stone;

            if (sum == 0) return 0;
            int target = sum / 2;
            int[] dp = new int[target + 1];

            foreach (int stone in stones)
            {
                for (int i = target; i >= stone; i--)
                {
                    // Gained value = stone
                    int gained = stone;
                    int cost = stone;
                    dp[i] = Math.Max(dp[i], gained + dp[i - cost]);
                }
            }

            return Math.Abs(sum - dp[target] - dp[target]);
        }

        // 1140 - https://leetcode.com/problems/stone-game-ii/
        public int StoneGameII(int[] piles)
        {
            var memo = new Dictionary<string, int>();
            int sum = piles.Sum();
            int delta = this.StoneGame2Inner(piles, 0, 1, memo);

            return (sum + delta) / 2;
        }

        private int StoneGame2Inner(int[] piles, int idx, int m, Dictionary<string, int> memo)
        {
            if (idx == piles.Length - 1) return piles[idx];

            string key = $"{m}-{idx}";
            if (memo.ContainsKey(key)) return memo[key];

            int maxRes = Int32.MinValue;
            int currSum = 0;
            for (int x = 1; x <= 2 * m; x++)
            {
                if (idx + x - 1 > piles.Length - 1) break;
                currSum += piles[idx + x - 1];

                int res = currSum - ((idx + x - 1 < piles.Length - 1)
                              ? this.StoneGame2Inner(piles, idx + x, Math.Max(m, x), memo)
                              : 0);
                if (res > maxRes)
                {
                    maxRes = res;
                }
            }

            memo[key] = maxRes;
            return maxRes;
        }

        // 64 - https://leetcode.com/problems/minimum-path-sum/
        public int MinPathSum(int[][] grid)
        {
            int[][] costs = new int[grid.Length][];
            for (int i = 0; i < grid.Length; i++)
            {
                costs[i] = new int[grid[i].Length];
                for (int j = 0; j < grid[i].Length; j++)
                {
                    costs[i][j] = Int32.MaxValue;
                }
            }

            costs[0][0] = grid[0][0];

            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (i == 0 && j == 0) continue;
                    int upperCost = i > 0 ? costs[i - 1][j] : Int32.MaxValue;
                    int leftCost = j > 0 ? costs[i][j - 1] : Int32.MaxValue;
                    costs[i][j] = Math.Min(upperCost, leftCost) + grid[i][j];
                }
            }

            return costs[grid.Length - 1][grid[grid.Length - 1].Length - 1];
        }

        // 1340 - https://leetcode.com/problems/jump-game-v/
        public int MaxJumps(int[] arr, int d)
        {
            int max = 0;
            int[] memo = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                max = Math.Max(this.FindMaxJumps(arr, d, i, memo), max);
            }

            return max;
        }

        private int FindMaxJumps(int[] arr, int d, int idx, int[] memo)
        {
            if (memo[idx] > 0) return memo[idx];

            int max = 1;
            for (int i = idx + 1; i <= Math.Min(arr.Length - 1, idx + d); i++)
            {
                if (arr[i] >= arr[idx]) break;
                else
                {
                    max = Math.Max(max, this.FindMaxJumps(arr, d, i, memo) + 1);
                }
            }

            for (int i = idx - 1; i >= Math.Max(0, idx - d); i--)
            {
                if (arr[i] >= arr[idx]) break;
                else
                {
                    max = Math.Max(max, this.FindMaxJumps(arr, d, i, memo) + 1);
                }
            }

            memo[idx] = max;
            return max;
        }

        // 221 - https://leetcode.com/problems/maximal-square/
        public int MaximalSquare(char[][] matrix)
        {
            int max = 0;
            int[][] dp = new int[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
            {
                dp[i] = new int[matrix[i].Length];
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    dp[i][j] = matrix[i][j] == '0'
                        ? 0
                        : (
                            i == 0 || j == 0
                                ? 1
                                : Math.Min(dp[i - 1][j], Math.Min(dp[i - 1][j - 1], dp[i][j - 1])) + 1
                        );

                    max = Math.Max(max, dp[i][j]);
                }
            }

            return max * max;
        }

        // 84 - https://leetcode.com/problems/largest-rectangle-in-histogram/
        public int LargestRectangleArea(int[] heights)
        {
            // REVIEW: 从每个下降区前开始往前回溯；因为之前的面积都包含在更长的柱子的面积里了
            int max = 0;
            for (int i = 1; i <= heights.Length; i++)
            {
                if (i == heights.Length || heights[i] < heights[i - 1])
                {
                    int currHeight = heights[i - 1];
                    for (int j = i - 1; j >= 0; j--)
                    {
                        currHeight = Math.Min(currHeight, heights[j]);
                        int currArea = currHeight * (i - j);

                        // Console.WriteLine($"F:{j},T:{i-1},A:{currArea}");

                        max = Math.Max(currArea, max);
                    }
                }
            }

            return max;
        }

        // 85 - https://leetcode.com/problems/maximal-rectangle/
        public int MaximalRectangle(char[][] matrix)
        {
            // DP: 最长的竖线 ("1")
            int[][] dp = new int[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
            {
                dp[i] = new int[matrix[i].Length];
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    dp[i][j] = matrix[i][j] == '0'
                        ? 0
                        : (
                            i == 0
                                ? 1
                                : dp[i - 1][j] + 1
                        );

                    // Console.Write($"{dp[i][j]} ");
                }

                // Console.WriteLine("\n");
            }

            int maxRec = 0;
            for (int i = 0; i < dp.Length; i++)
            {
                for (int j = 1; j <= dp[i].Length; j++)
                {
                    if (j == dp[i].Length || dp[i][j] < dp[i][j - 1])
                    {
                        int currHeight = dp[i][j - 1];
                        for (int k = j - 1; k >= 0; k--)
                        {
                            currHeight = Math.Min(currHeight, dp[i][k]);
                            int currArea = currHeight * (j - k);

                            // Console.WriteLine($"I: {i}, F:{k},T:{j-1},A:{currArea}, CH:{currHeight}, CL:{j - k}");

                            maxRec = Math.Max(currArea, maxRec);
                        }
                    }
                }
            }

            return maxRec;
        }

        // 312 - https://leetcode.com/problems/burst-balloons/
        public int MaxCoins(int[] nums)
        {
            // REVIEW: Brilliant solution!
            // 1. Remove 0 nums 'cuz they won't contribute to the result!
            int[] filteredNumbers = new[] {1}.Concat(nums.Where(n => n != 0)).Concat(new[] {1}).ToArray();
            Dictionary<string, int> cachedSums = new Dictionary<string, int>();
            return this.SubMaxCoins(filteredNumbers, 0, filteredNumbers.Length - 1, cachedSums);
        }

        public int SubMaxCoins(int[] nums, int l, int r, Dictionary<string, int> cachedSums)
        {
            if (r - l < 2) return 0; // 如果子串已经少于三个元素，计算结果已经在上一层得出了。
            if (cachedSums.TryGetValue($"{l}_{r}", out int val)) return val;

            int maxSum = 0;
            for (int mid = l + 1; mid < r; mid++)
            {
                maxSum = Math.Max(maxSum, nums[l] * nums[r] * nums[mid]
                                          + this.SubMaxCoins(nums, l, mid, cachedSums)
                                          + this.SubMaxCoins(nums, mid, r, cachedSums));
            }

            cachedSums.Add($"{l}_{r}", maxSum);
            return maxSum;
        }
        
        // 338 - https://leetcode.com/problems/counting-bits/
        public int[] CountBits(int num)
        {
            int[] result = new int[num + 1];
            result[0] = 0;
            result[1] = 1;

            for (int i = 2; i <= num; i++)
            {
                if (i % 2 == 1) result[i] = result[i - 1] + 1;
                else result[i] = result[i / 2]; // i % 2 == 0
            }

            return result;
        }
        
        // https://leetcode.com/problems/count-square-submatrices-with-all-ones/
        public int CountSquares(int[][] matrix)
        {
            int[][] dp = new int[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++) dp[i] = new int[matrix[i].Length];

            int cnt = 0;
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (matrix[i][j] == 0)
                    {
                        dp[i][j] = 0;
                    }
                    else if (i > 0 && j > 0)
                    {
                        dp[i][j] = 1 + Math.Min(dp[i - 1][j], Math.Min(dp[i][j - 1], dp[i - 1][j - 1]));
                        cnt += dp[i][j];
                    }
                    else
                    {
                        dp[i][j] = 1;
                        cnt++;
                    }
                }
            }

            return cnt;
        }

        public int[] TopKFrequent(int[] nums, int k)
        {
            var grps = nums.GroupBy(n => n).ToList();
            MaxHeap<MinHeapNode> minHeap = new MaxHeap<MinHeapNode>(k + 1);
            foreach (IGrouping<int, int> grp in grps)
            {
                Console.WriteLine(string.Join(", ", minHeap.Nodes.Select(n => $"{n.Num}-{n.Cnt}")));
                
                if (minHeap.Count < k)
                {
                    minHeap.Insert(new MinHeapNode(grp.Key, grp.Count()));
                }
                else
                {
                    int cnt = grp.Count();
                    if (cnt > minHeap.GetMax().Cnt)
                    {
                        minHeap.RemoveMax();
                        minHeap.Insert(new MinHeapNode(grp.Key, cnt));
                    }
                }
                
                Console.WriteLine(string.Join(", ", minHeap.Nodes.Select(n => $"{n.Num}-{n.Cnt}")));
            }

            return minHeap.Nodes.Select(n => n.Num).ToArray();
        }

        class MinHeapNode : IHeapNode
        {
            public int Num { get; }
            public int Cnt { get; }

            public MinHeapNode(int num,int cnt)
            {
                this.Num = num;
                this.Cnt = cnt;
            }
            
            public int GetValue()
            {
                return -this.Cnt;
            }
        }
        
        // https://leetcode.com/problems/uncrossed-lines/
        public int MaxUncrossedLines(int[] A, int[] B)
        {
            // REVIEW: 这TM就是求最长公共子序列 (Longest common subseq), 因为公共子序列就不会有题中说到的几种相交的case...
            // REVIEW: 想清楚状态转移
            int[][] dp = new int[A.Length][];
            for (int i = 0; i < A.Length; i++) dp[i] = new int[B.Length];

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < B.Length; j++)
                {
                    int ll = i == 0 ? 0 : dp[i - 1][j];
                    int lr = i == 0 || j == 0 ? 0 : dp[i - 1][j - 1];
                    int rr = j == 0 ? 0 : dp[i][j - 1];

                    if (A[i] == B[j])
                    {
                        dp[i][j] = lr + 1;
                    }
                    else
                    {
                        dp[i][j] = Math.Max(ll, Math.Max(lr, rr));
                    }
                }
            }

            return dp[A.Length - 1][B.Length - 1];
        }
        
        // https://leetcode.com/problems/house-robber-ii/
        public int Rob(int[] nums)
        {
            List<int> nums1 = new List<int>(nums);
            List<int> nums2 = new List<int>(nums);
            nums1[0] = 0;
            nums2[nums2.Count - 1] = 0;
            return Math.Max(RobInner(nums1), RobInner(nums2));
        }

        // https://leetcode.com/problems/house-robber-iii/
        public int Rob(TreeNode root)
        {
            return RobTreeInner(root, "Z", new Dictionary<string, int>());
        }

        private int RobTreeInner(TreeNode root, string locator, Dictionary<string, int> memo)
        {
            if (root == null) return 0;
            if (memo.TryGetValue(locator, out int res)) return res;
            
            // Skip me and skip my next level.
            int skipMe = RobTreeInner(root.left, locator + "L", memo) + RobTreeInner(root.right, locator + "R", memo);
            int countMe = root.val;
            if (root.left != null)
            {
                countMe += (RobTreeInner(root.left.left, locator + "LL", memo) + RobTreeInner(root.left.right, locator + "LR", memo));
            }
            
            if (root.right != null)
            {
                countMe += (RobTreeInner(root.right.left, locator + "RL", memo) + RobTreeInner(root.right.right, locator + "RR", memo));
            }

            res = Math.Max(skipMe, countMe);
            memo[locator] = res;
            return res;
        }

        public int RobInner(List<int> nums) {
            int[] dp = new int[nums.Count];
        
            int max = 0;
            for(int i=0;i<nums.Count;i++) {
                int p = nums[i];
                if (i == 2) {
                    p += dp[i-2];
                } else if (i > 2) {
                    p += Math.Max(dp[i-2], dp[i-3]);
                }
            
                dp[i] = p;
                max = Math.Max(max, p);
            }
        
            return max;
        }
        
        // https://leetcode.com/problems/edit-distance/
        public int MinDistance(string word1, string word2)
        {
            return MinDistanceInner(word1, word2, new Dictionary<int, Dictionary<int, int>>());
        }

        private int MinDistanceInner(string word1, string word2, Dictionary<int, Dictionary<int, int>> memo)
        {
            // REVIEW: IDEA: 从 word1 构造 word2
            if (word1.Length == 0) return word2.Length;
            if (word2.Length == 0) return word1.Length;
            if (memo.TryGetValue(word1.Length, out Dictionary<int, int> minDist) && minDist.TryGetValue(word2.Length, out int dist))
            {
                return dist;
            }

            if (word1[0] == word2[0])
            {
                dist = MinDistanceInner(word1.Substring(1), word2.Substring(1), memo);
            }
            else
            {
                // 认为当前char需要从W1 remove，所以忽略本char
                int removeFromW1 = 1 + MinDistanceInner(word1.Substring(1), word2, memo);
                
                // 认为是直接替换（修改）W1
                int changeFromWn = 1 + MinDistanceInner(word1.Substring(1), word2.Substring(1), memo);
                
                // 认为是直接添加一个 （W2中的）char, 所以忽略W2的char
                int insertIntoW1 = 1 + MinDistanceInner(word1, word2.Substring(1), memo);

                dist = Math.Min(removeFromW1, Math.Min(changeFromWn, insertIntoW1));
            }

            if (!memo.ContainsKey(word1.Length))
            {
                memo.Add(word1.Length, new Dictionary<int, int>());
            }

            memo[word1.Length][word2.Length] = dist;
            return dist;
        }
        
        // https://leetcode.com/problems/wildcard-matching/
        public bool IsMatch(string s, string p)
        {
            // DP than DFA.
            while (p.Contains("**"))
            {
                p = p.Replace("**", "*");
            }

            if (p.Length - p.Count(c => c == '*') > s.Length) return false;

            int[][] memo = new int[s.Length + 1][];
            for (int i = 0; i <= s.Length; i++) memo[i] = new int[p.Length + 1];

            return this.IsMatch(s, 0, p, 0, memo);
        }

        public bool IsMatch(string s, int si, string p, int pi, int[][] memo)
        {
            if (si == s.Length && pi == p.Length)
            {
                return true;
            }
            else if (pi == p.Length)
            {
                return false;
            }
            
            if (memo[si][pi] != 0) return memo[si][pi] > 0;

            bool ret = false;
            if (p[pi] == '?')
            {
                ret = s.Length > si && this.IsMatch(s, si + 1, p, pi + 1, memo);
            }
            else if (p[pi] == '*')
            {
                for (int i = si; i <= s.Length; i++)
                {
                    ret = this.IsMatch(s, i, p, pi + 1, memo);
                    if (ret) break;
                }
            }
            else
            {
                if (si < s.Length && s[si] == p[pi])
                {
                    ret = this.IsMatch(s, si + 1, p, pi + 1, memo);
                }
            }

            memo[si][pi] = ret ? 1 : -1;
            return ret;
        }
        
        // https://leetcode.com/problems/longest-chunked-palindrome-decomposition/
        public int LongestDecomposition(string text) {
            // I cannot prove this problem could be solved by greedy algo, so still use memo-search but technically the
            // time complexity is still O(n).
            int[] memo = new int[text.Length / 2 + 1];
            return this.LongestDecompositionSearch(text, 0, memo);
        }

        private int LongestDecompositionSearch(string text, int idx, int[] memo)
        {
            if (memo[idx] > 0)
            {
                return memo[idx];
            }

            if (idx == text.Length / 2)
            {
                return text.Length % 2 == 1 ? 1 : 0;
            }

            memo[idx] = 1;

            for (int len = 1; len <= (text.Length - 2 * idx) / 2; len++)
            {
                int li = idx;
                int ri = (text.Length - idx) - len;
                if (this.QStringEqual(text, li, ri, len))
                {
                    memo[idx] = Math.Max(memo[idx], 2 + this.LongestDecompositionSearch(text, idx + len, memo));
                }
            }

            return memo[idx];
        }

        private bool QStringEqual(string text, int li, int ri, int len)
        {
            for (int l = 0; l < len; l++)
            {
                if (text[li + l] != text[ri + l]) return false;
            }

            return true;
        }
        
        // https://leetcode.com/problems/partition-equal-subset-sum/
        public bool CanPartition(int[] nums)
        {
            int total = nums.Sum();
            if (total % 2 == 1) return false;

            int half = total / 2;
            bool[] dp = new bool[half + 1];
            dp[0] = true;

            int lastKnownTrue = 0;
            foreach (int num in nums)
            {
                for (int i = Math.Min(half, num + lastKnownTrue); i >= num; i--)
                {
                    if (dp[i - num])
                    {
                        dp[i] = true;
                        lastKnownTrue = Math.Max(lastKnownTrue, i);
                    }
                }
            }

            return dp[half];
        }
        
        // https://leetcode.com/problems/largest-divisible-subset/
        public IList<int> LargestDivisibleSubset(int[] nums)
        {
            // REVIEW: DP 思想， FW 的维护
            nums = nums.OrderBy(n => n).ToArray();
            int[] dp = new int[nums.Length];
            int[] fw = new int[nums.Length];

            int maxIdx = -1;
            for (int i = 0; i < nums.Length; i++)
            {
                fw[i] = -1;
                for (int j = 0; j < i; j++)
                {
                    if (nums[i] % nums[j] == 0 && dp[j] + 1 > dp[i])
                    {
                        dp[i] = dp[j] + 1;
                        fw[i] = j;
                    }
                }

                if (maxIdx == -1 || dp[i] > dp[maxIdx])
                {
                    maxIdx = i;
                }
            }

            List<int> ret = new List<int>();
            while (maxIdx >= 0)
            {
                ret.Add(nums[maxIdx]);
                maxIdx = fw[maxIdx];
            }

            return ret;
        }
        
        // https://leetcode.com/problems/super-washing-machines/
        public int FindMinMoves(int[] machines)
        {
            // REVIEW: 寻找积累的需要最大补充数的machine.
            int total = machines.Sum();
            if (total % machines.Length != 0) return -1;

            int target = total / machines.Length;
            int prev = 0;
            int maxNeed = 0;
            foreach (int machine in machines)
            {
                int need = target - machine + prev;
                
                // 两个限制（步数）的因素：1、单个machine需要移走，每次只能-1，2、单个machine需要通过(via)的数目
                maxNeed = Math.Max(maxNeed, Math.Max(Math.Abs(need), machine - target));
                prev = need;
            }

            return maxNeed;
        }
        
        // https://leetcode.com/problems/dungeon-game/
        public int CalculateMinimumHP(int[][] dungeon)
        {
            // dp means the required minimal health to enter each of the positions.
            int[][] dp = new int[dungeon.Length][];
            for (int i = 0; i < dp.Length; i++)
            {
                dp[i] = new int[dungeon[i].Length];
            }

            for (int i = dp.Length - 1; i >= 0; i--)
            {
                for (int j = dp[i].Length - 1; j >= 0; j--)
                {
                    if (i == dp.Length - 1 && j == dp[i].Length - 1)
                    {
                        dp[i][j] = Math.Max(1, 1 - dungeon[i][j]);
                    }
                    else if (i == dp.Length - 1)
                    {
                        dp[i][j] = Math.Max(1, dp[i][j + 1] - dungeon[i][j]);
                    }
                    else if (j == dp[i].Length - 1)
                    {
                        dp[i][j] = Math.Max(1, dp[i + 1][j] - dungeon[i][j]);
                    }
                    else
                    {
                        dp[i][j] = Math.Max(1, Math.Min(dp[i + 1][j], dp[i][j + 1]) - dungeon[i][j]);
                    }
                }
            }

            return dp[0][0];
        }
        
        
        // https://leetcode.com/problems/cherry-pickup/
        public int CherryPickup(int[][] grid)
        {
            // TODO (too hard)
            return -1;
        }

        // https://leetcode.com/problems/unique-binary-search-trees-ii/
        public IList<TreeNode> GenerateTrees(int n)
        {
            if (n == 0) return new List<TreeNode>();
            return this.GenerateTreesRec(1, n);
        }

        public List<TreeNode> GenerateTreesRec(int start, int len)
        {
            if (len == 0) return new List<TreeNode> { null };
            if (len == 1) return new List<TreeNode> { new TreeNode(start) };
            else
            {
                List<TreeNode> ret = new List<TreeNode>();
                for (int llen = 0; llen < len; llen++)
                {
                    List<TreeNode> lset = GenerateTreesRec(start, llen);
                    List<TreeNode> rset = GenerateTreesRec(start + llen + 1, len - llen - 1);
                    for (int i = 0; i < lset.Count; i++)
                    {
                        for (int j = 0; j < rset.Count; j++)
                        {
                            var node = new TreeNode(start + llen);
                            node.left = lset[i];
                            node.right = rset[j];
                            ret.Add(node);
                        }
                    }
                }

                return ret;
            }
        }

        public void Run()
        {
        }
    }
}