namespace LeetCode.Csharp
{
    using System;
    using System.Linq;
    using LeetCode.Csharp.Common;
    using LeetCode.Csharp.Solutions2;

    internal class Program
    {
        private class HeapNode : IHeapNode
        {
            private int val;

            public HeapNode(int val)
            {
                this.val = val;
            }

            public int GetValue()
            {
                return this.val;
            }
        }

        public static void Main(string[] args)
        {
            MaxHeap<HeapNode> h = new MaxHeap<HeapNode>(100);
            int[] array = new[] {-31, 4, 1, 5, 7, 2, 132, 78, 90, 13, 254, 7, 22};
            foreach (int i in array)
            {
                h.Insert(new HeapNode(i));
            }

            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine(h.GetMax().GetValue());
                h.RemoveMax();
            }
        }
    }
}