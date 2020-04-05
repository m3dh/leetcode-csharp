namespace LeetCode.Csharp.Solutions2
{
    using System;
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