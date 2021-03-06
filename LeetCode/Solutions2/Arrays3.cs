﻿namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LeetCode.Csharp.Common;

    class Arrays3
    {
        private class MinMeetingRoomNode : IHeapNode
        {
            public readonly int EndTime;

            public MinMeetingRoomNode(int endTime)
            {
                this.EndTime = endTime;
            }

            public int GetValue()
            {
                return -this.EndTime;
            }
        }

        // https://leetcode.com/problems/meeting-rooms-ii/
        public int MinMeetingRooms(int[][] intervals)
        {
            // REVIEW: Idea - 维护一个堆，里面是现在正在开的会，每来一个新会就把已经开完的会弹出
            // REVIEW: 还有更好的解法，不需要堆，就是维护occupied count就行了，end来了occupied --, begin来了occupied ++

            intervals = intervals.OrderBy(i => i[0]).ThenBy(i => -i[1]).ToArray();
            MaxHeap<MinMeetingRoomNode> roomMinHeap = new MaxHeap<MinMeetingRoomNode>(intervals.Length);

            int max = 0;
            foreach (int[] interval in intervals)
            {
                int startTime = interval[0];
                while (roomMinHeap.Count > 0 && roomMinHeap.GetMax().EndTime <= startTime)
                {
                    roomMinHeap.RemoveMax();
                }

                roomMinHeap.Insert(new MinMeetingRoomNode(interval[1]));
                max = Math.Max(max, roomMinHeap.Count);
            }

            return max;
        }

        // https://leetcode.com/problems/validate-stack-sequences/
        public bool ValidateStackSequences(int[] pushed, int[] popped)
        {
            // REVIEW: Idea - 把pushed数组当成stack来用 - 修改里面的内容

            if (pushed.Length != popped.Length) return false;

            if (pushed.Length == 0) return true;

            int sTop = 0;
            int popIdx = 0;

            for (int pushIndex = 0; pushIndex < pushed.Length; pushIndex++)
            {
                // push
                pushed[sTop] = pushed[pushIndex];
                sTop++;

                while (sTop > 0 && pushed[sTop - 1] == popped[popIdx])
                {
                    sTop--;
                    popIdx++;

                    if (popIdx == popped.Length) return true;
                }
            }

            return false;
        }

        // https://leetcode.com/problems/find-and-replace-in-string/
        public string FindReplaceString(string S, int[] indexes, string[] sources, string[] targets)
        {
            // REVIEW: 看一下 O(n) 的解法
            int[] replaces = new int[S.Length];
            for (int i = 0; i < replaces.Length; i++) replaces[i] = -1;

            for (int i = 0; i < indexes.Length; i++)
            {
                bool canReplace = true;
                for (int j = 0; j < sources[i].Length; j++)
                {
                    if (sources[i][j] != S[indexes[i] + j])
                    {
                        canReplace = false;
                        break;
                    }
                }

                if (canReplace)
                {
                    replaces[indexes[i]] = i;
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < S.Length;)
            {
                if (replaces[i] < 0)
                {
                    sb.Append(S[i]);
                    i++;
                }
                else
                {
                    sb.Append(targets[replaces[i]]);
                    i += sources[replaces[i]].Length;
                }
            }

            return sb.ToString();
        }

        // https://leetcode.com/problems/minimum-domino-rotations-for-equal-row/
        public int MinDominoRotations(int[] A, int[] B)
        {
            // 看似简单，但 tc 不能是每个数出现的总数量，需要去重！

            if (A.Length == 0) return 0;

            int[] ac = new int[6];
            int[] bc = new int[6];
            int[] tc = new int[6];
            for (int i = 0; i < A.Length; i++)
            {
                ac[A[i] - 1]++;
                bc[B[i] - 1]++;
                tc[A[i] - 1]++;
                if (B[i] != A[i])
                {
                    tc[B[i] - 1]++;
                }
            }

            int min = int.MaxValue;
            for (int i = 0; i < 6; i++)
            {
                if (tc[i] == A.Length)
                {
                    min = Math.Min(min, A.Length - Math.Max(ac[i], bc[i]));
                }
            }

            return min == int.MaxValue ? -1 : min;
        }

        // https://leetcode.com/problems/string-transforms-into-another-string/
        public bool CanConvert(string str1, string str2)
        {
            // REVIEW: 每个不同字符map到一个字符，注意最后避免26个字母成环的检测
            Dictionary<char, char> map = new Dictionary<char, char>();
            for (int i = 0; i < str1.Length; i++)
            {
                char c1 = str1[i];
                char c2 = str2[i];
                if (map.TryGetValue(c1, out char cm1))
                {
                    c1 = cm1;
                }
                else
                {
                    map[c1] = c2;
                    c1 = c2;
                }

                if (c1 != c2) return false;
            }

            if (map.Values.Distinct().Count() == 26)
            {
                return map.Keys.All(k => map[k] == k);
            }

            return true;
        }

        // https://leetcode.com/problems/minimum-window-subsequence/
        // 用的还是普通最长子序列的 DP，但传递的是 S 中的起始位置
        public string MinWindow(string S, string T)
        {
            // REVIEW: 重写并改成不是S.Length+1的！
            int start = -1;
            int minLen = int.MaxValue;
            int[][] dp = new int[S.Length][];
            for (int i = 0; i < S.Length; i++)
            {
                dp[i] = new int[T.Length];
                for (int j = 0; j < T.Length; j++) dp[i][j] = -1;
            }

            for (int i = 0; i < S.Length; i++)
            {
                for (int j = 0; j < T.Length && j <= i; j++)
                {
                    if (S[i] == T[j])
                    {
                        // now pass this start index to following items
                        if (j == 0 || i == 0)
                        {
                            dp[i][j] = i;
                        }
                        else
                        {
                            // from previous items
                            dp[i][j] = dp[i - 1][j - 1];
                        }
                    }
                    else
                    {
                        dp[i][j] = i == 0 ? -1 : dp[i - 1][j]; // dp[i][j-1] is not considered as j must be all matched.
                    }
                }

                if (dp[i][T.Length - 1] != -1)
                {
                    int len = i - dp[i][T.Length - 1] + 1;
                    if (len < minLen)
                    {
                        minLen = len;
                        start = dp[i][T.Length - 1];
                    }
                }
            }

            return (start == -1) ? "" : S.Substring(start, minLen);
        }

        // https://leetcode.com/problems/split-array-with-equal-sum/
        public bool SplitArray(int[] nums)
        {
            // REVIEW: Divide-n-conquer
            int[] sums = new int[nums.Length];
            int sum = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                sum += nums[i];
                sums[i] = sum;
            }

            // fact: i >= 1, j >= 3, k >= 5
            // divide by 'j'
            for (int j = 3; j < nums.Length - 3; j++)
            {
                // record all the possibilities to make it o(n) [inner loop]
                HashSet<int> possibles = new HashSet<int>();
                for (int i = 1; i < j - 1; i++)
                {
                    if (sums[i - 1] == sums[j - 1] - sums[i])
                    {
                        possibles.Add(sums[i - 1]);
                    }
                }

                for (int k = j + 2; k < nums.Length - 1; k++)
                {
                    if (sum - sums[k] == sums[k - 1] - sums[j])
                    {
                        if (possibles.Contains(sum - sums[k]))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        // https://leetcode.com/problems/longest-substring-without-repeating-characters/
        public int LengthOfLongestSubstring(string s)
        {
            // REVIEW: 滑动窗口模板
            if (s == null) return 0;

            int maxLen = 0;
            int l = 0;
            int r = 0;
            HashSet<char> window = new HashSet<char>();

            while (l < s.Length)
            {
                if (!window.Contains(s[r]))
                {
                    window.Add(s[r]);
                    maxLen = Math.Max(maxLen, window.Count);
                    if (r == s.Length - 1)
                    {
                        // because moving r is not possible but moving l cannot extend the window, just give up.
                        break;
                    }
                    else
                    {
                        // push right
                        r++;
                    }
                }
                else
                {
                    // move left
                    window.Remove(s[l]);
                    l++;
                }
            }

            return maxLen;
        }

        // https://leetcode.com/problems/contains-duplicate-iii/
        public bool ContainsNearbyAlmostDuplicate(int[] nums, int k, int t)
        {
            // REVIEW: SortedSet -> GetViewBetween -> Inclusive!
            SortedSet<long> ss = new SortedSet<long>();
            int l = 0;
            int r = 0;
            while (l < nums.Length)
            {
                // k = 2 (2-0)
                if (r - l + 1 <= k + 1)
                {
                    if (r < nums.Length)
                    {
                        long lo = Math.Min((long) nums[r] - t, (long) nums[r] + t);
                        long up = Math.Max((long) nums[r] - t, (long) nums[r] + t);
                        var subView = ss.GetViewBetween(lo, up);
                        if (subView.Count > 0)
                        {
                            return true;
                        }

                        ss.Add(nums[r]);
                        r++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    ss.Remove(nums[l]);
                    l++;
                }
            }

            return false;
        }

        // https://leetcode.com/problems/employee-free-time/
        public class Interval
        {
            public int start;
            public int end;

            public Interval() { }
            public Interval(int _start, int _end)
            {
                start = _start;
                end = _end;
            }
        }

        private class Oper
        {
            public int Time;
            public int Op; // 0 - End, 1 - Begin
        }

        // https://leetcode.com/problems/employee-free-time/
        public IList<Interval> EmployeeFreeTime(IList<IList<Interval>> schedule)
        {
            var opers = schedule
                .SelectMany(sc => sc.SelectMany(s => new[] { new Oper { Op = 1, Time = s.start }, new Oper { Op = 0, Time = s.end }, }))
                .OrderBy(op => op.Time)
                .ThenBy(op => op.Op);

            List<Interval> ret = new List<Interval>();
            int prevEnd = -1;
            int started = 0;
            foreach (Oper oper in opers)
            {
                if (oper.Op == 0)
                {
                    started = Math.Max(0, started - 1);
                    prevEnd = oper.Time;
                }
                else
                {
                    if (started == 0)
                    {
                        if (prevEnd >= 0 && prevEnd < oper.Time)
                        {
                            ret.Add(new Interval(prevEnd, oper.Time));
                        }
                    }

                    started++;
                }
            }

            return ret;
        }

        // https://leetcode.com/problems/minimum-swaps-to-make-sequences-increasing/
        public int MinSwap(int[] A, int[] B)
        {
            // swap means the last has been swapped.
            int swapCnt = 1;

            // keep means the last pair hasn't been swapped.
            int keepCnt = 0;
            for (int i = 1; i < A.Length; i++)
            {
                int asp = B[i - 1];
                int bsp = A[i - 1];
                int akp = A[i - 1];
                int bkp = B[i - 1];

                int swapNext = 65536;
                if (A[i] > bsp && B[i] > asp)
                {
                    swapNext = Math.Min(swapNext, swapCnt + 1);
                }

                if (A[i] > bkp && B[i] > akp)
                {
                    swapNext = Math.Min(swapNext, keepCnt + 1);
                }

                int keepNext = 65536;
                if (A[i] > asp && B[i] > bsp)
                {
                    keepNext = Math.Min(keepNext, swapCnt);
                }

                if (A[i] > akp && B[i] > bkp)
                {
                    keepNext = Math.Min(keepNext, keepCnt);
                }

                keepCnt = keepNext;
                swapCnt = swapNext;
            }

            return Math.Min(swapCnt, keepCnt);
        }

        public void Run()
        {
            Console.WriteLine(ContainsNearbyAlmostDuplicate(new[] { 2, 1 }, 1, 1));
        }
    }
}
