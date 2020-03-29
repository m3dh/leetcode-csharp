namespace LeetCode.Csharp.Solutions
{
    public class NumDecodingSolution
    {
        // https://leetcode.com/problems/decode-ways-ii/
        public int NumDecodings2(string s)
        {
            if (s.Length == 0) return 0;
            int modVal = 7 + 1000000000;

            int[] dp = new int[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                long currCount = 0;
                if (s[i] == '*')
                {
                    // 1 - 9
                    if (i == 0) currCount += 9;
                    else currCount += 9 * (long) dp[i - 1];
                }
                else if (s[i] != '0')
                {
                    if (i == 0) currCount += 1;
                    else currCount += dp[i - 1];
                }

                if (i >= 1)
                {
                    int prevCount = i == 1 ? 1 : dp[i - 2];
                    if (s[i] == '*')
                    {
                        if (s[i - 1] == '1')
                        {
                            // 1 - 9
                            currCount += (long) prevCount * 9;
                        }
                        else if (s[i - 1] == '2')
                        {
                            // 1 - 6
                            currCount += (long) prevCount * 6;
                        }
                        else if (s[i - 1] == '*')
                        {
                            // 11,12,13,14,15,16,17,18,19,21,22,23,24,25,26 -> NO '20'
                            currCount += (long) prevCount * 15;
                        }
                    }
                    else if (s[i - 1] == '*')
                    {
                        if (s[i] > '6') currCount += prevCount; // Can only be '1'
                        else currCount += (long) prevCount * 2; // '1' or '2'
                    }
                    else if (((s[i] <= '6' && s[i - 1] == '2') || s[i - 1] < '2') && s[i - 1] != '0')
                    {
                        currCount += prevCount;
                    }
                }

                dp[i] = (int) (currCount % modVal);
            }

            return dp[s.Length - 1];
        }

        // https://leetcode.com/problems/decode-ways/
        public int NumDecodings(string s)
        {
            if (s.Length == 0) return 0;

            int[] dp = new int[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                int currCount = 0;
                if (s[i] != '0')
                {
                    if (i == 0) currCount += 1;
                    else currCount += dp[i - 1];
                }

                if (i >= 1 && ((s[i] <= '6' && s[i - 1] == '2') || s[i - 1] < '2') && s[i - 1] != '0')
                {
                    if (i == 1) currCount += 1;
                    else currCount += dp[i - 2];
                }

                dp[i] = currCount;
            }

            return dp[s.Length - 1];
        }
    }
}