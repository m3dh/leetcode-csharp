namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Arrays
    {
        // 560 - https://leetcode.com/problems/subarray-sum-equals-k/
        public int SubarraySum(int[] nums, int k)
        {
            // REVIEW: 局部性思想
            Dictionary<int, int> sums = new Dictionary<int, int> {{0, 1}};
            int sum = 0;
            int retCount = 0;
            foreach (int num in nums)
            {
                sum += num;
                if (sums.ContainsKey(sum - k)) retCount += sums[sum - k];
                sums[sum] = sums.ContainsKey(sum) ? sums[sum] + 1 : 1;
            }

            return retCount;
        }

        // 32 - https://leetcode.com/problems/longest-valid-parentheses/
        public int LongestValidParentheses(string s)
        {
            int l = 0;
            int r = 0;
            int max = 0;
            foreach (char c in s)
            {
                if (c == '(') l++;
                else if (c == ')') r++;

                if (r > l)
                {
                    l = 0;
                    r = 0;
                }
                else if (r == l) max = Math.Max(max, r * 2);
            }

            // Handle (()
            l = 0;
            r = 0;
            foreach (char c in s.Reverse())
            {
                if (c == '(') l++;
                else if (c == ')') r++;

                if (l > r)
                {
                    l = 0;
                    r = 0;
                }
                else if (r == l) max = Math.Max(max, r * 2);
            }

            return max;
        }

        // 5 - https://leetcode.com/problems/longest-palindromic-substring/
        public string LongestPalindrome(string s)
        {
            string longestSub = "";

            Action<int, int> tryMax = (l, r) =>
            {
                while (l >= 0 && r < s.Length && s[l] == s[r])
                {
                    l--;
                    r++;
                }

                if (r - l + 1 > longestSub.Length)
                {
                    longestSub = s.Substring(l, r - l + 1);
                }
            };

            for (int i = 0; i < s.Length; i++)
            {
                tryMax(i, i);
                tryMax(i, i + 1);
            }

            return longestSub;
        }

        // 11 - https://leetcode.com/problems/container-with-most-water/
        public int MaxArea(int[] height)
        {
            // REVIEW: 贪心思想 - 先移动短的
            int max = 0;
            int i = 0;
            int j = height.Length - 1;
            while (i < j)
            {
                if (Math.Min(height[i], height[j]) * (j - i) > max)
                {
                    max = Math.Min(height[i], height[j]) * (j - i);
                }

                if (height[i] < height[j]) i++;
                else j--;
            }

            return max;
        }
        
        // 42 - https://leetcode.com/problems/trapping-rain-water/
        public int Trap(int[] height) {
            int count = 0;
        
            // Think about cornor cases.

            for (int i = 0; i < height.Length - 1;)
            {
                if (height[i] == 0)
                {
                    i++;
                    continue;
                }

                int maxVal = -1;
                int maxIdx = -1;
                for (int j = i + 1; j < height.Length; j++)
                {
                    if (height[j] > maxVal)
                    {
                        maxVal = height[j];
                        maxIdx = j;
                    }

                    if (height[j] >= height[i])
                    {
                        // In this case, the first if has been executed.
                        break;
                    }
                }

                int level = Math.Min(height[maxIdx], height[i]);
                for (int j = i + 1; j < maxIdx; j++)
                {
                    count += (level - height[j]);
                }
                
               // Console.WriteLine($"FROM: {height[i]} MAX:{maxVal} IDX:{i}-{maxIdx}, C:{count}");

                i = maxIdx;
            }

            return count;
        }
        
        // 844 - https://leetcode.com/problems/backspace-string-compare/
        public bool BackspaceCompare(string S, string T) {
            // REVIEW: 倒序处理
            int sIndex = S.Length - 1;
            int tIndex = T.Length - 1;
            while(true) {
                sIndex = MoveIndexBack(S, sIndex);
                tIndex = MoveIndexBack(T, tIndex);
                
                if(sIndex < 0 || tIndex < 0) return sIndex == tIndex;
                if (S[sIndex] != T[tIndex]) return false;

                sIndex--;
                tIndex--;
            }
        }
    
        private int MoveIndexBack(string str, int index) {
            int moveCount = 0;
            while(index >= 0 && (str[index] == '#' || moveCount > 0))
            {
                if (str[index] == '#') moveCount++;
                else moveCount--;
                index--;
            }

            // MOVE: Until the first char that should be compared, or -1.
            return index;
        }
        
        // 155 - MinStack
        public class MinStack {
            // REVIEW: 边界条件，this._currMin >= x 也需要 Push 两次
            private int _currMin = Int32.MaxValue;
            private readonly Stack<int> _stack;
    
            /** initialize your data structure here. */
            public MinStack() {
                this._stack = new Stack<int>();
            }
    
            public void Push(int x) {
                if (this._currMin >= x) {
                    // When stack.Pop() == currMin, we pop again for next min value...
                    // The compare should be >= since the min value could be pushed more than once.
                    this._stack.Push(this._currMin);
                    this._currMin = x;
                }
        
                this._stack.Push(x);
            }
    
            public void Pop() {
                int val = this._stack.Pop();
                if (val == this._currMin) {
                    // currMin being poped...
                    this._currMin = this._stack.Pop();
                }
            }
    
            public int Top() {
                return this._stack.Peek();
            }
    
            public int GetMin() {
                return this._currMin;
            }
        }
        
        public void Run()
        {
            Console.WriteLine(this.BackspaceCompare("bxj##tw","bxo#j##tw")); // TRUE
            Console.WriteLine(this.BackspaceCompare("nzp#o#g", "b#nzp#o#g")); // TRUE
        }
    }
}