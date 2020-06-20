namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using LeetCode.Csharp.Common;
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
        
        // https://leetcode.com/problems/shortest-bridge/
        public int ShortestBridge(int[][] A)
        {
            // REVIEW: 要把第一个岛的全部点都加进去才能保证正确！
            int[][] dirs = new[] {new[] {1, 0}, new[] {-1, 0}, new[] {0, 1}, new[] {0, -1}};
            int[][] islandMap = new int[A.Length][];
            int[][] costMap = new int[A.Length][];
            for (int i = 0; i < A.Length; i++)
            {
                islandMap[i] = new int[A[i].Length];
                costMap[i] = new int[A[i].Length];
            }

            int islandId = 1;
            List<int[]> initial = new List<int[]>();
            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[i].Length; j++)
                {
                    // A new island.
                    if (A[i][j] == 1 && islandMap[i][j] == 0)
                    {
                        Queue<int[]> q = new Queue<int[]>();
                        q.Enqueue(new[] {i, j});
                        islandMap[i][j] = islandId;
                        while (q.Count > 0)
                        {
                            int[] pos = q.Dequeue();

                            if (islandId == 1)
                            {
                                initial.Add(pos);
                            }

                            foreach (int[] dir in dirs)
                            {
                                int ni = pos[0] + dir[0];
                                int nj = pos[1] + dir[1];

                                if (ni >= 0 && ni < A.Length && nj >= 0 && nj < A[ni].Length)
                                {
                                    if (islandMap[ni][nj] == 0 && A[ni][nj] == 1)
                                    {
                                        islandMap[ni][nj] = islandId;
                                        q.Enqueue(new[] {ni, nj});
                                    }
                                }
                            }
                        }

                        islandId++;
                    }
                }
            }

            if (islandId != 3) return -1;
            Queue<int[]> qBridge = new Queue<int[]>();

            foreach (int[] ipos in initial)
            {
                qBridge.Enqueue(ipos);
                costMap[ipos[0]][ipos[1]] = 1;
            }

            while (qBridge.Count > 0)
            {
                int[] pos = qBridge.Dequeue();

                foreach (int[] dir in dirs)
                {
                    int ni = pos[0] + dir[0];
                    int nj = pos[1] + dir[1];

                    if (ni >= 0 && ni < A.Length && nj >= 0 && nj < A[ni].Length)
                    {
                        if (costMap[ni][nj] == 0) // not visited
                        {
                            if (islandMap[ni][nj] == 1)
                            {
                                costMap[ni][nj] = 1;
                            }
                            else if (islandMap[ni][nj] == 0)
                            {
                                costMap[ni][nj] = costMap[pos[0]][pos[1]] + 1;
                            }
                            else
                            {
                                return costMap[pos[0]][pos[1]] - 1;
                            }

                            qBridge.Enqueue(new[] {ni, nj});
                        }
                    }
                }
            }

            return -1;
        }
        
        // https://leetcode.com/problems/similar-string-groups/
        public int NumSimilarGroups(string[] A)
        {
            // build the graph.
            Dictionary<int, List<int>> g = new Dictionary<int, List<int>>();
            for (int i1 = 0; i1 < A.Length; i1++)
            {
                List<int> l = new List<int>();
                g[i1] = l;
                for (int i2 = 0; i2 < A.Length; i2++)
                {
                    int distance = 0;
                    for (int i = 0; i < A[i1].Length; i++)
                    {
                        if (A[i1][i] != A[i2][i]) distance++;
                    }

                    if (distance == 2)
                    {
                        l.Add(i2);
                    }
                }
            }

            int cnt = 0;
            bool[] visited = new bool[A.Length];
            for (int i = 0; i < A.Length; i++)
            {
                if (!visited[i])
                {
                    cnt++;
                    Queue<int> q = new Queue<int>();
                    q.Enqueue(i);
                    visited[i] = true;
                    while (q.Count > 0)
                    {
                        int cur = q.Dequeue();
                        foreach (var t in g[cur])
                        {
                            if (!visited[t])
                            {
                                visited[t] = true;
                                q.Enqueue(t);
                            }
                        }
                    }
                 }
            }

            return cnt;
        }

        // https://leetcode.com/problems/word-ladder/
        public int LadderLength(string beginWord, string endWord, IList<string> wordList)
        {
            // REVIEW: The idea is to create a graph from generated a*c to abc, adc, aac...
            Dictionary<string, List<string>> g = new Dictionary<string, List<string>>();
            foreach (string s in wordList)
            {
                for (int l = 0; l < s.Length; l++)
                {
                    string wildCard = $"{s.Substring(0, l)}*{s.Substring(l+1)}";
                    if (!g.TryGetValue(wildCard, out List<string> t))
                    {
                        g.Add(wildCard, new List<string> {s});
                    }
                    else
                    {
                        t.Add(s);
                    }
                }
            }

            int step = 0;
            List<string> q = new List<string> {beginWord};
            HashSet<string> visited = new HashSet<string> {beginWord};
            while (q.Count > 0)
            {
                step++;
                List<string> n = new List<string>();
                foreach (string s in q)
                {
                    for (int l = 0; l < s.Length; l++)
                    {
                        string wildCard = $"{s.Substring(0, l)}*{s.Substring(l + 1)}";
                        if (g.TryGetValue(wildCard, out List<string> ts))
                        {
                            foreach (string t in ts)
                            {
                                if (string.Equals(endWord, t, StringComparison.Ordinal)) return step + 1;

                                if (!visited.Contains(t))
                                {
                                    visited.Add(t);
                                    n.Add(t);
                                }
                            }
                        }
                    }
                }

                q = n;
            }

            return 0;
        }
        
        // https://leetcode.com/problems/word-ladder-ii/
        public IList<IList<string>> FindLadders(string beginWord, string endWord, IList<string> wordList)
        {
            Dictionary<string, List<string>> g = new Dictionary<string, List<string>>();
            foreach (string s in wordList)
            {
                for (int l = 0; l < s.Length; l++)
                {
                    string wildCard = $"{s.Substring(0, l)}*{s.Substring(l+1)}";
                    if (!g.TryGetValue(wildCard, out List<string> t))
                    {
                        g.Add(wildCard, new List<string> {s});
                    }
                    else
                    {
                        t.Add(s);
                    }
                }
            }
            
            List<IList<string>> ret = new List<IList<string>>();
            Dictionary<string, List<string>> r = new Dictionary<string, List<string>> {[beginWord] = new List<string>()};
            
            List<string> q = new List<string> {beginWord};
            Dictionary<string, int> visited = new Dictionary<string, int> {[beginWord] = 0};

            int step = 1;
            while (q.Count > 0)
            {
                bool found = false;
                List<string> n = new List<string>();
                foreach (string s in q)
                {
                    for (int l = 0; l < s.Length; l++)
                    {
                        string wildCard = $"{s.Substring(0, l)}*{s.Substring(l + 1)}";
                        if (g.TryGetValue(wildCard, out List<string> ts))
                        {
                            foreach (string t in ts)
                            {
                                if (string.Equals(endWord, t, StringComparison.Ordinal))
                                {
                                    found = true;
                                }

                                if (!visited.TryGetValue(t, out int vStep) || vStep == step)
                                {
                                    // maintain 't' to 's'.
                                    if (r.TryGetValue(t, out List<string> from))
                                    {
                                        from.Add(s);
                                    }
                                    else
                                    {
                                        r.Add(t, new List<string> {s});
                                    }

                                    visited[t] = step;
                                    n.Add(t);
                                }
                            }
                        }
                    }
                }

                if (found)
                {
                    this.BuildBackTrace(endWord, r, ret, new string[step + 1], 0);
                    break;
                }

                step++;
                q = n;
            }

            return ret;
        }

        private void BuildBackTrace(string endWord, Dictionary<string, List<string>> r, List<IList<string>> ret, string[] tmp, int idx)
        {
            tmp[idx] = endWord;
            var back = r[endWord];
            if (back.Any())
            {
                foreach (var bs in back.Distinct())
                {
                    this.BuildBackTrace(bs, r, ret, tmp, idx + 1);
                }
            }
            else
            {
                ret.Add(tmp.Reverse().ToList());
            }
        }
        
        // https://leetcode.com/problems/palindrome-pairs/
        public IList<IList<int>> PalindromePairs(string[] words)
        {
            Dictionary<string, int> wordsRevMap = new Dictionary<string, int>();
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                char[] ca = word.ToCharArray();
                wordsRevMap.Add(new string(ca.Reverse().ToArray()), i);
            }
            
            List<IList<int>> ret = new List<IList<int>>();
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];

                // length starts from 0 - handle corner cases
                for (int l = 0; l <= word.Length; l++)
                {
                    // on left
                    if (this.IsPalindrome(word, l, word.Length - 1) && wordsRevMap.TryGetValue(word.Substring(0, l), out int idx))
                    {
                        if (idx != i)
                        {
                            ret.Add(new[] {i, idx});
                        }
                    }

                    // on right
                    if (l != word.Length && // if l == word.Length, test once is good enough... (idx,i) will test another direction.
                        this.IsPalindrome(word, 0, word.Length - 1 - l) && wordsRevMap.TryGetValue(word.Substring(word.Length - l), out idx))
                    {
                        if (idx != i)
                        {
                            ret.Add(new[] {idx, i});
                        }
                    }
                }
            }

            return ret;
        }

        private bool IsPalindrome(string s, int l, int r)
        {
            while (l < r)
            {
                if (s[l] != s[r]) return false;
                l++;
                r--;
            }

            return true;
        }
        
        // https://leetcode.com/problems/critical-connections-in-a-network/
        public IList<IList<int>> CriticalConnections(int n, IList<IList<int>> connections)
        {
            int[] visitOrder = new int[n]; // 0 - not visited.
            int[] lowerOrder = new int[n];
            Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();
            foreach (IList<int> connection in connections)
            {
                if (graph.TryGetValue(connection[0], out List<int> fl))
                {
                    fl.Add(connection[1]);
                }
                else
                {
                    graph[connection[0]] = new List<int> {connection[1]};
                }

                if (graph.TryGetValue(connection[1], out List<int> fr))
                {
                    fr.Add(connection[0]);
                }
                else
                {
                    graph[connection[1]] = new List<int> {connection[0]};
                }
            }
            
            List<IList<int>> ret = new List<IList<int>>();
            this.TarjanRec(1, graph, 0, -1, ret, visitOrder, lowerOrder);
            return ret;
        }

        private void TarjanRec(int cnt, Dictionary<int, List<int>> g, int cur, int par, List<IList<int>> ret, int[] visitOrder, int[] lowerOrder)
        {
            // 寻找不能到达小于自己数字（非父）的边
            if (lowerOrder[cur] == 0)
            {
                visitOrder[cur] = cnt;
                lowerOrder[cur] = cnt; // can reach myself.
                foreach (int target in g[cur])
                {
                    if (target == par) continue;
                    
                    this.TarjanRec(cnt + 1, g, target, cur, ret, visitOrder, lowerOrder);

                    if (lowerOrder[target] < lowerOrder[cur])
                    {
                        lowerOrder[cur] = lowerOrder[target];
                    }
                }

                if (lowerOrder[cur] == cnt && par >= 0)
                {
                    ret.Add(new[] {par, cur});
                }
            }
        }
        
        // https://leetcode.com/problems/spiral-matrix-ii/
        public int[][] GenerateMatrix(int n) {
            int[][] ret = new int[n][];
            for(int i=0;i<n;i++) ret[i] = new int[n];
        
            int[][] dirs = new int[][] { new int[] {0,1}, new int[]{1,0}, new int[]{0,-1}, new int[]{-1,0} };
        
            int val = 1;
            int dir = 0;
            int[] cur = new int[] {0,0};
            while(val <= n*n) {
                if(ret[cur[0]][cur[1]] == 0) {
                    ret[cur[0]][cur[1]] = val;
                    val++;
                }
            
                int nx = cur[0] + dirs[dir][0];
                int ny = cur[1] + dirs[dir][1];
                if(nx < 0 || nx >= n || ny < 0 || ny >= n || ret[nx][ny] > 0) {
                    dir = (dir + 1) % 4;
                } else {
                    cur[0] = nx;
                    cur[1] = ny;
                }
            }
        
            return ret;
        }

        public void Run()
        {
            Console.WriteLine(JsonConvert.SerializeObject(this.GenerateMatrix(4)));
        }
    }
}