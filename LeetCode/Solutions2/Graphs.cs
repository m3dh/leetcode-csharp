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
            int[] depDgs = new int[numCourses];

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

        public class Node
        {
            public int val;
            public IList<Node> neighbors;

            public Node()
            {
                val = 0;
                neighbors = new List<Node>();
            }

            public Node(int _val)
            {
                val = _val;
                neighbors = new List<Node>();
            }

            public Node(int _val, List<Node> _neighbors)
            {
                val = _val;
                neighbors = _neighbors;
            }
        }

        // https://leetcode.com/problems/clone-graph/
        public Node CloneGraph(Node node)
        {
            return CloneNode(node, new Dictionary<int, Node>());
        }

        private Node CloneNode(Node node, Dictionary<int, Node> nodes)
        {
            Node newNode = new Node(node.val);
            nodes.Add(node.val, newNode);
            foreach (Node neighbor in node.neighbors)
            {
                if (!nodes.TryGetValue(neighbor.val, out Node nNode))
                {
                    nNode = CloneNode(neighbor, nodes);
                }
                
                newNode.neighbors.Add(nNode);
            }

            return newNode;
        }

        // https://leetcode.com/problems/network-delay-time/
        public int NetworkDelayTime(int[][] times, int N, int K)
        {
            // REVIEW: SPFA
            Dictionary<int, List<int[]>> edges = times.GroupBy(t => t[0]).ToDictionary(g => g.Key, g => g.ToList());

            Queue<int> q = new Queue<int>();
            HashSet<int> qm = new HashSet<int>();

            int[] costs = new int[N + 1];
            for (int i = 1; i <= N; i++)
            {
                if (i != K)
                {
                    costs[i] = -1;
                }
            }
            
            q.Enqueue(K);
            qm.Add(K);
            while (q.Count > 0)
            {
                int f = q.Dequeue();
                qm.Remove(f);
                if (edges.ContainsKey(f))
                {
                    foreach (int[] edge in edges[f])
                    {
                        if (costs[edge[1]] > costs[f] + edge[2] || costs[edge[1]] < 0)
                        {
                            costs[edge[1]] = costs[f] + edge[2];
                            if (qm.Add(edge[1]))
                            {
                                q.Enqueue(edge[1]);
                            }
                        }
                    }
                }
            }

            int max = 0;
            foreach (int cost in costs)
            {
                if (cost < 0) return -1;
                else max = Math.Max(max, cost);
            }

            return max;
        }
    }
}