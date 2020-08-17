namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LeetCode.Csharp.Common;
    using Newtonsoft.Json;

    public class Arrays2
    {
        // https://leetcode.com/problems/max-chunks-to-make-sorted/
        public int MaxChunksToSorted1(int[] arr)
        {
            int idx = 0;
            int cnt = 0;

            List<int> expecting = new List<int>();
            HashSet<int> found = new HashSet<int>();

            while (idx < arr.Length)
            {
                if (idx == arr[idx])
                {
                    idx++;
                    cnt++;
                }
                else
                {
                    while (idx < arr.Length)
                    {
                        expecting.Add(idx);
                        found.Add(arr[idx]);

                        if (expecting.All(e => found.Contains(e)))
                        {
                            cnt++;
                            idx++;
                            expecting.Clear();
                            found.Clear();
                            break;
                        }

                        idx++;
                    }
                }
            }

            return cnt;
        }

        // https://leetcode.com/problems/max-chunks-to-make-sorted-ii/
        public int MaxChunksToSorted(int[] arr)
        {
            // REVIEW: 有 O(n) 的解法，去看题解！

            var sorted = arr.OrderBy(a => a).ToArray();

            int cnt = 0;
            int delta = 0;
            Counter<int> cntr = new Counter<int>();
            for (int i = 0; i < arr.Length; i++)
            {
                int sc = cntr.Incr(sorted[i]);
                if (sc == 1)
                {
                    delta++;
                }
                else if (sc == 0)
                {
                    delta--;
                }

                int dc = cntr.Decr(arr[i]);
                if (dc == -1)
                {
                    delta++;
                }
                else if (dc == 0)
                {
                    delta--;
                }

                if (delta == 0)
                {
                    cnt++;
                }
            }

            return cnt;
        }

        // https://leetcode.com/problems/patching-array/
        public int MinPatches(int[] nums, int n)
        {
            // REVIEW: 记录miss，miss以下的数字都可获得！
            long miss = 1;
            int i = 0;
            int cnt = 0;
            while (miss <= n)
            {
                if (i < nums.Count() && nums[i] <= miss)
                {
                    miss += nums[i];
                    i++;
                }
                else
                {
                    cnt++;
                    miss += miss;
                }
            }

            return cnt;
        }

        // https://leetcode.com/problems/count-triplets-that-can-form-two-arrays-of-equal-xor/
        public int CountTriplets(int[] arr)
        {
            int[] xorLeft = new int[arr.Length];
            xorLeft[0] = arr[0];
            for (int i = 1; i < arr.Length; i++)
            {
                xorLeft[i] = xorLeft[i-1] ^ arr[i];
            }

            int cnt = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    for (int k = j; k < arr.Length; k++)
                    {
                        int x1; // 第一个数组的亦或值
                        if (i == 0)
                        {
                            x1 = xorLeft[j - 1];
                        }
                        else
                        {
                            x1 = (xorLeft[i - 1] ^ xorLeft[j - 1]);
                        }

                        // 第一个数组与第二个数组亦或值相同时，结果加一
                        if (x1 == (xorLeft[j - 1] ^ xorLeft[k]))
                        {
                            Console.WriteLine($"{i},{j},{k}");
                            cnt++;
                        }
                    }
                }
            }

            return cnt;
        }

        public string PushDominoes(string dominoes)
        {
            if (string.IsNullOrEmpty(dominoes))
            {
                return dominoes;
            }

            char[] dChars = dominoes.ToArray();

            int pIdx = 0;
            char prev = dChars[0];
            for (int i = 1; i < dominoes.Length; i++)
            {
                if (dChars[i] != '.')
                {
                    if (dChars[i] == 'L')
                    {
                        if (prev == '.' || prev == 'L')
                        {
                            for (int j = pIdx; j < i; j++) dChars[j] = 'L';
                        }
                        else
                        {
                            // prev == 'R' # R...L
                            int slots = i - pIdx - 1;
                            for (int j = 1; j <= slots / 2; j++)
                            {
                                dChars[pIdx + j] = 'R';
                                dChars[i - j] = 'L';
                            }

                            if (slots % 2 == 1)
                            {
                                dChars[pIdx + slots / 2 + 1] = '.';
                            }
                        }

                        prev = 'L';
                        pIdx = i;
                    }
                    else
                    {
                        prev = dChars[i];
                        pIdx = i;
                    }
                }
                else
                {
                    if (prev == 'R') dChars[i] = 'R';
                }
            }

            return new string(dChars);
        }

        // https://leetcode.com/problems/interleaving-string/
        public bool IsInterleave(string s1, string s2, string s3)
        {
            if (s1.Length + s2.Length != s3.Length) return false;

            // REVIEW: DP复杂度分析第一招：因为最多要填满整个DP数组，所以复杂度就是DP数组大小.
            // REVIEW: 一维数组即可，因为在当前计算中dp[j]的值就是dp[i-1][j]
            bool[] dp = new bool[s2.Length+1];
            for (int i = 0; i <= s1.Length; i++)
            {
                for (int j = 0; j <= s2.Length; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        dp[j] = true;
                    }
                    else if (i == 0)
                    {
                        dp[j] = dp[j - 1] && s2[j - 1] == s3[i + j - 1];
                    }
                    else if (j == 0)
                    {
                        dp[j] = dp[j] && s1[i - 1] == s3[i + j - 1];
                    }
                    else
                    {
                        dp[j] = (dp[j - 1] && s2[j - 1] == s3[i + j - 1]) || (dp[j] && s1[i - 1] == s3[i + j - 1]);
                    }
                }
            }

            return dp[s2.Length];
        }

        public void WiggleSort(int[] nums)
        {
            // REVIEW: 第一个数必须从中间开始取（所以此处从左半的最后一个开始，以避免可能的相等情况）
            int[] sNums = nums.OrderBy(n => n).ToArray();
            int l = (nums.Count() - 1) / 2; // 4 -> 1, 5 -> 2
            int r = nums.Count() - 1;

            for (int i = 0; i < nums.Length; i++)
            {
                if (i % 2 == 0)
                {
                    nums[i] = sNums[l--];
                }
                else
                {
                    nums[i] = sNums[r--];
                }
            }
        }

        // https://leetcode.com/problems/4sum/
        public IList<IList<int>> FourSum(int[] nums, int target)
        {
            nums = nums.OrderBy(n => n).ToArray();
            Dictionary<int, List<Tuple<int, int>>> left = new Dictionary<int, List<Tuple<int, int>>>();
            List<IList<int>> ret = new List<IList<int>>();
            for (int i = 1; i < nums.Length - 2; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    int lSum = nums[i] + nums[j];
                    if (!left.TryGetValue(lSum, out List<Tuple<int, int>> l))
                    {
                        l = new List<Tuple<int, int>>();
                        left[lSum] = l;
                    }

                    l.Add(Tuple.Create(nums[j], nums[i]));
                }

                for (int j = i + 1; j < nums.Length - 1; j++)
                {
                    for (int k = j + 1; k < nums.Length; k++)
                    {
                        int rSum = nums[j] + nums[k];
                        if (left.TryGetValue(target - rSum, out List<Tuple<int, int>> l) && l.Count > 0)
                        {
                            foreach (Tuple<int, int> t in l)
                            {
                                ret.Add(new[] { t.Item1, t.Item2, nums[j], nums[k] });
                            }
                        }
                    }
                }
            }

            HashSet<string> dedup = new HashSet<string>();
            return ret.Where(p =>
            {
                string k = string.Join(",", p);
                return dedup.Add(k);
            }).ToArray();
        }

        public int MaxScore(int[] cardPoints, int k)
        {
            // REVIEW: Sliding window!

            if (cardPoints.Length == 0)
            {
                return 0;
            }
            else if (cardPoints.Length <= k)
            {
                return cardPoints.Sum();
            }

            // This is actually a sliding window of size k
            int sum = 0;
            for (int i = 0; i < k; i++) sum += cardPoints[i];

            int max = sum;
            for (int i = 0; i < k; i++)
            {
                sum -= cardPoints[k - i - 1];
                sum += cardPoints[cardPoints.Length - i - 1];
                max = Math.Max(max, sum);
            }

            return max;
        }

        public int[] ThreeEqualParts(int[] a)
        {
            // REVIEW: IDEA - 通过计算1的数量来分段

            int oneCount = a.Count(i => i == 1);
            if (oneCount % 3 == 0 && oneCount > 0)
            {
                int partOneCount = oneCount / 3;
                int lastOneIdx = a.Length - 1;
                while (a[lastOneIdx] == 0) lastOneIdx--;

                int trailingZeroCount = a.Length - lastOneIdx - 1;

                int index = 0;
                int curOneCnt = 0;
                while (curOneCnt < partOneCount)
                {
                    if (a[index] == 1) curOneCnt++;
                    index++;
                }

                for (int i = 0; i < trailingZeroCount; i++)
                {
                    if (a[index] != 0) return new[] { -1, -1 };
                    index++;
                }

                int secondStart = index;
                curOneCnt = 0;
                while (curOneCnt < partOneCount)
                {
                    if (a[index] == 1) curOneCnt++;
                    index++;
                }

                for (int i = 0; i < trailingZeroCount; i++)
                {
                    if (a[index] != 0) return new[] { -1, -1 };
                    index++;
                }

                int thirdStart = index;

                int al = secondStart - 1;
                int bl = thirdStart - 1;
                int cl = a.Length - 1;

                while (partOneCount > 0)
                {
                    int ct = a[al--] + a[bl--] + a[cl--];
                    if (ct != 0 && ct != 3) return new[] { -1, -1 };
                    partOneCount--;
                }

                return new[] { secondStart - 1, thirdStart };
            }

            if (oneCount == 0)
            {
                return new[] { 0, a.Length - 1 };
            }

            return new[] { -1, -1 };
        }

        // https://leetcode.com/problems/maximum-product-subarray/
        public int MaxProduct(int[] nums)
        {
            int max = 1;
            int min = 1;
            int ret = int.MinValue;

            // 2,3,-2,4
            // (2,0), (6,0), (0,-12), (4, -48)

            foreach (int num in nums)
            {
                int a = max * num;
                int b = min * num;
                max = Math.Max(num, Math.Max(a, b));
                min = Math.Min(num, Math.Min(a, b));
                ret = Math.Max(max, ret);
            }

            return ret;
        }

        public int FindNthDigit(int n)
        {
            long an = n;
            long l = 1;
            long r = 9;
            int w = 1;

            while (true)
            {
                if (an <= (r - l + 1) * w)
                {
                    long num = (an - 1) / w + l;
                    long cnt = (an - 1) % w;
                    return int.Parse(num.ToString()[(int)cnt].ToString());
                }
                else
                {
                    var clen = (r - l + 1) * (long) w;
                    an -= clen;
                    l *= 10;
                    r = r * 10 + 9;
                    w++;
                }
            }
        }

        // https://leetcode.com/problems/remove-duplicate-letters/
        public string RemoveDuplicateLetters(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;

            // REVIEW: 用类似最大栈的方法来求值！
            char[] ret = new char[s.Length];
            char[] counters = new char[26];
            foreach (char c in s)
            {
                counters[c - 'a']++;
            }

            int i = 0;
            HashSet<char> contains = new HashSet<char>();
            foreach (char c in s)
            {
                counters[c - 'a']--;

                if (contains.Contains(c)) continue;

                while (i > 0 && ret[i - 1] > c && counters[ret[i - 1] - 'a'] > 0)
                {
                    // we can remove char @ i to make the string smaller, and we still have more same char in the future.
                    contains.Remove(ret[i - 1]);
                    i--;
                }

                ret[i++] = c;
                contains.Add(c);

               // Console.WriteLine(new string(ret.Take(i).ToArray()));
            }

            return new string(ret.Take(i).ToArray());
        }

        // https://leetcode.com/problems/largest-rectangle-in-histogram/
        public int LargestRectangleArea(int[] heights)
        {
            // REVIEW: 递增栈思想的运用

            if (heights.Count() == 0) return 0;

            var hList = new List<int>(heights);
            hList.Add(0);

            int max = 0;
            Stack<int> s = new Stack<int>();
            for (int i = 0; i < hList.Count(); i++)
            {
                while (s.Count() > 0 && hList[s.Peek()] >= hList[i])
                {
                    int curTop = s.Pop();

                    // all numbers got pop-ed by current number previous must be larger than it.
                    int area = hList[curTop] * (s.Count() == 0 ? i : (i - s.Peek() - 1));
                    max = Math.Max(max, area);
                }

                s.Push(i);
            }

            return max;
        }

        public void Run()
        {
        }
    }
}
