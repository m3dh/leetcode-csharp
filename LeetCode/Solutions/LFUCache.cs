namespace LeetCode.Csharp.Solutions
{
    using System;
    using System.Collections.Generic;

    public class LFUCache
    {
        private readonly int _capacity;
        private int _count = 0;
        private int _minFreq = -1;

        private Dictionary<int, LinkedList<Tuple<int, int>>> _freqMap = new Dictionary<int, LinkedList<Tuple<int, int>>>
        {
            [1] = new LinkedList<Tuple<int, int>>()
        }; // Tuple<key, freq>
        private Dictionary<int, Tuple<int, LinkedListNode<Tuple<int, int>>>> _cache = new Dictionary<int, Tuple<int, LinkedListNode<Tuple<int, int>>>>(); // Tuple<val, node>

        public LFUCache(int capacity)
        {
            this._capacity = capacity;
        }

        public int Get(int key)
        {
            if (this._cache.TryGetValue(key, out Tuple<int, LinkedListNode<Tuple<int, int>>> val))
            {
                var node = val.Item2;
                var fl = this._freqMap[node.Value.Item2];
                fl.Remove(node);
                if (fl.Count == 0 && node.Value.Item2 == this._minFreq)
                {
                    this._minFreq++;
                }

                if (!this._freqMap.TryGetValue(node.Value.Item2 + 1, out LinkedList<Tuple<int, int>> ll))
                {
                    ll = new LinkedList<Tuple<int, int>>();
                    this._freqMap[node.Value.Item2 + 1] = ll;
                }

                var newNode = ll.AddLast(Tuple.Create(node.Value.Item1, node.Value.Item2 + 1));
                this._cache[key] = Tuple.Create(val.Item1, newNode);
                return val.Item1;
            }

            return -1;
        }

        public void Put(int key, int value)
        {
            if (this._cache.TryGetValue(key, out Tuple<int, LinkedListNode<Tuple<int, int>>> val))
            {
                var node = val.Item2;
                var fl = this._freqMap[node.Value.Item2];
                fl.Remove(node);
                if (fl.Count == 0 && node.Value.Item2 == this._minFreq)
                {
                    this._minFreq++;
                }

                if (!this._freqMap.TryGetValue(node.Value.Item2 + 1, out LinkedList<Tuple<int, int>> ll))
                {
                    ll = new LinkedList<Tuple<int, int>>();
                    this._freqMap[node.Value.Item2 + 1] = ll;
                }

                var newNode = ll.AddLast(Tuple.Create(node.Value.Item1, node.Value.Item2 + 1));
                this._cache[key] = Tuple.Create(value, newNode);
            }
            else
            {
                if (this._capacity == 0) return;

                if (this._count == this._capacity)
                {
                    // evict one.
                    var minFreqList = this._freqMap[this._minFreq];
                    var itemToEvict = minFreqList.First;
                    minFreqList.RemoveFirst();

                    this._cache.Remove(itemToEvict.Value.Item1);
                }
                else
                {
                    this._count++;
                }

                var firstFreq = this._freqMap[1];
                var newNode = firstFreq.AddLast(Tuple.Create(key, 1));
                this._cache[key] = Tuple.Create(value, newNode);
                this._minFreq = 1;
            }
        }
    }
}