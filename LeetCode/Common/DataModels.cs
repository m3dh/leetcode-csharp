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

    public class MyQueue
    {

        // reverse
        private Stack<int> _stack1 = new Stack<int>();

        // normal
        private Stack<int> _stack2 = new Stack<int>();

        /** Initialize your data structure here. */
        public MyQueue()
        {

        }

        /** Push element x to the back of queue. */
        public void Push(int x)
        {
            this._stack1.Push(x);
        }

        /** Removes the element from in front of queue and returns that element. */
        public int Pop()
        {
            if (this._stack2.Count == 0)
            {
                // dump all items in stack1 in a reverse order.
                // before we hit an empty stack2, don't dump again (otherwise it'll break the order)
                while (this._stack1.Count > 0)
                {
                    this._stack2.Push(this._stack1.Pop());
                }
            }

            return this._stack2.Pop();
        }

        /** Get the front element. */
        public int Peek()
        {
            if (this._stack2.Count == 0)
            {
                // dump all items in stack1 in a reverse order.
                // before we hit an empty stack2, don't dump again (otherwise it'll break the order)
                while (this._stack1.Count > 0)
                {
                    this._stack2.Push(this._stack1.Pop());
                }
            }

            return this._stack2.Peek();
        }

        /** Returns whether the queue is empty. */
        public bool Empty()
        {
            return this._stack1.Count + this._stack2.Count == 0;
        }
    }

    public class MyStack {

        // reverse
        private Queue<int> q = new Queue<int>();

        
        /** Initialize your data structure here. */
        public MyStack() {
        
        }
    
        /** Push element x onto stack. */
        public void Push(int x)
        {
            int len = this.q.Count;
            this.q.Enqueue(x);
            for (int i = 0; i < len; i++) this.q.Enqueue(this.q.Dequeue());
        }

        /** Removes the element on top of the stack and returns that element. */
        public int Pop()
        {
            return this.q.Dequeue();
        }
    
        /** Get the top element. */
        public int Top()
        {
            return this.q.Peek();
        }
    
        /** Returns whether the stack is empty. */
        public bool Empty() {
            return this.q.Count == 0;
        }
    }
    
    // https://leetcode.com/problems/maximum-frequency-stack/
    public class FreqStack
    {
        // x -> freq
        Dictionary<int, int> freqMap = new Dictionary<int, int>();
        
        // freq -> x's in that freq
        Dictionary<int, Stack<int>> stacks = new Dictionary<int, Stack<int>>();

        private int maxFreq = 0;
        
        public FreqStack()
        {

        }

        public void Push(int x)
        {
            if (this.freqMap.TryGetValue(x, out int freq))
            {
                freq++;
                this.freqMap[x] = freq;
            }
            else
            {
                freq = 1;
                this.freqMap[x] = freq;
            }

            if (freq > this.maxFreq) this.maxFreq = freq;

            if (this.stacks.TryGetValue(freq, out Stack<int> s))
            {
                s.Push(x);
            }
            else
            {
                s = new Stack<int>();
                s.Push(x);
                this.stacks[freq] = s;
            }
        }

        public int Pop()
        {
            while (this.stacks[this.maxFreq].Count == 0)
            {
                this.maxFreq--;
            }

            int ret = this.stacks[this.maxFreq].Pop();
            this.freqMap[ret]--;
            return ret;
        }
    }

    public class FirstUnique
    {
        Dictionary<int, Item> allItems = new Dictionary<int, Item>();
        LinkedList<int> uniqueItems = new LinkedList<int>();

        public FirstUnique(int[] nums)
        {
            foreach (int num in nums)
            {
                Add(num);
            }
        }

        public int ShowFirstUnique()
        {
            if (uniqueItems.Count > 0)
            {
                return uniqueItems.First.Value;
            }
            else
            {
                return -1;
            }
        }

        public void Add(int value)
        {
            if (allItems.TryGetValue(value, out Item item))
            {
                if (item.Count == 1)
                {
                    uniqueItems.Remove(item.Node);
                    item.Node = null;
                }
                
                item.Count++;
            }
            else
            {
                item = new Item
                {
                    Value = value,
                    Count = 1,
                    Node = new LinkedListNode<int>(value),
                };

                uniqueItems.AddLast(item.Node);
                this.allItems.Add(value, item);
            }
        }

        class Item
        {
            public int Value { get; set; }
            public int Count { get; set; }
            public LinkedListNode<int> Node { get; set; }
        }
    }
}
