namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using LeetCode.Csharp.Common;

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

        public void Run()
        {
            Solution s = new Solution(2, new int[]{1});
            for(int i=0;i<10;i++)
            Console.WriteLine(s.Pick());
        }
    }
}