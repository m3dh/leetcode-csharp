
namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
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
            
            char[]status = new char[maxChoosableInteger];
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
        public int MinDistance(string word1, string word2)
        {
            int lcsLen = this.LongestCommonSubsequence(word1, word2);
            return word1.Length + word2.Length - 2 * lcsLen;
        }

        // <<< End

        public void Run()
        {
            Console.WriteLine(this.LongestCommonSubsequence("abacfgcaba", "abacgfcaba")); // 5
        }
    }
}