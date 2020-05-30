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
            if (matrix.Length == 0 || matrix[0].Length == 0) return new List<int>();

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

        public bool PossibleBipartition(int N, int[][] dislikes)
        {
            if (N <= 0) return true;
            
            int[] parts = new int[N+1];
            Queue<int> q = new Queue<int>();
            Dictionary<int, List<int>> dislikeMap = new Dictionary<int, List<int>>();
            foreach (int[] dislike in dislikes)
            {
                if (!dislikeMap.ContainsKey(dislike[0]))
                {
                    dislikeMap[dislike[0]] = new List<int>();
                }
                
                if (!dislikeMap.ContainsKey(dislike[1]))
                {
                    dislikeMap[dislike[1]] = new List<int>();
                }
                
                dislikeMap[dislike[0]].Add(dislike[1]);
                dislikeMap[dislike[1]].Add(dislike[0]);
            }

            for (int i = 1; i <= N; i++)
            {
                if (parts[i] == 0)
                {
                    parts[i] = -1;
                    q.Enqueue(i);
                    
                    while (q.Count > 0)
                    {
                        int part = q.Dequeue();
                        int next = parts[part] == 1 ? -1 : 1;
                        if (dislikeMap.ContainsKey(part))
                        {
                            foreach (int dpart in dislikeMap[part])
                            {
                                if (parts[dpart] == 0)
                                {
                                    parts[dpart] = next;
                                    q.Enqueue(dpart);
                                }
                                else if (parts[dpart] != next)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        public void Run()
        {
            string dl =
                "[[21,47],[4,41],[2,41],[36,42],[32,45],[26,28],[32,44],[5,41],[29,44],[10,46],[1,6],[7,42],[46,49],[17,46],[32,35],[11,48],[37,48],[37,43],[8,41],[16,22],[41,43],[11,27],[22,44],[22,28],[18,37],[5,11],[18,46],[22,48],[1,17],[2,32],[21,37],[7,22],[23,41],[30,39],[6,41],[10,22],[36,41],[22,25],[1,12],[2,11],[45,46],[2,22],[1,38],[47,50],[11,15],[2,37],[1,43],[30,45],[4,32],[28,37],[1,21],[23,37],[5,37],[29,40],[6,42],[3,11],[40,42],[26,49],[41,50],[13,41],[20,47],[15,26],[47,49],[5,30],[4,42],[10,30],[6,29],[20,42],[4,37],[28,42],[1,16],[8,32],[16,29],[31,47],[15,47],[1,5],[7,37],[14,47],[30,48],[1,10],[26,43],[15,46],[42,45],[18,42],[25,42],[38,41],[32,39],[6,30],[29,33],[34,37],[26,38],[3,22],[18,47],[42,48],[22,49],[26,34],[22,36],[29,36],[11,25],[41,44],[6,46],[13,22],[11,16],[10,37],[42,43],[12,32],[1,48],[26,40],[22,50],[17,26],[4,22],[11,14],[26,39],[7,11],[23,26],[1,20],[32,33],[30,33],[1,25],[2,30],[2,46],[26,45],[47,48],[5,29],[3,37],[22,34],[20,22],[9,47],[1,4],[36,46],[30,49],[1,9],[3,26],[25,41],[14,29],[1,35],[23,42],[21,32],[24,46],[3,32],[9,42],[33,37],[7,30],[29,45],[27,30],[1,7],[33,42],[17,47],[12,47],[19,41],[3,42],[24,26],[20,29],[11,23],[22,40],[9,37],[31,32],[23,46],[11,38],[27,29],[17,37],[23,30],[14,42],[28,30],[29,31],[1,8],[1,36],[42,50],[21,41],[11,18],[39,41],[32,34],[6,37],[30,38],[21,46],[16,37],[22,24],[17,32],[23,29],[3,30],[8,30],[41,48],[1,39],[8,47],[30,44],[9,46],[22,45],[7,26],[35,42],[1,27],[17,30],[20,46],[18,29],[3,29],[4,30],[3,46]]";
            
            
            Console.WriteLine(
                JsonConvert.SerializeObject(
                    this.PossibleBipartition(50, JsonConvert.DeserializeObject<int[][]>(dl))
                ));
        }
    }
}