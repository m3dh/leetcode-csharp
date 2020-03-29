namespace LeetCode.Csharp.Solutions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MinWindowSolution
    {
        // 76 - https://leetcode.com/problems/minimum-window-substring/
        public string MinWindow(string s, string t)
        {
            Dictionary<char, int> expectChars = new Dictionary<char, int>();
            Dictionary<char, int> currChars = new Dictionary<char, int>();

            foreach (char c in t)
            {
                if (!expectChars.ContainsKey(c)) expectChars.Add(c, 1);
                else expectChars[c]++;

                if (!currChars.ContainsKey(c)) currChars.Add(c, 0);
            }

            string minLenStr = null;
            int l = 0;
            int r = 0;
            while (r < s.Length)
            {
                if (expectChars.ContainsKey(s[r]))
                {
                    currChars[s[r]]++;
                }

                if (expectChars.All(ec => currChars[ec.Key] >= ec.Value))
                {
                    // Once meet the requirements, move L to make the string shorter.
                    while (l <= r)
                    {
                        if (expectChars.ContainsKey(s[l]))
                        {
                            if (currChars[s[l]] > expectChars[s[l]])
                            {
                                currChars[s[l]]--;
                            }
                            else
                            {
                                // Cannot be removed.
                                break;
                            }
                        }
                        
                        l++;
                    }
                    
                    if (minLenStr == null || r - l + 1 < minLenStr.Length)
                    {
                        minLenStr = s.Substring(l, r - l + 1);
                    }
                }

                r++;
            }

            return minLenStr ?? "";
        }
    }
}