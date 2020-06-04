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

        // BD-2-1
        public IList<IList<int>> BD21_Traversal(TreeNode root)
        {
            List<IList<int>> ret = new List<IList<int>>();
            bool directFromLeft = false;
            List<TreeNode> roots = new List<TreeNode> {root};
            while (roots.Count > 0)
            {
                List<int> level = new List<int>();
                List<TreeNode> newRoots = new List<TreeNode>();
                for (int i = 0; i < roots.Count; i++)
                {
                    level.Add(roots[directFromLeft ? i : roots.Count - 1 - i].val);

                    TreeNode node = roots[i];
                    if (node.left != null) newRoots.Add(node.left);
                    if (node.right != null) newRoots.Add(node.right);
                }

                roots = newRoots;
                directFromLeft = !directFromLeft;
                ret.Add(level);
            }

            return ret;
        }

        private void Lc987Recursion(TreeNode root, int x, int y, Dictionary<int, IList<Tuple<int, int>>> store)
        {
            if (root == null) return;
            if (store.TryGetValue(x, out IList<Tuple<int, int>> nodes)) nodes.Add(Tuple.Create(y, root.val));
            else store[x] = new List<Tuple<int, int>> {Tuple.Create(y, root.val)};
            this.Lc987Recursion(root.left, x - 1, y - 1, store);
            this.Lc987Recursion(root.right, x + 1, y - 1, store);
        }

        public bool IsValidSequence(TreeNode root, int[] arr)
        {
            return IsValidSequenceSearch(root, arr, 0);
        }

        private bool IsValidSequenceSearch(TreeNode root, int[] arr, int idx)
        {
            if (root == null || root.val != arr[idx])
            {
                return false;
            }
            else
            {
                // root.val == arr[idx]
                if (arr.Length - 1 == idx)
                {
                    return root.left == null && root.right == null;
                }
                else
                {
                    return IsValidSequenceSearch(root.left, arr, idx + 1) ||
                           IsValidSequenceSearch(root.right, arr, idx + 1);
                }
            }
        }

        // REVIEW: Morris traversal
        // https://leetcode.com/problems/binary-tree-inorder-traversal/
        public IList<int> InorderTraversal(TreeNode root)
        {
            List<int> ret = new List<int>();
            while (root != null)
            {
                if (root.left == null)
                {
                    // either left is really null or we have already visited left side.
                    ret.Add(root.val);
                    root = root.right;
                }
                else
                {
                    // find the predecessor of root node.
                    TreeNode node = root.left;
                    while (node.right != null)
                    {
                        node = node.right;
                    }

                    // link predecessor & curr-node.
                    node.right = root;

                    // remove link to left to avoid duplications.
                    TreeNode next = root.left;
                    root.left = null;
                    root = next;
                }
            }

            return ret;
        }
        
        
        // https://leetcode.com/problems/count-of-smaller-numbers-after-self/
        public IList<int> CountSmaller(int[] nums)
        {
            BoundBinaryTree bbt = new BoundBinaryTree();
            int[] ret = new int[nums.Length];
            for (int i = nums.Length - 1; i >= 0; i--)
            {
                bbt.Add(nums[i]);
                ret[i] = bbt.CountSmallerThan(nums[i]);
            }

            return ret;
        }

        public int CountRangeSum(int[] nums, int lower, int upper)
        {
            long sum = 0;
            int cnt = 0;
            
            // Find greater or equal than.
            BoundBinaryTree lowerBounder = new BoundBinaryTree();
            lowerBounder.Add(0);
            
            // Find smaller or equal than.
            BoundBinaryTree upperBounder = new BoundBinaryTree();
            upperBounder.Add(0);
            
            foreach (int num in nums)
            {
                sum += num;
                
                // sum - ? >= lower  ->  -? >= lower - sum
                // sum - ? <= upper  ->  -? <= upper - sum   ->    ?  >=  sum - upper
                
                // Find how many sums have been found between 
                int greaterOrEqualThanLowerCnt = lowerBounder.Count - lowerBounder.CountSmallerThan(lower - sum);
                int lessOrEqualThanUpperCnt = upperBounder.Count - upperBounder.CountSmallerThan(sum - upper);

                if (greaterOrEqualThanLowerCnt + lessOrEqualThanUpperCnt > lowerBounder.Count)
                {
                    cnt += greaterOrEqualThanLowerCnt + lessOrEqualThanUpperCnt - lowerBounder.Count;
                }
                
                lowerBounder.Add(-sum);
                upperBounder.Add(sum);
            }

            return cnt;
        }

        public void Run()
        {
            Console.WriteLine(CountRangeSum(new []{-2147483647,0,-2147483647,2147483647}, -5, 64));
        }
    }
}