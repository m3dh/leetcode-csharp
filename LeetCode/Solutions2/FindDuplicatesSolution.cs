namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Collections.Generic;

    public class FindDuplicatesSolution
    {
        // 442 - https://leetcode.com/problems/find-all-duplicates-in-an-array/
        public IList<int> FindDuplicates(int[] nums)
        {
            List<int> ret = new List<int>();

            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[Math.Abs(nums[i]) - 1] < 0)
                {
                    ret.Add(Math.Abs(nums[i]));
                }
                else
                {
                    nums[Math.Abs(nums[i]) - 1] = -nums[Math.Abs(nums[i]) - 1];
                }
            }

            return ret;
        }
        
        // 287 - https://leetcode.com/problems/find-the-duplicate-number/
        public int FindDuplicate(int[] nums)
        {
            return -1;
        }
        
        public IList<int> FindDuplicates2(int[] nums)
        {
            List<int> ret = new List<int>();

            // Scan 1: Swap
            for (int i = 0; i < nums.Length; i++)
            {
                // Ensures for any member number x in array, array[x] = x.
                // So in the second scan, if found any array[y] <> y, array[y] is a duplication.
                while (nums[nums[i]] != nums[i])
                {
                    int tmp = nums[i];
                    nums[i] = nums[tmp];
                    nums[tmp] = tmp;
                }
            }

            // Scan 2: Find duplications
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] != i)
                {
                    ret.Add(nums[i]);
                }
            }

            return ret;
        }
    }
}