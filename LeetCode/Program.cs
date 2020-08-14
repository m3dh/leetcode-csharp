﻿namespace LeetCode.Csharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LeetCode.Csharp.Common;
    using LeetCode.Csharp.Solutions;
    using LeetCode.Csharp.Solutions2;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class Program
    {
        public static void Main()
        {
            new Arrays2().Run();
        }
    }

    // https://leetcode.com/problems/iterator-for-combination/
    public class CombinationIterator
    {
        private readonly int _combinationLength;
        private readonly char[] _chars;
        private readonly int[] _current;
        private bool _hasNext;

        // (0,1,2,3), 2

        public CombinationIterator(string characters, int combinationLength)
        {
            this._combinationLength = combinationLength;
            this._chars = characters.ToCharArray();
            this._current = new int[combinationLength];

            for (int i = 0; i < combinationLength; i++)
            {
                this._current[i] = i;
            }

            this._hasNext = combinationLength <= this._chars.Length;
        }

        public string Next()
        {
            if (!this._hasNext) throw new InvalidOperationException();

            string ret = new string(this._current.Select(i => this._chars[i]).ToArray());

            this._hasNext = this._current[0] < this._chars.Length - this._combinationLength;

            if (this._hasNext)
            {
                for (int i = this._combinationLength - 1; i >= 0; i--)
                {
                    if (this._current[i] < this._chars.Length - (this._combinationLength - i))
                    {
                        this._current[i]++;
                        for (int j = i + 1; j < this._combinationLength; j++)
                        {
                            this._current[j] = this._current[j - 1] + 1;
                        }

                        break;
                    }
                }
            }

            return ret;
        }

        public bool HasNext()
        {
            return this._hasNext;
        }
    }

    // https://leetcode.com/problems/product-of-the-last-k-numbers/
    public class ProductOfNumbers
    {
        private List<long> list = new List<long>();

        public ProductOfNumbers()
        {
        }

        public void Add(int num)
        {
            if (num == 0)
            {
                this.list.Clear();
            }
            else
            {
                if (this.list.Count == 0)
                {
                    this.list.Add(num);
                }
                else
                {
                    this.list.Add(num * this.list[this.list.Count - 1]);
                }
            }
        }

        public int GetProduct(int k)
        {
            if (k > this.list.Count) return 0;
            else
            {
                // k = 2, 3 items    -   l[n-1] / l[n-3]
                var pre = this.list.Count >= k + 1 ? this.list[this.list.Count - k - 1] : 1;
                return (int) (this.list[this.list.Count - 1] / pre);
            }
        }
    }
}