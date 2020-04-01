namespace LeetCode.Csharp.Solutions2
{
    using System.Collections.Generic;

    public class FindDuplicatesSolution
    {
        public IList<int> FindDuplicates(int[] nums)
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