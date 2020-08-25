namespace LeetCode.Csharp
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
            Console.WriteLine($"RET: {new CalculatorSolution().Calculate("(1+(4+5+2)-3)+(6+8)")}");
            Console.WriteLine($"RET: {new CalculatorSolution().Calculate(" 2-1 + 2")}");
            Console.WriteLine($"RET: {new CalculatorSolution().Calculate(" 3+5/2")}");
        }

        public int FindLengthOfLCIS(int[] nums)
        {
            int l = 0;
            int r = 0;
            int max = 0;

            // 1,3,5,4,7
            // 0, 0 => 1
            //    2 => 3
            //    2

            while (l < nums.Count())
            {
                if (r - l + 1 > max)
                {
                    max = r - l + 1;
                }

                bool moved = false;
                while (r + 1 < nums.Count() && nums[r + 1] > nums[r])
                {
                    moved = true;
                    r++;
                }

                if (!moved)
                {
                    l = r + 1;
                    r = l;
                }
            }

            return max;
        }

        public string AlienOrder(string[] words)
        {
            List<char[]> odr = new List<char[]>(); // having a < b.
            HashSet<char> apr = new HashSet<char>();
            for (int i = 0; i < words.Length; i++)
            {
                for (int j = 0; j < words[i].Length; j++)
                    apr.Add(words[i][j]);
            }

            for (int i = 1; i < words.Length; i++)
            {
                string s = words[i - 1];
                string l = words[i];
                int minLen = Math.Min(s.Length, l.Length);
                for (int j = 0; j < minLen; j++)
                {
                    if (s[j] != l[j])
                    {
                        odr.Add(new[] { s[j], l[j] });
                        break;
                    }
                }
            }

            List<char> result = new List<char>();
            Dictionary<char, HashSet<char>> dirs = new Dictionary<char, HashSet<char>>();

            int[] inCnt = new int[26];
            foreach (char[] od in odr)
            {
                if (dirs.TryGetValue(od[0], out HashSet<char> h))
                {
                    if (h.Add(od[1]))
                    {
                        inCnt[od[1] - 'a']++;
                    }
                }
                else
                {
                    h = new HashSet<char>();
                    h.Add(od[1]);
                    dirs[od[0]] = h;

                    inCnt[od[1] - 'a']++;
                }
            }

            while (apr.Count() > 0)
            {
                bool flag = false;
                foreach (char c in apr)
                {
                    if (inCnt[c - 'a'] == 0)
                    {
                        flag = true;
                        result.Add(c);
                        apr.Remove(c);
                        if (dirs.TryGetValue(c, out HashSet<char> h))
                        {
                            foreach (char c2 in h)
                            {
                                inCnt[c2 - 'a']--;
                            }
                        }
                        break;
                    }
                }

                if (!flag) return "";
            }

            return new string(result.ToArray());
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