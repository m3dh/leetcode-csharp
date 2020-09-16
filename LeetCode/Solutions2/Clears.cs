﻿namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Clears
    {
        // https://leetcode.com/problems/majority-element-ii/
        public IList<int> MajorityElement(int[] nums)
        {
            int num0 = 0;
            int num1 = 0;
            int cnt0 = 0;
            int cnt1 = 0;

            foreach (int num in nums)
            {
                if (cnt0 > 0 && num0 == num)
                {
                    cnt0++;
                }
                else if (cnt1 > 0 && num1 == num)
                {
                    cnt1++;
                }
                else if (cnt0 == 0)
                {
                    cnt0 = 1;
                    num0 = num;
                }
                else if (cnt1 == 0)
                {
                    cnt1 = 1;
                    num1 = num;
                }
                else
                {
                    cnt0--;
                    cnt1--;
                }
            }

            cnt0 = 0;
            cnt1 = 0;
            foreach (int num in nums)
            {
                if (num0 == num) cnt0++;
                if (num1 == num) cnt1++;
            }

            List<int> ret = new List<int>();
            if (cnt0 > nums.Length / 3) ret.Add(num0);
            if (cnt1 > nums.Length / 3 && num0 != num1) ret.Add(num1);
            return ret;
        }

        // https://leetcode.com/problems/ugly-number/
        public bool IsUgly(int num)
        {
            while (num == 1)
            {
                if (num % 2 == 0) num /= 2;
                if (num % 3 == 0) num /= 3;
                if (num % 5 == 0) num /= 5;
                else return false;
            }

            return true;
        }

        public int NthUglyNumber(int n)
        {
            List<int> ret = new List<int>();
            ret.Add(1);
            int i2 = 0;
            int i3 = 0;
            int i5 = 0;
            for (int i = 1; i < n; i++)
            {
                int m2 = ret[i2] * 2, m3 = ret[i3] * 3, m5 = ret[i5] * 5;

                int mn = Math.Min(m2, Math.Min(m3, m5));

                // Whenever equals, we push all the equal indexes up so they'll be no duplication.

                if (mn == m2) ++i2;
                if (mn == m3) ++i3;
                if (mn == m5) ++i5;

                ret.Add(mn);
            }

            return ret.Last();
        }

        // https://leetcode.com/problems/wiggle-sort/
        public void WiggleSort(int[] nums)
        {
            bool less = true;
            for (int i = 1; i < nums.Length; i++)
            {
                if (less)
                {
                    if (nums[i - 1] > nums[i])
                    {
                        int tmp = nums[i];
                        nums[i] = nums[i - 1];
                        nums[i - 1] = tmp;
                    }
                }
                else
                {
                    if (nums[i - 1] < nums[i])
                    {
                        int tmp = nums[i];
                        nums[i] = nums[i - 1];
                        nums[i - 1] = tmp;
                    }
                }

                less = !less;
            }
        }
    }
}