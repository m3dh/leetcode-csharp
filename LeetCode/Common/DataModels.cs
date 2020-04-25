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

    public class LRUCache
    {
        private readonly int _capacity;
        private readonly Dictionary<int, Tuple<int, LinkedListNode<int>>> _cache = new Dictionary<int, Tuple<int, LinkedListNode<int>>>();
        private readonly LinkedList<int> _cacheItems = new LinkedList<int>(); // keys

        public LRUCache(int capacity)
        {
            this._capacity = capacity;
        }

        public int Get(int key)
        {
            if (this._cache.TryGetValue(key, out Tuple<int, LinkedListNode<int>> val))
            {
                this._cacheItems.Remove(val.Item2);
                this._cacheItems.AddLast(val.Item2);
                return val.Item1;
            }
            else
            {
                return -1;
            }
        }

        public void Put(int key, int value)
        {
            if (this._cache.TryGetValue(key, out Tuple<int, LinkedListNode<int>> val))
            {
                this._cacheItems.Remove(val.Item2);
            }
            else if (this._cache.Count + 1 > this._capacity)
            {
                // evict
                var nodeToRemove = this._cacheItems.First;
                this._cacheItems.RemoveFirst();
                this._cache.Remove(nodeToRemove.Value);
            }

            LinkedListNode<int> node = new LinkedListNode<int>(key);
            this._cacheItems.AddLast(node);
            this._cache[key] = Tuple.Create<int, LinkedListNode<int>>(value, node);
        }
    }
}
