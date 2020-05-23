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

        private void RecCombine2(int cIdx, int[] candidates, int target, List<IList<int>> result, Stack<int> curr,
            HashSet<string> hs)
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

        // https://leetcode.com/problems/valid-palindrome-ii/submissions/
        public bool ValidPalindrome(string s)
        {
            return ValidPalindrome(s, 0, s.Length - 1, true);
        }

        public bool ValidPalindrome(string s, int l, int r, bool canSkip)
        {
            while (l < r)
            {
                if (s[l] == s[r])
                {
                    l++;
                    r--;
                }
                else
                {
                    // REVIEW: Only try different approaches when != happens.
                    if (canSkip)
                    {
                        return ValidPalindrome(s, l + 1, r, false) || ValidPalindrome(s, l, r - 1, false);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // https://leetcode.com/problems/permutations-ii/
        public IList<IList<int>> PermuteUnique(int[] nums)
        {
            List<IList<int>> result = new List<IList<int>>();
            SearchPermute1(nums, new bool[nums.Length], 0, new int[nums.Length], result);
            return result;
        }

        public void SearchPermute1(int[] nums, bool[] visited, int idx, int[] curr, List<IList<int>> result)
        {
            if (idx == curr.Length)
            {
                result.Add(new List<int>(curr));
                return;
            }

            HashSet<int> everApp = new HashSet<int>();
            
            for (int i = 0; i < nums.Length; i++)
            {
                if (!visited[i] && everApp.Add(nums[i]))
                {
                    visited[i] = true;
                    curr[idx] = nums[i];
                    SearchPermute1(nums, visited, idx + 1, curr, result);
                    
                    visited[i] = false;
                }
            }
        }
        
        // https://leetcode.com/problems/spiral-matrix/
        public IList<int> SpiralOrder(int[][] matrix)
        {
            if(matrix.Length == 0 || matrix[0].Length    == 0) return new List<int>();
            
            List<int> ret = new List<int>();
            bool[][] visited = new bool[matrix.Length][];
            int[][] dirs = new[] {new[] {0, 1,}, new[] {1, 0}, new[] {0, -1}, new[] {-1, 0}};
            for (int i = 0; i < matrix.Length; i++) visited[i] = new bool[matrix[i].Length];
            int x = 0;
            int y = 0;
            int d = 0;
            int c = matrix.Length * matrix[0].Length;
            while (ret.Count < c)
            {
                if (!visited[x][y])
                {
                    ret.Add(matrix[x][y]);
                    visited[x][y] = true;
                }

                int nx = x + dirs[d][0];
                int ny = y + dirs[d][1];
                if (nx >= 0 && ny >= 0 && nx < matrix.Length && ny < matrix[nx].Length && !visited[nx][ny])
                {
                    x = nx;
                    y = ny;
                }
                else
                {
                    d = (d + 1) % 4;
                }
            }

            return ret;
        }

        public void Run()
        {
            Console.WriteLine(
                JsonConvert.SerializeObject(
                    this.PermuteUnique(new[] {1,1,2})
                    ));
        }
    }
}