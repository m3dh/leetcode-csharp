namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Collections.Generic;

    public class Queues
    {
        // https://leetcode.com/problems/longest-continuous-subarray-with-absolute-diff-less-than-or-equal-to-limit/
        public int LongestSubarray(int[] nums, int limit)
        {
            // a decreasing queue
            LinkedList<int> maxQueue = new LinkedList<int>();

            // a increasing queue
            LinkedList<int> minQueue = new LinkedList<int>();

            int l = 0;
            int r = 0;
            int min = 0;
            int max = 0;
            int maxLen = 0;
            while (l < nums.Length)
            {
                if (max - min <= limit)
                {
                    maxLen = Math.Max(r - l, maxLen);

                    if (r < nums.Length)
                    {
                        while (maxQueue.Count > 0 && nums[maxQueue.Last.Value] < nums[r])
                        {
                            maxQueue.RemoveLast();
                        }

                        maxQueue.AddLast(r);
                        max = nums[maxQueue.First.Value];

                        while (minQueue.Count > 0 && nums[minQueue.Last.Value] > nums[r])
                        {
                            minQueue.RemoveLast();
                        }

                        minQueue.AddLast(r);
                        min = nums[minQueue.First.Value];

                        r++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (l == maxQueue.First.Value)
                    {
                        maxQueue.RemoveFirst();
                        max = maxQueue.Count == 0 ? 0 : nums[maxQueue.First.Value];
                    }

                    if (l == minQueue.First.Value)
                    {
                        minQueue.RemoveFirst();
                        min = minQueue.Count == 0 ? 0 : nums[minQueue.First.Value];
                    }

                    l++;
                }
            }

            return maxLen;
        }
    }
}
