namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using LeetCode.Csharp.Common;

    public class Trees
    {
        // 987 - https://leetcode.com/problems/vertical-order-traversal-of-a-binary-tree/
        public IList<IList<int>> VerticalTraversal(TreeNode root)
        {
            Dictionary<int, IList<Tuple<int, int>>> store = new Dictionary<int, IList<Tuple<int, int>>>();
            this.Lc987Recursion(root, 0, 0, store);
            List<IList<int>> ret = new List<IList<int>>();
            foreach (int x in store.Keys.OrderBy(k => k))
            {
                ret.Add(store[x].OrderByDescending(t => t.Item1).ThenBy(t => t.Item2).Select(t => t.Item2).ToArray());
            }

            return ret;
        }

        private void Lc987Recursion(TreeNode root, int x, int y, Dictionary<int, IList<Tuple<int, int>>> store)
        {
            if (root == null) return;
            if (store.TryGetValue(x, out IList<Tuple<int, int>> nodes)) nodes.Add(Tuple.Create(y, root.val));
            else store[x] = new List<Tuple<int, int>> { Tuple.Create(y, root.val) };
            this.Lc987Recursion(root.left, x - 1, y - 1, store);
            this.Lc987Recursion(root.right, x + 1, y - 1, store);
        }
    }
}