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

        public class Solution : VersionControl
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

        public void Run()
        {
            Console.WriteLine(GetSum(2, 3));
        }
    }
}