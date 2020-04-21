namespace LeetCode.Csharp.Common
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class ListNode
    {
        public int val;
        public ListNode next;

        public ListNode(int x)
        {
            val = x;
        }
    }

    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;

        public TreeNode(int x)
        {
            val = x;
        }
    }

    public class BinaryMatrix
    {
        public int[][] _mtx;
        
        public BinaryMatrix(string mtx)
        {
            this._mtx = JsonConvert.DeserializeObject<int[][]>(mtx);
        }
        
        public int Get(int x, int y)
        {
            if (x >= this._mtx.Length || y >= this._mtx[0].Length)
            {
                throw new ArgumentException($"x:{x}, y:{y}");
            }
            
            return this._mtx[x][y];
        }

        public IList<int> Dimensions()
        {
            return new[] {this._mtx.Length, this._mtx[0].Length};
        }
    }
}
