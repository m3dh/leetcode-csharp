namespace LeetCode.Csharp.Solutions2
{
    using System.Collections.Generic;
    using System.Linq;

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

        }
    }
}
