namespace LeetCode.Csharp.Solutions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        
        // 105 - https://leetcode.com/problems/construct-binary-tree-from-preorder-and-inorder-traversal/
        public TreeNode BuildTree2(int[] preorder, int[] inorder) {
            if (inorder.Length == 0) return null;
            
            // This solution is all about partitioning.
            return this.BuildTreeInner2(new Stack<int>(preorder.Reverse()), inorder, 0, inorder.Length - 1);
        }
        
        private TreeNode BuildTreeInner2(Stack<int> roots, int[] nodes, int l, int r)
        {
            int currRoot = roots.Pop();
            for (int i = l; i <= r; i++)
            {
                if (nodes[i] == currRoot)
                {
                    TreeNode node = new TreeNode(currRoot);

                    // Process left first because we're actually using a Queue of preOrder.
                    if (i > l) node.left = this.BuildTreeInner2(roots, nodes, l, i - 1);
                    if (i < r) node.right = this.BuildTreeInner2(roots, nodes, i + 1, r);

                    return node;
                }
            }
            
            throw new Exception("Unexpected input");
        }
    }
}