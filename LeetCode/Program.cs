namespace LeetCode.Csharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LeetCode.Csharp.Common;
    using LeetCode.Csharp.Solutions;
    using LeetCode.Csharp.Solutions2;
    using Newtonsoft.Json;

    internal class Program
    {
        public static void Main()
        {
            var obj = new Planning();
            obj.Run();
        }
        
        public class StoneNode : IHeapNode {
            private readonly int _weight;
        
            public StoneNode(int weight) {
                this._weight = weight;
            }
        
            public int GetValue() {
                return this._weight;
            }
        }
    
        public int LastStoneWeight(int[] stones) {
            if(stones.Length==0) return 0;
            MaxHeap<StoneNode> heap = new MaxHeap<StoneNode>(stones.Length);
            foreach(int stone in stones) heap.Insert(new StoneNode(stone));
            while (heap.Count > 1)
            {
                StoneNode max1 = heap.GetMax();
                heap.RemoveMax();

                StoneNode max2 = heap.GetMax();
                heap.RemoveMax();

                var newVal = max1.GetValue() - max2.GetValue();
                if (newVal > 0) heap.Insert(new StoneNode(newVal));
            }

            return heap.Count == 1 ? heap.GetMax().GetValue() : 0;
        }
    }
}