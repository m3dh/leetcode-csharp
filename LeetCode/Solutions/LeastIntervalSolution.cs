namespace LeetCode.Csharp.Solutions
{
    using System;
    using System.Linq;

    public class LeastIntervalSolution
    {
        // 621 - https://leetcode.com/problems/task-scheduler/
        public int LeastInterval(char[] tasks, int n) {
            int[] map = new int[26];
            foreach(char c in tasks)
            map[c - 'A']++;
            Array.Sort(map);
            int time = 0;
            while (map[25] > 0) {
                int i = 0;
                
                // Fill one interval every time.
                while (i <= n) {
                    if (map[25] == 0)
                        break;
                    if (i < 26 && map[25 - i] > 0)
                        map[25 - i]--;
                    time++;
                    i++;
                }
                
                Array.Sort(map);
            }
            return time;
        }
        
        // This is incorrect!
        // Using batching fill will result to non-filled gaps.
        public int LeastInterval2(char[] tasks, int n)
        {
            Console.WriteLine(tasks.Length);
            
            int[] counters = tasks
                .GroupBy(ch => ch)
                .Select(cg => cg.Count())
                .OrderByDescending(t => t)
                .ToArray();

            int time = 0;
            while(counters[0] > 0)
            {
                Console.WriteLine(">> " + string.Join(",", counters));
                
               // Pick the current largest task type and fill intervals.
               int large = counters[0];
               counters[0] = 0; // Used up.
               time += (large + (large - 1) * n);

               int leftSlots = (large - 1) * n;
               int filler = 1;
               
               Console.WriteLine($"PICK: {large}, SLOT: {leftSlots}, T: {time}");
               
               while (filler < counters.Length && counters[filler] > 0 && leftSlots > 0)
               {
                   int toFill = counters[filler];
                   int filled = Math.Min(counters[filler], Math.Min(large - 1, leftSlots));
                   counters[filler] -= filled;
                   leftSlots -= filled;

                   Console.WriteLine($"FILL: {toFill}, SELECT: {filled}, SLOT: {leftSlots}");
                   
                   filler++;
               }

               counters = counters.OrderByDescending(t => t).ToArray();
            }

            return time;
        }
    }
}