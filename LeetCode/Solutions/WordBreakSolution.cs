namespace LeetCode.Csharp.Solutions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    public class WordBreakSolution
    {
        // 139 - https://leetcode.com/problems/word-break/
        public IList<string> WordBreak1(string s, IList<string> wordDict)
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
        
        // 140 - https://leetcode.com/problems/word-break-ii/
        public IList<string> WordBreak2(string s, IList<string> wordDict)
        {
            // O(s*n)
            
            // DP : Possible combinations starting from this point.
            List<string>[] dp = new List<string>[s.Length];
            
            for (int i = 0; i < s.Length; i++)
            {
                if (i == 0 || (dp[i - 1] != null && dp[i - 1].Count > 0))
                {
                    foreach (string word in wordDict)
                    {
                        if (word.Length > s.Length - i) continue;

                        bool match = true;
                        for (int j = 0; j < word.Length; j++)
                        {
                            if (word[j] != s[i + j])
                            {
                                match = false;
                                break;
                            }
                        }

                        if (match)
                        {
                            int nIndex = word.Length + i - 1;
                            if (dp[nIndex] == null) dp[nIndex] = new List<string>();
                            if (i == 0)
                            {
                                dp[nIndex].Add(word);
                            }
                            else
                            {
                                dp[nIndex].AddRange(dp[i - 1].Select(ss => $"{ss} {word}"));
                            }
                        }
                    }
                }
            }

            return dp[s.Length - 1] ?? new List<string>();
        }
        
        public IList<string> WordBreak(string s, IList<string> wordDict)
        {
            // memorize.
            return InnerWordBreak2Memo(s, 0, wordDict, new List<string>[s.Length]);
        }

        private IList<string> InnerWordBreak2Memo(string s, int idx, IList<string> wordDict, List<string>[] memo)
        {
            if (memo[idx] == null)
            {
                memo[idx] = new List<string>();
                string substr = idx == 0 ? s : s.Substring(idx);
                foreach (string word in wordDict)
                {
                    if (word.Length <= substr.Length && substr.StartsWith(word))
                    {
                        if(substr.Length == word.Length) memo[idx].Add(word);
                        else
                        {
                            IList<string> nextResult = this.InnerWordBreak2Memo(s, idx + word.Length, wordDict, memo);
                            if (nextResult.Count > 0)
                            {
                                foreach (string s1 in nextResult)
                                {
                                    memo[idx].Add($"{word} {s1}");
                                }
                            }
                        }
                    }
                }
            }

            return memo[idx];
        }

        public void Run()
        {
            Console.WriteLine(JsonConvert.SerializeObject(WordBreak("catsanddog", new[] {"cat", "cats", "and", "sand", "dog"})));
        }
    }
}