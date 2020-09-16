namespace LeetCode.Csharp.Solutions
{
    using System;

    public class LeastIntervalSolution
    {
        // 621 - https://leetcode.com/problems/task-scheduler/
        public int LeastInterval(char[] tasks, int n) {
            // 这尼玛竟然是 O(N_all) 复杂度的，因为sort of 26 items被视作常量...
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
    }
}