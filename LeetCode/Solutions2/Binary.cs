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
    }
}