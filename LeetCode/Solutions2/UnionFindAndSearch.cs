namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class UnionFindAndSearch
    {
        class UnionFindSet
        {
            private readonly int[] _set;

            public UnionFindSet(int size)
            {
                this._set = new int[size + 1];
                for (int i = 0; i <= size; i++) this._set[i] = i;
            }

            public int Find(int node)
            {
                // Find root.
                while (node != this._set[node])
                {
                    int newNode = this._set[node];
                    this._set[node] = this._set[newNode];
                    node = newNode;
                }

                return node;
            }

            public bool Union(int na, int nb)
            {
                int ra = this.Find(na);
                int rb = this.Find(nb);

                Console.WriteLine($"{na},{nb} - {ra},{rb}");

                if (ra == rb) return false;
                else
                {
                    this._set[ra] = rb;
                    Console.WriteLine(string.Join(", ", this._set));
                    return true;
                }
            }
        }

        // 684 - https://leetcode.com/problems/redundant-connection/
        public int[] FindRedundantConnection(int[][] edges)
        {
            // REVIEW: 并查集基础
            if (edges.Length > 0)
            {
                UnionFindSet ufs = new UnionFindSet(edges.Length);
                foreach (int[] edge in edges)
                {
                    if (!ufs.Union(edge[0], edge[1]))
                    {
                        return edge;
                    }
                }
            }

            return null;
        }

        // https://leetcode.com/problems/satisfiability-of-equality-equations/
        public bool EquationsPossible(string[] equations)
        {
            int[] set = new int[26];
            for (int i = 0; i < 26; i++) set[i] = i;
            foreach (string equation in equations)
            {
                char l = equation[0];
                char r = equation[3];
                bool e = equation[1] == '=';

                if(e) Union(set, l, r);
            }

            foreach (string equation in equations)
            {
                char l = equation[0];
                char r = equation[3];
                bool e = equation[1] == '=';

                if (!e)
                {
                    int il = Find(set, l);
                    int ir = Find(set, r);
                    if (il == ir) return false;
                }
            }

            return true;
        }

        private int Find(int[] set, int val)
        {
            while (set[val] != val)
            {
                int nVal = set[val];
                set[val] = set[nVal];
                val = nVal;
            }

            return val;
        }

        private bool Union(int[]set, int l, int r)
        {
            int il = Find(set, l);
            int ir = Find(set, r);
            if (il == ir) return false;
            else
            {
                set[ir] = il;
                return true;
            }
        }

        // https://leetcode.com/problems/most-stones-removed-with-same-row-or-column/
        public int RemoveStones(int[][] stones)
        {
            int[] ufs = new int[20005];
            for (int i = 0; i < 20005; i++) ufs[i] = i;
            foreach (int[] stone in stones)
            {
                Union(ufs, stone[0], 10000 + stone[1]);
            }

            HashSet<int> roots = new HashSet<int>();
            foreach (int[] stone in stones)
            {
                roots.Add(Find(ufs, stone[0]));
                roots.Add(Find(ufs, 10000 + stone[1]));
            }

            return stones.Length - roots.Count;
        }

        public void Run() {
            Console.WriteLine(
                JsonConvert.SerializeObject(
                    this.FindRedundantConnection(new int[][]
                    {
                        new int[] {1, 2},
                        new int[] {2, 3},
                        new int[] {3, 4},
                        new int[] {1, 4},
                        new int[] {1, 5}
                    })));

            Console.WriteLine(
                JsonConvert.SerializeObject(
                    this.FindRedundantConnection(new int[][]
                    {
                        new int[] {1, 5},
                        new int[] {3, 4},
                        new int[] {3, 5},
                        new int[] {4, 5},
                        new int[] {2, 4}
                    })));
        }
    }
}