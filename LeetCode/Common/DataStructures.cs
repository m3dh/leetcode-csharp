namespace LeetCode.Csharp.Common
{
    using System;
    using System.Linq;

    public interface IHeapNode
    {
        int GetValue();
    }

    public class MaxHeap<T> where T : IHeapNode
    {
        private int _size;
        private readonly T[] _nodes;

        public MaxHeap(int maxSize)
        {
            this._size = 0;
            this._nodes = new T[maxSize];
        }

        public T[] Nodes => this._nodes.Take(this._size).ToArray();

        public int Count => this._size;

        public T GetMax()
        {
            if (this._size == 0) throw new Exception("Empty heap");
            return this._nodes[0];
        }

        public void RemoveMax()
        {
            if (this._size == 0) throw new Exception("Empty heap");

            T t = this._nodes[this._size - 1];
            int newVal = t.GetValue();
            int index = 0;
            while (
                (GetLeftChild(index) < this._size && newVal < this._nodes[GetLeftChild(index)].GetValue())
                ||
                (GetRightChild(index) < this._size && newVal < this._nodes[GetRightChild(index)].GetValue())
            )
            {
                // Select between left and right.
                if (GetLeftChild(index) >= this._size)
                {
                    this._nodes[index] = this._nodes[GetRightChild(index)];
                    index = GetRightChild(index);
                }
                else if (GetRightChild(index) >= this._size)
                {
                    this._nodes[index] = this._nodes[GetLeftChild(index)];
                    index = GetLeftChild(index);
                }
                else if (this._nodes[GetRightChild(index)].GetValue() > this._nodes[GetLeftChild(index)].GetValue())
                {
                    this._nodes[index] = this._nodes[GetRightChild(index)];
                    index = GetRightChild(index);
                }
                else
                {
                    this._nodes[index] = this._nodes[GetLeftChild(index)];
                    index = GetLeftChild(index);
                }
            }

            this._nodes[index] = t;
            this._size--;
        }

        public void Insert(T t)
        {
            if (this._size >= this._nodes.Length) throw new Exception("Overflow");

            int newVal = t.GetValue();
            int index = this._size;
            while (index > 0 && newVal > this._nodes[GetParentIndex(index)].GetValue())
            {
                this._nodes[index] = this._nodes[GetParentIndex(index)];
                index = GetParentIndex(index);
            }

            this._nodes[index] = t;
            this._size++;
        }

        private static int GetParentIndex(int index)
        {
            return (index - 1) / 2;
        }

        private static int GetLeftChild(int index)
        {
            return index * 2 + 1;
        }

        private static int GetRightChild(int index)
        {
            return index * 2 + 2;
        }
    }

    public class BoundBinaryTree
    {
        public class TreeNode
        {
            public long Val { get; set; }
            public int Smaller { get; set; }

            public TreeNode Left;
            public TreeNode Right;
        }

        private TreeNode _root = null;
        public int Count { get; set; } = 0;

        public void Add(long val)
        {
            this.Count++;
            this.Add(ref this._root, val);
        }

        private void Add(ref TreeNode root, long val)
        {
            if (root == null)
            {
                root = new TreeNode {Val = val};
                return;
            }

            if (val < root.Val)
            {
                root.Smaller++;
                this.Add(ref root.Left, val);
            }
            else
            {
                this.Add(ref root.Right, val);
            }
        }

        public int CountSmallerThan(long val)
        {
            if (this._root == null) return 0;
            return this.CountSmallerThan(this._root, val);
        }

        private int CountSmallerThan(TreeNode root, long val)
        {
            if (root.Val < val)
            {
                return 1 + root.Smaller + (root.Right == null ? 0 : this.CountSmallerThan(root.Right, val));
            }
            else
            {
                return root.Left == null ? 0 : this.CountSmallerThan(root.Left, val);
            }
        }
    }
}