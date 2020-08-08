namespace LeetCode.Csharp.Solutions2
{
    using System.Collections.Generic;
    using System.Linq;
    using LeetCode.Csharp.Common;

    public class Arrays2
    {
        // https://leetcode.com/problems/max-chunks-to-make-sorted/
        public int MaxChunksToSorted1(int[] arr)
        {
            int idx = 0;
            int cnt = 0;

            List<int> expecting = new List<int>();
            HashSet<int> found = new HashSet<int>();

            while (idx < arr.Length)
            {
                if (idx == arr[idx])
                {
                    idx++;
                    cnt++;
                }
                else
                {
                    while (idx < arr.Length)
                    {
                        expecting.Add(idx);
                        found.Add(arr[idx]);

                        if (expecting.All(e => found.Contains(e)))
                        {
                            cnt++;
                            idx++;
                            expecting.Clear();
                            found.Clear();
                            break;
                        }

                        idx++;
                    }
                }
            }

            return cnt;
        }

        // https://leetcode.com/problems/max-chunks-to-make-sorted-ii/
        public int MaxChunksToSorted(int[] arr)
        {
            // REVIEW: 有 O(n) 的解法，去看题解！

            var sorted = arr.OrderBy(a => a).ToArray();

            int cnt = 0;
            int delta = 0;
            Counter<int> cntr = new Counter<int>();
            for (int i = 0; i < arr.Length; i++)
            {
                int sc = cntr.Incr(sorted[i]);
                if (sc == 1)
                {
                    delta++;
                }
                else if (sc == 0)
                {
                    delta--;
                }

                int dc = cntr.Decr(arr[i]);
                if (dc == -1)
                {
                    delta++;
                }
                else if (dc == 0)
                {
                    delta--;
                }

                if (delta == 0)
                {
                    cnt++;
                }
            }

            return cnt;
        }

        // https://leetcode.com/problems/patching-array/
        public int MinPatches(int[] nums, int n)
        {
            // REVIEW: 记录miss，miss以下的数字都可获得！
            long miss = 1;
            int i = 0;
            int cnt = 0;
            while (miss <= n)
            {
                if (i < nums.Count() && nums[i] <= miss)
                {
                    miss += nums[i];
                    i++;
                }
                else
                {
                    cnt++;
                    miss += miss;
                }
            }

            return cnt;
        }
    }
}
