namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using LeetCode.Csharp.Common;
    using Newtonsoft.Json;

    public class Graphs
    {
        // https://leetcode.com/problems/course-schedule/
        public bool CanFinish(int numCourses, int[][] prerequisites)
        {
            HashSet<int> finished = new HashSet<int>();
            
            // A map of being depends on.
            List<int>[] deps = new List<int>[numCourses];
            
            // A map of depending counts.
            int [] depDgs = new int[numCourses];
            
            for (int i = 0; i < numCourses; i++)
            {
                deps[i] = new List<int>();
            }

            foreach (int[] prerequisite in prerequisites)
            {
                depDgs[prerequisite[0]]++;
                deps[prerequisite[1]].Add(prerequisite[0]);
            }

            Queue<int> q = new Queue<int>();

            for (int i = 0; i < numCourses; i++)
            {
                if (depDgs[i] == 0)
                {
                    finished.Add(i);
                    q.Enqueue(i);
                }
            }

            while (q.Count > 0)
            {
                int f = q.Dequeue();
                foreach (int i in deps[f])
                {
                    depDgs[i]--;
                    if (depDgs[i] == 0 && finished.Add(i))
                    {
                        q.Enqueue(i);
                    }
                }
            }

            return finished.Count == numCourses;
        }
    }
}