namespace LeetCode.Csharp.Solutions
{
    public class CheckInclusionSolution
    {
        // 567 - https://leetcode.com/problems/permutation-in-string/
        public bool CheckInclusion(string s1, string s2)
        {
            int len = s1.Length;
            if (len == 0 || len > s2.Length) return false;
            
            int[] target = new int[26];
            int[] current = new int[26];
            for (int i = 0; i < len; i++)
            {
                target[s1[i] - 'a']++;
                current[s2[i] - 'a']++;
            }

            if (this.CompareAlphabet(target, current)) return true;

            for (int i = len; i < s2.Length; i++)
            {
                current[s2[i - len] - 'a']--;
                current[s2[i] - 'a']++;
                
                if (this.CompareAlphabet(target, current)) return true;
            }

            return false;
        }
        
        private bool CompareAlphabet(int[]l, int[]r)
        {
            for (int i = 0; i < 26; i++)
            {
                if (l[i] != r[i]) return false;
            }

            return true;
        }
    }
}