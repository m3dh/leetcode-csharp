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
        public TreeNode BuildTree2(int[] preorder, int[] inorder)
        {
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

        // 1008 - https://leetcode.com/problems/construct-binary-search-tree-from-preorder-traversal/
        public TreeNode BstFromPreorder(int[] preorder)
        {
            if (preorder.Length == 0) return null;
            TreeNode root = new TreeNode(preorder[0]);
            Stack<TreeNode> stack = new Stack<TreeNode>();
            stack.Push(root);

            for (int i = 1; i < preorder.Length; i++)
            {
                int node = preorder[i];
                TreeNode currNode = stack.Peek();
                while (stack.Count > 0 && stack.Peek().val < node) // This makes the sub-tree
                {
                    // So currNode is a node bigger than nodeVal, or node whose parent is bigger than nodeVal.
                    currNode = stack.Pop();
                }

                if (currNode.val > node)
                {
                    currNode.left = new TreeNode(node);
                    stack.Push(currNode.left);
                }
                else
                {
                    currNode.right = new TreeNode(node);
                    stack.Push(currNode.right);
                }
            }

            return root;
        }
    }
}