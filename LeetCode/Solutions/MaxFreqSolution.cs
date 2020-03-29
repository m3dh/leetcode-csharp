namespace LeetCode.Csharp.Solutions
{
    using System.Collections.Generic;
    using System.Linq;

    public class MaxFreqSolution
    {
        // 1297 - https://leetcode.com/problems/maximum-number-of-occurrences-of-a-substring/
        public int MaxFreq(string s, int maxLetters, int minSize, int maxSize)
        {
            if (s.Length == 0) return 0;
            
            Dictionary<string, int> counters = new Dictionary<string, int>();
            for (int l = 0; l < s.Length; l++)
            {
                for (int len = minSize; len <= maxSize; len++)
                {
                    if (l + len > s.Length) break;
                    string substring = s.Substring(l, len);

                    if (substring.GroupBy(ch => ch).Count() > maxLetters)
                    {
                        break;
                    }

                    if (!counters.ContainsKey(substring))
                    {
                        counters[substring] = 1;
                    }
                    else
                    {
                        counters[substring]++;
                    }
                }
            }

            if (counters.Count == 0) return 0;
            return counters.Max(p => p.Value);
        }
    }
}