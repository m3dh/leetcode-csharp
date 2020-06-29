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

        // https://leetcode.com/problems/binary-tree-inorder-traversal/
        public IList<int> InorderTraversal(TreeNode root)
        {
            // REVIEW: Morris traversal

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
        
        // https://leetcode.com/problems/recover-binary-search-tree/
        public void RecoverTree(TreeNode root)
        {
            // REVIEW: Morris traversal -> find the first pair of (bigger, smaller) to get left, and last pair of (bigger, smaller) to get right.
            // REVIEW: 修改版Morris (还原树）
            // example: 1, 6, 4, 2, 7 

            TreeNode prev = null;
            TreeNode first = null;
            TreeNode second = null;

            while (root != null)
            {
                if (root.left != null)
                {
                    // find the predecessor of root node.
                    TreeNode node = root.left;
                    while (node.right != null && node.right.val != root.val)
                    {
                        node = node.right;
                    }

                    // 一种不需要干掉left指针的方法：当重复找到同一个left子树的时候，重置right指针；
                    if (node.right != null)
                    {
                        node.right = null;
                    }
                    else
                    {
                        // link predecessor & curr-node.
                        node.right = root;
                        root = root.left;
                        continue;
                    }
                }

                // either left is really null or we have already visited left side.
                // OUTPUT: ret.Add(root.val);
                if (prev != null && prev.val > root.val)
                {
                    if (first == null)
                    {
                        first = prev;
                    }

                    second = root;
                }

                prev = root;
                root = root.right;
            }

            if (first != null)
            {
                int tmp = first.val;
                first.val = second.val;
                second.val = tmp;
            }
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

        // https://leetcode.com/problems/path-sum-ii/
        public IList<IList<int>> PathSum2(TreeNode root, int sum)
        {
            IList<IList<int>> result = new List<IList<int>>();
            this.PathSum2(root, sum, new List<int>(), 0, result);
            return result;
        }

        private void PathSum2(TreeNode root, int sum, List<int> path, int pLen, IList<IList<int>> result)
        {
            if (root == null) return;

            if (path.Count <= pLen)
            {
                path.Add(root.val);
            }
            else
            {
                path[pLen] = root.val;
            }
            
            if (root.left == null && root.right == null)
            {
                if (sum - root.val == 0)
                {
                    result.Add(path.Take(pLen + 1).ToArray());
                }
            }
            else
            {
                this.PathSum2(root.left, sum - root.val, path, pLen + 1, result);
                this.PathSum2(root.right, sum - root.val, path, pLen + 1, result);
            }
        }
        
        // https://leetcode.com/problems/path-sum-iii/
        public int PathSum(TreeNode root, int sum)
        {
            int result = 0;
            this.PathSum(root, sum, 0, new List<int>(), 0, ref result);
            return result;
        }

        private void PathSum(TreeNode root, int sum, int cur, List<int> path, int pLen, ref int result)
        {
            if (root == null) return;

            if (path.Count <= pLen)
            {
                path.Add(root.val);
            }
            else
            {
                path[pLen] = root.val;
            }

            pLen++;
            cur += root.val;

            int t = cur;
            if (t == sum) result++;
            for (int i = 0; i < pLen - 1; i++)
            {
                t -= path[i];
                if (t == sum) result++;
            }
            
            this.PathSum(root.left, sum, cur, path, pLen + 1, ref result);
            this.PathSum(root.right, sum, cur, path, pLen + 1, ref result);
        }

        // https://leetcode.com/problems/validate-binary-search-tree/
        public bool IsValidBST(TreeNode root)
        {
            // use int? to handle corner cases...
            return root == null || (IsValidBstRec(root.left, null, root.val) && IsValidBstRec(root.right, root.val, null));
        }

        public bool IsValidBstRec(TreeNode root, int? curMin, int? curMax)
        {
            if (root == null)
            {
                return true;
            }
            else if ( (curMin != null && root.val <= curMin.Value) || (curMax != null && root.val >= curMax.Value))
            {
                return false;
            }
            else
            {
                bool ret = IsValidBstRec(root.left, curMin, root.val) && IsValidBstRec(root.right, root.val, curMax);
                return ret;
            }
        }

        // https://leetcode.com/problems/find-mode-in-binary-search-tree/
        public int[] FindMode(TreeNode root)
        {
            // In-order traversal
            List<int> modes = new List<int>();
            int pre = -1;
            int len = 0;
            int mLen = 0;
            FindModeRec(root, ref pre, ref len, ref mLen, modes);
            return modes.ToArray();
        }

        public void FindModeRec(TreeNode root, ref int pre, ref int len, ref int mLen, List<int> modes)
        {
            if (root == null) return;

            if (root.left != null) FindModeRec(root.left, ref pre, ref len, ref mLen, modes);

            if (len == 0 || pre != root.val)
            {
                len = 1;
                pre = root.val;
            }
            else if (pre == root.val)
            {
                len++;
            }

            if (len > mLen)
            {
                modes.Clear();
                modes.Add(root.val);
                mLen = len;
            }
            else if (len == mLen)
            {
                modes.Add(root.val);
            }

            if (root.right != null) FindModeRec(root.right, ref pre, ref len, ref mLen, modes);
        }

        // https://leetcode.com/problems/serialize-and-deserialize-binary-tree/
        public class Codec
        {
            public string serialize(TreeNode root)
            {
                List<int?> buffer = new List<int?>();
                Serialize(root, buffer);
                return string.Join("|", buffer.Select(item => item == null ? "n" : item.Value.ToString()));
            }

            public TreeNode deserialize(string data)
            {
                List<int?> buffer = data.Split("|").Select(item => item == "n" ? null : (int?) int.Parse(item)).ToList();
                int idx = 0;
                return Deserialize(ref idx, buffer);
            }

            private void Serialize(TreeNode root, List<int?> buffer)
            {
                if (root == null)
                {
                    buffer.Add(null);
                }
                else
                {
                    buffer.Add(root.val);
                    Serialize(root.left, buffer);
                    Serialize(root.right, buffer);
                }
            }

            private TreeNode Deserialize(ref int idx, List<int?> buffer)
            {
                if (buffer[idx] == null)
                {
                    idx++;
                    return null;
                }
                else
                {
                    TreeNode n = new TreeNode(buffer[idx].Value);
                    idx++;
                    n.left = Deserialize(ref idx, buffer);
                    n.right = Deserialize(ref idx, buffer);
                    return n;
                }
            }
        }

        public void Run()
        {
            Console.WriteLine(CountRangeSum(new []{-2147483647,0,-2147483647,2147483647}, -5, 64));
        }
    }
}