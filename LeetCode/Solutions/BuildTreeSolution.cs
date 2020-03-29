namespace LeetCode.Csharp.Solutions
{
    using System;
    using System.Collections.Generic;
    using LeetCode.Csharp.Common;

    public class BuildTreeSolution
    {
        // 106 - https://leetcode.com/problems/construct-binary-tree-from-inorder-and-postorder-traversal/
        public TreeNode BuildTree(int[] inorder, int[] postorder)
        {
            if (inorder.Length == 0) return null;
            
            // This solution is all about partitioning.
            return this.BuildTreeInner(new Stack<int>(postorder), inorder, 0, inorder.Length - 1);
        }

        private TreeNode BuildTreeInner(Stack<int> roots, int[] nodes, int l, int r)
        {
            int currRoot = roots.Pop();
            for (int i = l; i <= r; i++)
            {
                if (nodes[i] == currRoot)
                {
                    TreeNode node = new TreeNode(currRoot);

                    // Process right first because postOrder doest left hand first.
                    if (i < r) node.right = this.BuildTreeInner(roots, nodes, i + 1, r);
                    if (i > l) node.left = this.BuildTreeInner(roots, nodes, l, i - 1);

                    return node;
                }
            }
            
            throw new Exception("Unexpected input");
        }
    }
}