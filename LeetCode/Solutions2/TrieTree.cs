namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TrieTree
    {
        public class NumTrieNode
        {
            public NumTrieNode[] Next = new NumTrieNode[2];
            public int? Num;
        }

        public int FindMaximumXOR(int[] nums)
        {
            string[] numStrs = nums.Select(num => Convert.ToString(num, 2)).ToArray();
            int maxLen = numStrs.Select(s => s.Length).Max();
            NumTrieNode root = new NumTrieNode();
            for(int i=0; i<nums.Length; i++)
            {
                // aaa -> aa
                // 5 -> aa
                string s = numStrs[i];
                NumTrieNode node = root;
                int pad = maxLen - s.Length;
                for (int j = 0; j < maxLen; j++)
                {
                    int idx = j - pad;
                    char cur = idx >= 0 ? s[idx] : '0';
                    if (node.Next[cur - '0'] == null)
                    {
                        node.Next[cur - '0'] = new NumTrieNode();
                    }

                    node = node.Next[cur - '0'];
                    if (j == maxLen - 1)
                    {
                        node.Num = nums[i];
                    }
                }
            }

            int maxVal = int.MinValue;

            for (int i = 0; i < nums.Length; i++)
            {
                NumTrieNode node = root;
                string s = numStrs[i];
                int pad = maxLen - s.Length;
                for (int j = 0; j < maxLen; j++)
                {
                    int idx = j - pad;
                    char cur = idx >= 0 ? s[idx] : '0';
                    int toFind = cur == '0' ? 1 : 0;
                    if (node.Next[toFind] != null)
                    {
                        node = node.Next[toFind];
                    }
                    else
                    {
                        node = node.Next[cur - '0'];
                    }
                }

                if (node.Num != null && node.Num.Value != nums[i])
                {
                    maxVal = Math.Max(maxVal, node.Num.Value ^ nums[i]);
                }
            }

            return maxVal;
        }
    }
}
