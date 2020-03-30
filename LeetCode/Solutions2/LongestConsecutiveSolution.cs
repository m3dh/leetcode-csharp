namespace LeetCode.Csharp.Solutions2
{
    using System.Collections.Generic;
    using System.Linq;

    public class LongestConsecutiveSolution
    {
        // 128 - https://leetcode.com/problems/longest-consecutive-sequence/
        public int LongestConsecutive(int[] nums)
        {
            if (nums.Length == 0) return 0;
            HashSet<int> exists = new HashSet<int>(nums);
            Dictionary<int, int> lengths = new Dictionary<int, int>();
            
            int longest = 1;
            foreach(int n in nums)
            {
                var curLen = this.GetLcRecursive(exists, lengths, n);
                if (curLen > longest) longest = curLen;
            }

            return longest;
        }

        private int GetLcRecursive(HashSet<int> exists, Dictionary<int, int> lengths, int val)
        {
            if (lengths.ContainsKey(val)) return lengths[val];
            
            if (exists.Contains(val - 1))
            {
                lengths[val] = this.GetLcRecursive(exists, lengths, val - 1) + 1;
                return lengths[val];
            }
            else
            {
                lengths[val] = 1;
                return 1;
            }
        }
    }
}