namespace LeetCode.Csharp.Solutions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class IsNStraightHandSolution
    {
        // 846 - https://leetcode.com/problems/hand-of-straights/
        public bool IsNStraightHand(int[] hand, int W)
        {
            if (hand.Length == 0 || hand.Length % W != 0) return false;
            int groups = hand.Length / W;
            
            SortedDictionary<int, int> counters = new SortedDictionary<int, int>();
            foreach (int h in hand)
            {
                if (counters.ContainsKey(h)) counters[h]++;
                else counters[h] = 1;
            }

            for (int i = 0; i < groups; i++)
            {
                int pos = counters.Keys.First();
                for (int j = 0; j < W; j++)
                {
                    if (counters.TryGetValue(pos, out int cnt) && cnt > 0)
                    {
                        Console.WriteLine($"Removing {pos} ({cnt-1})");
                        if (cnt > 1) counters[pos]--;
                        else counters.Remove(pos);
                    }
                    else
                    {
                        Console.WriteLine($"Cannot remove {pos} ({cnt})");
                        return false;
                    }
                    
                    pos++;
                }
                
                Console.WriteLine("Completed once");
            }

            return true;
        }
    }
}