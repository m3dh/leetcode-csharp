﻿namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading;
    using LeetCode.Csharp.Common;
    using Newtonsoft.Json;

    public class Trees2
    {
        // https://leetcode.com/problems/different-ways-to-add-parentheses/
        public IList<int> DiffWaysToCompute(string input)
        {
            // REVIEW: Idea - Divide and conquer
            char[] opers = new[] { '*', '-', '+' };
            List<string> items = new List<string>();

            int lastIdx = 0;
            for (int i = 0; i <= input.Length; i++)
            {
                if (i == input.Length || opers.Any(o => input[i] == o))
                {
                    items.Add(input.Substring(lastIdx, (i-lastIdx)));
                    if (i < input.Length) items.Add(new string(input[i], 1));
                    lastIdx = i+1;
                }
            }

            return DiffWaysToCompute(items, 0, items.Count - 1);
        }

        private List<int> DiffWaysToCompute(List<string> items, int l, int r)
        {
            if (r == l) return new List<int> { int.Parse(items[r]) };

            List<int> ret = new List<int>();
            for (int i = l + 1; i <= r - 1; i+=2)
            {
                string oper = items[i];
                List<int> leftVals = DiffWaysToCompute(items, l, i - 1);
                List<int> rightVals = DiffWaysToCompute(items, i + 1, r);
                var func = GetCalc(oper);
                foreach (int leftVal in leftVals)
                {
                    foreach (int rightVal in rightVals)
                    {
                        ret.Add(func(leftVal, rightVal));
                    }
                }
            }

            return ret;
        }

        private Func<int, int, int> GetCalc(string op)
        {
            switch (op)
            {
                case "+": return (a, b) => a + b;
                case "-": return (a, b) => a - b;
                case "*": return (a, b) => a * b;
                default: throw new Exception();
            }
        }

        // https://leetcode.com/problems/maximum-binary-tree/
        public TreeNode ConstructMaximumBinaryTree(int[] nums)
        {
            // REVIEW: 用栈来实现O(n)
            Stack<TreeNode> s = new Stack<TreeNode>();
            foreach (int num in nums)
            {
                TreeNode n = new TreeNode(num);
                if (s.Count == 0)
                {
                    s.Push(n);
                }
                else if (s.Peek().val > num)
                {
                    s.Peek().right = n;
                    s.Push(n);
                }
                else
                {
                    while (s.Count > 0 && s.Peek().val < num)
                    {
                        n.left = s.Pop();
                    }

                    if (s.Count > 0)
                    {
                        s.Peek().right = n;
                    }

                    s.Push(n);
                }
            }

            while (s.Count > 1) s.Pop();

            return s.First();
        }

        // https://leetcode.com/problems/distribute-coins-in-binary-tree/submissions/
        public int DistributeCoins(TreeNode root)
        {
            int count = 0;
            DistributeCoinsRec(root, ref count);
            return count;
        }

        private int DistributeCoinsRec(TreeNode root, ref int count)
        {
            if (root == null) return 0;

            int curCoins = root.val + DistributeCoinsRec(root.left, ref count) + DistributeCoinsRec(root.right, ref count);
            int diff = Math.Abs(1 - curCoins);
            count += diff;
            return curCoins - 1;
        }

        // https://leetcode.com/problems/convert-sorted-list-to-binary-search-tree/
        public TreeNode SortedListToBST(ListNode head)
        {
            if (head == null) return null;

            int len = 0;
            ListNode n = head;
            while (n != null)
            {
                len++;
                n = n.next;
            }

            return SortedListToBST(ref head, 0, len - 1);
        }

        private TreeNode SortedListToBST(ref ListNode n, int l, int r)
        {
            TreeNode ret = null;

            if (n != null)
            {
                if (l == r)
                {
                    ret = new TreeNode(n.val);
                    n = n.next;
                }
                else
                {
                    // 0, 3 (0,1,2,3)
                    // 0, | 1 | 2, 3

                    int leftLen = (r - l) / 2;
                    int rightLen = (r - l) - leftLen;

                    // REVIEW: 边界条件处理

                    TreeNode lt = leftLen == 0 ? null : SortedListToBST(ref n, l, l + leftLen - 1);
                    ret = new TreeNode(n.val);
                    n = n.next;

                    TreeNode rt = rightLen == 0 ? null : SortedListToBST(ref n, r - rightLen + 1, r);

                    ret.left = lt;
                    ret.right = rt;
                }
            }

            return ret;
        }

        // https://leetcode.com/problems/largest-bst-subtree/
        private int maxSize = 0;

        public int LargestBSTSubtree(TreeNode root)
        {
            if (root != null)
            {
                // IDEA: 在DFS过程中记录下每个子树的最大、最小值，这样就能够自底向上构建树了
                Search(root);
            }

            return maxSize;
        }

        private int[] Search(TreeNode root)
        {
            // return [size, min, max]
            int[] l = root.left == null ? null : Search(root.left);
            int[] r = root.right == null ? null : Search(root.right);

            int size = 1;
            int min = root.val;
            int max = root.val;
            if (l != null)
            {
                if (l[0] != -1 && l[2] < root.val)
                {
                    size += l[0];
                    min = l[1];
                }
                else
                {
                    return new[] { -1, -1, -1 };
                }
            }

            if (r != null)
            {
                if (r[0] != -1 && r[1] > root.val)
                {
                    size += r[0];
                    max = r[2];
                }
                else
                {
                    return new[] { -1, -1, -1 };
                }
            }

            maxSize = Math.Max(size, maxSize);

            return new[] { size, min, max };
        }

        // https://leetcode.com/problems/my-calendar-i/
        public class MyCalendar
        {
            class BookMapNode
            {
                public int Start;
                public int End;

                public BookMapNode Left;
                public BookMapNode Right;
            }

            private BookMapNode _root = null;

            public MyCalendar()
            {
            }

            private bool Insert(int start, int end)
            {
                if (this._root == null)
                {
                    this._root = new BookMapNode
                    {
                        Start = start,
                        End = end,
                    };

                    return true;
                }
                else
                {
                    BookMapNode n = this._root;
                    while (true)
                    {
                        // [n.S, n.E]
                        //     
                        if (n.End <= start)
                        {
                            if (n.Right != null)
                            {
                                n = n.Right;
                            }
                            else
                            {
                                n.Right = new BookMapNode
                                {
                                    Start = start,
                                    End = end
                                };

                                return true;
                            }
                        }
                        else if (n.Start > end)
                        {
                            if (n.Left != null)
                            {
                                n = n.Left;
                            }
                            else
                            {
                                n.Left = new BookMapNode
                                {
                                    Start = start,
                                    End = end
                                };

                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            public bool Book(int start, int end)
            {
                return Insert(start, end);
            }
        }

        // https://leetcode.com/problems/my-calendar-ii/
        public class MyCalendarTwo
        {
            private List<int[]>[] intervals = new List<int[]>[2];

            public MyCalendarTwo()
            {
            }

            public bool Book(int start, int end)
            {
                foreach (int[] ints in this.intervals[1])
                {
                    if (ints[0] < end && ints[1] > start) return false;
                }

                for (int i = 0; i <= 1; i++)
                {
                    if (i == 0)
                    {
                        this.intervals[1].Add(new[] { start, end });
                    }
                    else
                    {
                        foreach (int[] ints in this.intervals[i - 1])
                        {
                            if (ints[0] < end && ints[1] > start)
                            {
                                this.intervals[i].Add(new[] { Math.Max(start, ints[0]), Math.Max(end, ints[1]) });
                            }
                        }
                    }
                }

                return true;
            }
        }

        // https://leetcode.com/problems/my-calendar-iii/
        public class MyCalendarThree
        {
            private List<List<int[]>> intervals = new List<List<int[]>>();

            private SortedList<int, int> deltas = new SortedList<int, int>();

            public MyCalendarThree()
            {
            }

            public int Book(int start, int end)
            {
                this.deltas[start] = this.deltas.TryGetValue(start, out int sd) ? sd + 1 : 1;
                this.deltas[end] = this.deltas.TryGetValue(end, out int ed) ? ed - 1 : -1;
                int ret = 0;
                int cur = 0;
                foreach (KeyValuePair<int, int> delta in this.deltas)
                {
                    cur += delta.Value;
                    ret = Math.Max(ret, cur);
                }

                return ret;
            }

            public int Book1(int start, int end)
            {
                int curMax = intervals.Count;
                if (curMax == 0)
                {
                    intervals.Add(new List<int[]> { new[] { start, end } });
                }
                else
                {
                    List<int[]> next = null;
                    foreach (int[] ints in intervals[curMax - 1])
                    {
                        if (ints[0] < end && ints[1] > start)
                        {
                            if (next == null)
                            {
                                next = new List<int[]>();
                            }

                            next.Add(new[] { Math.Max(ints[0], start), Math.Min(ints[1], end) });
                        }
                    }

                    if (next != null)
                    {
                        intervals.Add(next);
                    }

                    // now update the previous interval lists
                    for (int i = curMax - 1; i >= 0; i--)
                    {
                        if (i == 0)
                        {
                            intervals[i].Add(new[] { start, end });
                        }
                        else
                        {
                            foreach (int[] ints in intervals[i - 1])
                            {
                                if (ints[0] < end && ints[1] > start)
                                {
                                    intervals[i].Add(new[] { Math.Max(ints[0], start), Math.Min(ints[1], end) });
                                }
                            }
                        }
                    }
                }

                return intervals.Count;
            }
        }

        public string AlienOrder(string[] words)
        {
            Dictionary<char, int> icnt = new Dictionary<char, int>();
            Dictionary<char, List<char>> ocnt = new Dictionary<char, List<char>>();
            HashSet<char> found = new HashSet<char>();
            foreach(string w in words)
            {
                foreach (char c in w) found.Add(c);
            }

            for (int i = 1; i < words.Length; i++)
            {
                string w1 = words[i - 1];
                string w2 = words[i];
                int minLen = Math.Min(w1.Length, w2.Length);
                for (int j = 0; j < minLen; j++)
                {
                    if (w1[j] != w2[j])
                    {
                        if (icnt.ContainsKey(w2[j])) icnt[w2[j]]++;
                        else icnt[w2[j]] = 1;

                        if (ocnt.TryGetValue(w1[j], out List<char> l)) l.Add(w2[j]);
                        else ocnt[w1[j]] = new List<char> { w2[j] };

                        break;
                    }
                }
            }

            List<char> result = new List<char>();
            char? next = null;
            while (true)
            {
                if (next == null)
                {
                    next = found.FirstOrDefault(c => !icnt.ContainsKey(c) || icnt[c] == 0);
                    if (next == null || next == char.MinValue)
                    {
                        if (found.Count() == 0)
                        {
                            break;
                        }
                        else
                        {
                            return "";
                        }
                    }
                }

                char curr = next.Value;
                next = null;
                found.Remove(curr);
                result.Add(curr);

                if (ocnt.ContainsKey(curr))
                {
                    foreach (char nxt in ocnt[curr])
                    {
                        icnt[nxt]--;
                        if (icnt[nxt] == 0 && next == null)
                        {
                            next = nxt;
                        }
                    }
                }
            }

            return new string(result.ToArray());
        }

        /**
         * Your MyCalendarThree object will be instantiated and called as such:
         * MyCalendarThree obj = new MyCalendarThree();
         * int param_1 = obj.Book(start,end);
         */

        public void Run()
        {
            Console.WriteLine(AlienOrder(new[] {"a", "b", "ca", "cc" }));
        }
    }
}
