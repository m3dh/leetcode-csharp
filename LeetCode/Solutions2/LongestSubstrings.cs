namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Collections.Generic;

    public class LongestSubstrings
    {
        public int LengthOfLongestSubstring(string s) {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }
        
            HashSet<char> window = new HashSet<char>();
            int l = 0;
            int r = 0;
            int maxLen = 0;

            while (true)
            {
                char rChar = s[r];
                if (window.Contains(rChar))
                {
                    while (s[l] != rChar)
                    {
                        window.Remove(s[l]);
                        l++;
                    }

                    l++;
                }

                window.Add(rChar);
                if (r - l + 1 > maxLen) maxLen = r - l + 1;
                if (++r >= s.Length) break;
            }
        
            return maxLen;
        }
    }
}