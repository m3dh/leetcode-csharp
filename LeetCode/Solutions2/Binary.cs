namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using LeetCode.Csharp.Common;
    using Newtonsoft.Json;

    public class Binary
    {
        // 50 - https://leetcode.com/problems/powx-n/
        public double MyPow(double x, int n)
        {
            // REVIEW: 二分法
            if (n == 0) return 1;
            if (n == Int32.MinValue) return 1.0 / (x * MyPow(x, - (n+1)));
            if (n < 0) return 1.0 / MyPow(x, -n);
            if (n % 2 == 0) {
                var half = MyPow(x, n / 2);
                return half * half;
            } else {
                return MyPow(x, n - 1) * x;
            }
        }

        // 338 - https://leetcode.com/problems/counting-bits/
        public int[] CountBits(int num)
        {
            if (num == 0) return new[]{0};
            
            int[] result = new int[num+1];
            result[0] = 0;
            result[1] = 1;
            
            for(int i=2;i<=num;i++) {
                if (i%2==1) result[i] = result[i-1]+1;
                else result[i] = result[i/2];
            }
        
            return result;
        }

        public int LeftMostColumnWithOne(BinaryMatrix binaryMatrix)
        {
            IList<int> dimensions = binaryMatrix.Dimensions();

            int leftMostCol = dimensions[1]; // A invalid column.
            for (int i = 0; i < dimensions[0]; i++)
            {
                if (binaryMatrix.Get(i, leftMostCol - 1) == 1)
                {
                    int l = 0;
                    int r = leftMostCol - 1;
                    int mid = leftMostCol;
                    while (l <= r)
                    {
                        mid = (l + r) / 2;
                        if (binaryMatrix.Get(i, mid) == 1)
                        {
                            if (mid == 0 || binaryMatrix.Get(i, mid - 1) == 0)
                            {
                                // return mid;
                                break;
                            }
                            else
                            {
                                r = mid - 1; // arr[mid - 1] -> 1
                            }
                        }
                        else
                        {
                            l = mid + 1;
                        }
                    }

                    if (mid < leftMostCol)
                    {
                        leftMostCol = mid;
                    }

                    // Cannot find any better results.
                    if (leftMostCol == 0) break;
                }
            }

            return leftMostCol < dimensions[1] ? leftMostCol : -1;
        }
        
        // https://leetcode.com/problems/first-bad-version/
        public class VersionControl
        {
            public int FirstBadVer { get; set; }
            
            public bool IsBadVersion(int v)
            {
                return v >= FirstBadVer;
            }
        }

        public class Solution1 : VersionControl
        {
            public int FirstBadVersion(int n)
            {
                int l = 1;
                int r = n;
                while (l < r)
                {
                    int mid = (l + r) / 2;
                    if (IsBadVersion(mid))
                    {
                        r = mid;
                    }
                    else
                    {
                        l = mid + 1;
                    }
                }

                return IsBadVersion(l) ? l : r;
            }
        }
        
        // https://leetcode.com/problems/sum-of-two-integers/
        public int GetSum(int a, int b)
        {
            // REVIEW: INT数真神奇！相加的时候不需要考虑符号；额外处理成uint
            int mask = 1;
            int carry = 0;
            int ret = 0;
            uint au = (uint)a;
            uint bu = (uint)b;
            while ((au | bu) != 0 || carry > 0)
            {
                if (mask == 0) break;
                
                uint ba = au & 1;
                uint bb = bu & 1;
                if ((ba & bb) == 1)
                {
                    if (carry == 1)
                    {
                        ret |= mask;
                    }

                    carry = 1;
                }
                else if ((ba ^ bb) == 1)
                {
                    if (carry == 0)
                    {
                        ret |= mask;
                    }
                }
                else
                {
                    if (carry == 1)
                    {
                        ret |= mask;
                    }

                    carry = 0;
                }

                mask <<= 1;
                au >>= 1;
                bu >>= 1;
                
               // Console.WriteLine($"a:{au:x8}, b:{bu:x8}, r:{ret:x8}, c:{carry}");
            }
            
            return ret;
        }

        public string FrequencySort(string s)
        {
            var ss = s
                .GroupBy(c => c)
                .OrderByDescending(g => g.Count())
                .Select(g => string.Join("", Enumerable.Repeat(g.Key.ToString(), g.Count())));
            return string.Join("", ss);
        }

        // https://leetcode.com/problems/range-sum-query-mutable/
        public class NumArray
        {
            private readonly int[] _segs;
            private readonly int _sz;
            private readonly int[] _nums;
            
            public NumArray(int[] nums)
            {
                this._nums = nums;
                this._sz = (int) Math.Floor(Math.Sqrt(nums.Length));
                this._segs = new int[nums.Length / this._sz + 1];
                for (int i = 0; i < nums.Length; i++)
                {
                   this. _segs[i / this._sz] += nums[i];
                }
            }

            public void Update(int i, int val)
            {
                this._segs[i / this._sz] -= (this._nums[i] - val);
                this._nums[i] = val;
            }

            public int SumRange(int i, int j)
            {
                int res = 0;
                int beginSeg = i / this._sz;
                int endSeg = j / this._sz;
                for (int s = beginSeg; s <= endSeg; s++)
                {
                    res += this._segs[s];
                }

                for (int k = beginSeg * this._sz; k < i; k++)
                {
                    res -= this._nums[k];
                }

                for (int k = j + 1; k < (1+ endSeg) * (this._sz) && k < this._nums.Length; k++)
                {
                    res -= this._nums[k];
                }

                return res;
            }
        }
        
        // https://leetcode.com/problems/random-pick-with-weight/
        public class Solution {
            
            private readonly int[] _w;
            private readonly int _totalSum;

            public Solution(int[] w)
            {
                this._w = new int[w.Length];
                int sum = 0;
                for (int i = 0; i < w.Length; i++)
                {
                    sum += w[i];
                    this._w[i] = sum;
                }

                this._totalSum = sum;
            }

            public int PickIndex()
            {
                int randNum = this._rand.Next(this._totalSum); // 0 - (ts-1)
                int l = 0;
                int r = this._w.Length - 1;

                // i when randNum < _w[i] but randNum >= _w[i-1]
                while (l < r)
                {
                    int mid = (l + r) / 2;
                    if (randNum >= this._w[mid])
                    {
                        l = mid + 1; // +1 to break the loop!
                    }
                    else if (mid == 0 || randNum >= this._w[mid - 1])
                    {
                        return mid;
                    }
                    else
                    {
                        r = mid;
                    }
                }

                return r;
            }

            // https://leetcode.com/problems/random-pick-with-blacklist/
            private readonly Dictionary<int, int> _mapping;
            private readonly Random _rand = new Random(DateTimeOffset.Now.Millisecond);
            private readonly int _finalCount;

            public Solution(int N, int[] blacklist)
            {
                int finalCount = N - blacklist.Length;
                Dictionary<int, int> mapping = new Dictionary<int, int>();

                if (blacklist.Length > 0)
                {
                    HashSet<int> blHash = new HashSet<int>(blacklist);
                    List<int> blToMap = blacklist.Where(b => b < finalCount).ToList();
                    for (int i = finalCount; i < N; i++)
                    {
                        if (!blHash.Contains(i))
                        {
                            mapping.Add(blToMap[0], i);
                            blToMap.RemoveAt(0);
                        }
                    }
                }

                this._mapping = mapping;
                this._finalCount = finalCount;
            }
    
            public int Pick()
            {
                int randNum = this._rand.Next(this._finalCount);
                if (this._mapping.TryGetValue(randNum, out int mapped)) return mapped;
                return randNum;
            }
        }

        public int HIndex(int[] citations)
        {
            if (citations.Length == 0) return 0;
            int l = 0;
            int r = citations.Length - 1;
            while (l < r)
            {
                int mid = (l + r) / 2;
                if (citations[mid] == citations.Length - mid)
                {
                    return citations.Length - mid;
                }
                else if (citations[mid] < citations.Length - mid)
                {
                    l = mid + 1;
                }
                else
                {
                    r = mid;
                }
            }

            if (citations[l] >= citations.Length - l)
            {
                return citations.Length - l;
            }
            else
            {
                // Handle a corner case: We've reached the last element still not qualified.
                return 0;
            }
        }

        public int[] SearchRange(int[] nums, int target)
        {
            int l = FindLeft(nums, target);
            int r = FindRight(nums, target);
            return new[] { l, r };
        }

        private int FindRight(int[] nums, int target)
        {
            int l = 0;
            int r = nums.Length - 1;
            while (l <= r)
            {
                int mid = (l + r) / 2;
                if (nums[mid] < target)
                {
                    l = mid + 1;
                }
                else if (nums[mid] > target)
                {
                    r = mid - 1;
                }
                else
                {
                    if (mid == nums.Length-1 || nums[mid + 1] > target)
                    {
                        return mid;
                    }
                    else
                    {
                        l = mid + 1;
                    }
                }
            }

            return -1;
        }

        private int FindLeft(int[] nums, int target)
        {
            int l = 0;
            int r = nums.Length - 1;
            while (l <= r)
            {
                int mid = (l + r) / 2;
                if (nums[mid] < target)
                {
                    l = mid + 1;
                }
                else if (nums[mid] > target)
                {
                    r = mid - 1;
                }
                else
                {
                    if (mid == 0 || nums[mid - 1] < target)
                    {
                        return mid;
                    }
                    else
                    {
                        r = mid - 1;
                    }
                }
            }

            return -1;
        }

        // https://leetcode.com/problems/koko-eating-bananas/solution/
        public int MinEatingSpeed(int[] piles, int H)
        {
            if (piles == null || piles.Count() == 0) return 0;
            if (piles.Count() > H) return -1; // impossible

            int l = 1;
            int r = piles.Max();

            while (l < r)
            {
                int m = l + (r - l) / 2; // l + r/2 - l/2
                if (CanFinish(m, piles, H))
                {
                    r = m;
                }
                else
                {
                    l = m + 1;
                }
            }

            return CanFinish(l, piles, H) ? l : r;
        }

        private bool CanFinish(int k, int[] piles, int H)
        {
            int count = 0;
            foreach (int pile in piles)
            {
                count += (pile / k + (pile % k > 0 ? 1 : 0));
            }

            return count <= H;
        }

        // https://leetcode.com/problems/find-peak-element/
        // https://segmentfault.com/a/1190000016825704
        public int FindPeakElement(int[] nums)
        {
            // REVIEW: 二分查找寻找区域极值
            int l = 0;
            int r = nums.Count() - 1;
            while (l < r)
            {
                int m = l + (r - l) / 2;
                // if (m == nums.Count() - 1) return m; - Won't happen, this need l = r

                if (nums[m] > nums[m + 1])
                {
                    r = m;
                }
                else
                {
                    l = m + 1;
                }
            }

            return l;
        }

        // https://leetcode.com/problems/find-in-mountain-array
        public int FindInMountainArray(int target, int[] mountainArr)
        {
            // 先找极值，再二分搜索
            int len = mountainArr.Length;
            int l = 0;
            int r = len - 1;
            while (l < r)
            {
                Console.WriteLine($"L:{l}, R:{r}");
                int m = l + (r - l) / 2;
                if (mountainArr[m] < mountainArr[m + 1])
                {
                    l = m + 1;
                }
                else
                {
                    r = m ;
                }
            }

            int peak = r;

            l = 0;

            while (l <= r)
            {
                int m = l + (r - l) / 2;
                int mv = mountainArr[m];
                if (mv == target)
                {
                    return m;
                }
                else if (mv < target)
                {
                    l = m + 1;
                }
                else
                {
                    r = m - 1;
                }
            }

            l = peak;
            r = len - 1;

            while (l <= r)
            {
                int m = l + (r - l) / 2;
                int mv = mountainArr[m];
                if (mv == target)
                {
                    return m;
                }
                else if (mv > target)
                {
                    l = m + 1;
                }
                else
                {
                    r = m - 1;
                }
            }

            return -1;
        }

        public int BulbSwitch(int n)
        {
            // 只有平方数能确保toggle奇数次！例如：16 - (1,16)(2,8)(4,4)，每次被整除都是一对，而4这个因数只会出现一次！
            int cnt = 0;
            for (int i = 1; i <= n; i++)
            {
                if (i * i <= n)
                {
                    Console.WriteLine($"{i * i}");
                    cnt++;
                }
                // else break;
            }

            return cnt;
        }

        public int MissingElement(int[] nums, int k)
        {
            // [4,7,9,10] (5,6,8) -> (10 - 4 - 4)
            int totalMissing = nums[nums.Length - 1] - nums[0] - nums.Length + 1;
            if (totalMissing < k)
            {
                return nums[nums.Length - 1] + k - totalMissing;
            }
            else
            {
                int l = 0;
                int r = nums.Length - 1;

                // find left = right index such that 
                // missing(left - 1) < k <= missing(left)
                while (l < r)
                {
                    int m = l + (r - l) / 2; // m = 2, (9-4-[2+1])
                    int curMissing = nums[m] - nums[0] - m;
                    if (curMissing < k)
                    {
                        l = m + 1;
                    }
                    else
                    {
                        r = m;
                    }
                }

                return nums[l - 1] + k - (nums[l - 1] - nums[0] - (l - 1));
            }
        }

        // https://leetcode.com/problems/search-in-rotated-sorted-array-ii/
        public bool Search(int[] nums, int target)
        {
            // REVIEW: Rotated Binary Search
            return SearchI(nums, target) != -1;
        }

        public int SearchI(int[] nums, int target)
        {
            if (nums.Length == 0) return -1;

            int l = 0;
            int r = nums.Length - 1;
            while (l <= r)
            {
                int mid = (l + r) / 2;

                if (nums[mid] == target) return mid;

                Console.WriteLine($"L:{l} R:{r} M:{mid}");

                if (nums[mid] < nums[r]) // rhs
                {
                    if (nums[mid] < target && nums[r] >= target)
                    {
                        // between mid,right
                        l = mid + 1;
                    }
                    else
                    {
                        r = mid - 1;
                    }
                }
                else if (nums[mid] > nums[r]) // lhs
                {
                    if (nums[l] <= target && target < nums[mid])
                    {
                        r = mid - 1;
                    }
                    else
                    {
                        l = mid + 1;
                    }
                }
                else // nums[mid] == nums[r]
                {
                    // 因为同样的值还是被nums[mid]保留了
                    r--;
                }
            }

            return -1;
        }

        // https://leetcode.com/problems/find-right-interval/
        public int[] FindRightInterval(int[][] intervals)
        {
            var sorted = intervals.Select((a, i) => new int[] { a[0], a[1], i }).OrderBy(itv => itv[0]).ToList();


            Console.WriteLine(JsonConvert.SerializeObject(sorted));

            int[] ret = new int[intervals.Length];
            for (int i = 0; i < intervals.Length; i++)
            {
                ret[i] = FindIndex(sorted, intervals[i][1]);
            }

            return ret;
        }

        // should find l and greater or equal to val (or -1)
        private int FindIndex(List<int[]> sorted, int val)
        {
            int l = 0;
            int r = sorted.Count - 1;
            while (l < r)
            {
                int m = l + (r - l) / 2;
                if (sorted[m][0] < val)
                {
                    l = m + 1;
                }
                else if (sorted[m][0] == val)
                {
                    return sorted[m][2];
                }
                else
                {
                    r = m;
                }
            }

            if (sorted[l][0] >= val) return sorted[l][2];
            else return -1;
        }

        // https://leetcode.com/problems/split-array-largest-sum/
        public int SplitArray(int[] nums, int m)
        {
            // REVIEW: 注意一下这种类型的二分的写法
            long minSum = nums.Max();
            long maxSum = nums.Sum();
            long ans = maxSum;

            // binary search for the left edge of count m.
            long l = minSum;
            long r = maxSum;
            while (l <= r)
            {
                long mid = l + (r - l) / 2;
                int splits = SplitArrayWithMaxSum(nums, mid);

                if (splits > m)
                {
                    l = mid + 1;
                }
                else if (splits <= m)
                {
                    ans = Math.Min(ans, mid);
                    r = mid - 1;
                }
            }

            return (int)ans;
        }

        private int SplitArrayWithMaxSum(int[] nums, long maxSum)
        {
            int cnt = 0;
            long sum = int.MaxValue;
            foreach (int num in nums)
            {
                if (sum + num > maxSum)
                {
                    cnt++;
                    sum = num;
                }
                else
                {
                    sum += num;
                }
            }

            return cnt;
        }

        public void Run()
        {
            Console.WriteLine(JsonConvert .SerializeObject(  FindRightInterval(new[] { new[] { 1, 2 }, new[] { 2, 3 }, new[] { 0, 1 }, new[] { 3, 4 } })));
        }
    }
}