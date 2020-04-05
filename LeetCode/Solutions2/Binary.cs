namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Binary
    {
        // 50 - https://leetcode.com/problems/powx-n/
        public double MyPow(double x, int n)
        {
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
    }
}