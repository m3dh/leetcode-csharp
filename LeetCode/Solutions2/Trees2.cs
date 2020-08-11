namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
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

        public void Run()
        {
            Console.WriteLine(JsonConvert.SerializeObject(DiffWaysToCompute("2*3-4*5"), Formatting.Indented));
        }
    }
}
