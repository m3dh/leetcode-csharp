namespace LeetCode.Csharp.Solutions2
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Search2
    {
        // https://leetcode.com/problems/partition-to-k-equal-sum-subsets/
        public bool CanPartitionKSubsets(int[] nums, int k)
        {
            if (nums.Count() == 0) return false;

            int sum = nums.Sum();
            if (sum % k != 0) return false;

            return CanPartition(nums, 0, 0, k, sum / k, new bool[nums.Length]);
        }

        private bool CanPartition(int[] nums, int start, int sum, int remain, int target, bool[] visited)
        {
            if (remain == 1)
            {
                // given all selected sums are target, no need to check last set.
                return true;
            }

            if (sum == target)
            {
                return CanPartition(nums, 0, 0, remain - 1, target, visited);
            }
            else
            {
                for (int i = start; i < nums.Length; i++)
                {
                    if (!visited[i])
                    {
                        visited[i] = true;
                        if (CanPartition(nums, i + 1, sum + nums[i], remain, target, visited))
                        {
                            return true;
                        }

                        visited[i] = false;
                    }
                }

                return false;
            }
        }

        // https://leetcode.com/problems/strobogrammatic-number-iii/
        public int StrobogrammaticInRange(string low, string high)
        {
            // REVIEW: 这可以用搜索做！
            return -1;
        }

        // https://leetcode.com/problems/the-most-similar-path-in-a-graph/
        public IList<int> MostSimilar(int n, int[][] roads, string[] names, string[] targetPath)
        {

        }
    }
}