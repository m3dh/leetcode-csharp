namespace LeetCode.Csharp.Solutions
{
    using System.Collections.Generic;
    using System.Linq;

    public class WordBreakSolution
    {
        // 139 - https://leetcode.com/problems/word-break/
        public IList<string> WordBreak(string s, IList<string> wordDict)
        {
            List<string>[] dp = new List<string>[s.Length]; 
            for (int i = 0; i < s.Length; i++)
            {
                if (i == 0 || dp[i - 1]?.Count > 0)
                {
                    var currWords = new List<string>(wordDict);
                    for (int len = 1; i + len <= s.Length && currWords.Count > 0; len++)
                    {
                        var subs = s.Substring(i, len);
                        currWords = currWords.Where(w => w.StartsWith(subs)).ToList();

                        var matchs = currWords.Where(w => w.Equals(subs)).ToList();
                        if (matchs.Count > 0)
                        {
                            var index = i + len - 1;
                            if (dp[index] == null) dp[index] = new List<string>();
                            if (i == 0) dp[index].AddRange(matchs);
                            else dp[index].AddRange(dp[i - 1].SelectMany(st => matchs.Select(mw => $"{st} {mw}")));
                        }
                    }
                }
            }

            // Console.WriteLine(string.Join(", ", dp));
            return dp[s.Length - 1];
        }
    }
}