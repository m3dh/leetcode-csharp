namespace LeetCode.Csharp.Solutions
{
    using System.Collections.Generic;
    using LeetCode.Csharp.Common;

    public class TreeTraversalSolution
    {
         // https://leetcode.com/problems/binary-tree-postorder-traversal/
        public IList<int> PostorderTraversal(TreeNode root)
        {
            // Let's use two stacks!
            List<int> ret = new List<int>();

            if (root != null)
            {
                Stack<TreeNode> visiting = new Stack<TreeNode>();
                visiting.Push(root);

                while (visiting.Count > 0)
                {
                    var curNode = visiting.Pop();

                    if (curNode.left == null && curNode.right == null)
                    {
                        ret.Add(curNode.val);
                    }
                    else if (curNode.left != null)
                    {
                        var leftNode = curNode.left;
                        curNode.left = null;
                        visiting.Push(curNode);
                        visiting.Push(leftNode);
                    }
                    else // if(curNode.right != null)
                    {
                        var rightNode = curNode.right;
                        curNode.right = null;
                        visiting.Push(curNode);
                        visiting.Push(rightNode);
                    }
                }
            }

            return ret;
        }

        // https://leetcode.com/problems/binary-tree-inorder-traversal/
        public IList<int> InorderTraversal(TreeNode root)
        {
            List<int> ret = new List<int>();

            if (root != null)
            {
                Stack<TreeNode> visiting = new Stack<TreeNode>();
                visiting.Push(root);

                while (visiting.Count > 0)
                {
                    var curNode = visiting.Pop();

                    if (curNode.right != null)
                    {
                        visiting.Push(curNode.right);
                        curNode.right = null;
                    }
                    
                    if (curNode.left != null)
                    {
                        var leftNode = curNode.left;
                        curNode.left = null;
                        visiting.Push(curNode);
                        visiting.Push(leftNode);
                    }
                    else
                    {
                        ret.Add(curNode.val);
                    }
                }
            }

            return ret;
        }

        // https://leetcode.com/problems/binary-tree-preorder-traversal/
        public IList<int> PreorderTraversal(TreeNode root)
        {
            List<int> ret = new List<int>();

            if (root != null)
            {
                Stack<TreeNode> visiting = new Stack<TreeNode>();
                visiting.Push(root);

                while (visiting.Count > 0)
                {
                    var curNode = visiting.Pop();
                    ret.Add(curNode.val);

                    if (curNode.right != null) visiting.Push(curNode.right);
                    if (curNode.left != null) visiting.Push(curNode.left);
                }
            }

            return ret;
        }
    }
}