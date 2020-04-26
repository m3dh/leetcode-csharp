namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    public class Search
    {
        // 332 - https://leetcode.com/problems/reconstruct-itinerary/
        public IList<string> FindItinerary(IList<IList<string>> tickets)
        {
            for (int i = 0; i < tickets.Count; i++)
            {
                tickets[i].Add(i.ToString());
            }

            Dictionary<string, List<IList<string>>> nodes = tickets
                .GroupBy(t => t[0])
                .ToDictionary(g => g.Key, g => g.OrderBy(t => t[1]).ToList());

            HashSet<string> visited = new HashSet<string>();

            List<string> route = new List<string>();
            this.FindItinerarySearch(nodes, visited, tickets.Count, "JFK", route);
            route.Reverse();
            return route;
        }

        private bool FindItinerarySearch(
            Dictionary<string, List<IList<string>>> nodes,
            HashSet<string> visited,
            int edgeToVisit,
            string currNode,
            List<string> route)
        {
            if (visited.Count == edgeToVisit)
            {
                // Have visited all edges.
                route.Add(currNode);
                return true;
            }

            foreach (IList<string> ticket in nodes[currNode])
            {
                string target = ticket[1];
                string tktKey = ticket[2];
                if (!visited.Contains(tktKey))
                {
                    visited.Add(tktKey);

                    if (this.FindItinerarySearch(nodes, visited, edgeToVisit, target, route))
                    {
                        route.Add(currNode);
                        return true;
                    }

                    visited.Remove(tktKey);
                }
            }

            return false;
        }

        // 39 - https://leetcode.com/problems/combination-sum/
        public IList<IList<int>> CombinationSum(int[] candidates, int target)
        {
            List<IList<int>> ret = new List<IList<int>>();
            this.RecCombine(0, candidates, target, ret, new int[candidates.Length]);
            return ret;
        }

        private void RecCombine(int cIdx, int[] candidates, int target, List<IList<int>> result, int[] memo)
        {
            if (target == 0)
            {
                List<int> r = new List<int>();
                for (int i = 0; i < memo.Length; i++)
                {
                    for (int k = 0; k < memo[i]; k++)
                    {
                        r.Add(candidates[i]);
                    }
                }

                result.Add(r);
                return;
            }

            if (cIdx < candidates.Length)
            {
                for (int x = 0;; x++)
                {
                    if (candidates[cIdx] * x <= target)
                    {
                        memo[cIdx] = x;
                        this.RecCombine(cIdx + 1, candidates, target - candidates[cIdx] * x, result, memo);
                    }
                    else
                    {
                        break;
                    }
                }
                
                memo[cIdx] = 0;
            }
        }
        
        // 40 - https://leetcode.com/problems/combination-sum-ii/
        public IList<IList<int>> CombinationSum2(int[] candidates, int target)
        {
            List<IList<int>> ret = new List<IList<int>>();
            this.RecCombine2(0, candidates, target, ret, new Stack<int>(), new HashSet<string>());
            return ret;
        }

        private void RecCombine2(int cIdx, int[] candidates, int target, List<IList<int>> result, Stack<int> curr, HashSet<string> hs)
        {
            if (target == 0)
            {
                string k = string.Join(",", curr.OrderBy(i => i));
                if (!hs.Contains(k))
                {
                    hs.Add(k);
                    result.Add(new List<int>(curr));
                }
            }
            else
            {
                if (cIdx < candidates.Length)
                {
                    if (target >= candidates[cIdx])
                    {
                        curr.Push(candidates[cIdx]);
                        this.RecCombine2(cIdx + 1, candidates, target - candidates[cIdx], result, curr, hs);
                        curr.Pop();
                    }

                    this.RecCombine2(cIdx + 1, candidates, target, result, curr, hs);
                }
            }
        }

        public void Run()
        {
            Console.WriteLine(JsonConvert.SerializeObject(this.CombinationSum2(new[] {8, 7, 4, 3}, 11)));
        }
    }
}