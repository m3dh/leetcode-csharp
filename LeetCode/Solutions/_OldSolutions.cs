namespace LeetCode.Csharp.Solutions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LeetCode.Csharp.Common;

    public class OldSolutions
    {
        // https://leetcode.com/problems/permutation-in-string/
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

        // https://leetcode.com/problems/word-break/
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

        // https://leetcode.com/problems/binary-tree-postorder-traversal/
        public IList<int> PostorderTraversal(TreeNode root)
        {
            // Let's use two stacks!
            List<int> ret = new List<int>();

            if (root != null)
            {
                Stack<TreeNode> visiting = new Stack<TreeNode>();
                visiting.Push(root);

                while (visiting.Count > 0)
                {
                    var curNode = visiting.Pop();

                    if (curNode.left == null && curNode.right == null)
                    {
                        ret.Add(curNode.val);
                    }
                    else if (curNode.left != null)
                    {
                        var leftNode = curNode.left;
                        curNode.left = null;
                        visiting.Push(curNode);
                        visiting.Push(leftNode);
                    }
                    else // if(curNode.right != null)
                    {
                        var rightNode = curNode.right;
                        curNode.right = null;
                        visiting.Push(curNode);
                        visiting.Push(rightNode);
                    }
                }
            }

            return ret;
        }

        // https://leetcode.com/problems/binary-tree-inorder-traversal/
        public IList<int> InorderTraversal(TreeNode root)
        {
            List<int> ret = new List<int>();

            if (root != null)
            {
                Stack<TreeNode> visiting = new Stack<TreeNode>();
                visiting.Push(root);

                while (visiting.Count > 0)
                {
                    var curNode = visiting.Pop();

                    if (curNode.right != null)
                    {
                        visiting.Push(curNode.right);
                        curNode.right = null;
                    }
                    
                    if (curNode.left != null)
                    {
                        var leftNode = curNode.left;
                        curNode.left = null;
                        visiting.Push(curNode);
                        visiting.Push(leftNode);
                    }
                    else
                    {
                        ret.Add(curNode.val);
                    }
                }
            }

            return ret;
        }

        // https://leetcode.com/problems/binary-tree-preorder-traversal/
        public IList<int> PreorderTraversal(TreeNode root)
        {
            List<int> ret = new List<int>();

            if (root != null)
            {
                Stack<TreeNode> visiting = new Stack<TreeNode>();
                visiting.Push(root);

                while (visiting.Count > 0)
                {
                    var curNode = visiting.Pop();
                    ret.Add(curNode.val);

                    if (curNode.right != null) visiting.Push(curNode.right);
                    if (curNode.left != null) visiting.Push(curNode.left);
                }
            }

            return ret;
        }

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
                            currCount += (long)prevCount * 9;
                        }
                        else if (s[i - 1] == '2')
                        {
                            // 1 - 6
                            currCount += (long)prevCount * 6;
                        }
                        else if (s[i - 1] == '*')
                        {
                            // 11,12,13,14,15,16,17,18,19,21,22,23,24,25,26 -> NO '20'
                            currCount += (long)prevCount * 15;
                        }
                    }
                    else if (s[i - 1] == '*')
                    {
                        if (s[i] > '6') currCount += prevCount; // Can only be '1'
                        else currCount += (long)prevCount * 2; // '1' or '2'
                    }
                    else if (((s[i] <= '6' && s[i - 1] == '2') || s[i - 1] < '2') && s[i - 1] != '0')
                    {
                        currCount += prevCount;
                    }
                }

                dp[i] = (int)(currCount % modVal);
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

                if (i >= 1 && ((s[i] <= '6' && s[i - 1] == '2' )||s[i - 1] < '2' )&& s[i - 1] != '0')
                {
                    if (i == 1) currCount += 1;
                    else currCount += dp[i - 2];
                }

                dp[i] = currCount;
            }

            return dp[s.Length - 1];
        }

        // https://leetcode.com/problems/maximum-number-of-occurrences-of-a-substring/
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

        // https://leetcode.com/problems/single-number/
        // https://leetcode.com/problems/single-number-ii/
        public int SingleNumber(int[] nums)
        {
            int[] bitCounters = new int[32];
            int[] bitMasks = new int[32];

            int mask = 1;
            for (int i = 0; i < 32; i++)
            {
                bitMasks[i] = mask;
                mask <<= 1;
            }

            foreach (int num in nums)
            {
                for (int maskIndex = 0; maskIndex < 32; maskIndex++)
                {
                    if ((bitMasks[maskIndex] & num) != 0)
                    {
                        bitCounters[maskIndex]++;
                    }
                }
            }

            int result = 0;
            mask = 1;
            for (int i = 0; i < 32; i++)
            {
                if (bitCounters[i] % 3 == 1)
                {
                    result |= mask;
                }

                mask <<= 1;
            }

            return result;
        }
        
        // https://leetcode.com/problems/single-number-iii/
        public int[] SingleNumber3(int[] nums)
        {
            int axorb = 0;
            foreach (int num in nums)
            {
                axorb ^= num;
            }

            int mask = 1;
            while ((axorb & mask) == 0) mask <<= 1;

            int a = 0;
            int b = 0;
            foreach (int num in nums)
            {
                if ((mask & num) == 0) a ^= num;
                else b ^= num;
            }

            return new[] {a, b};
        }

        // https://leetcode.com/problems/search-in-rotated-sorted-array/
        public bool Search(int[] nums, int target)
        {
            return SearchI(nums, target) != -1;
        }

        public int SearchI(int[] nums, int target)
        {
            if (nums.Length == 0) return -1;

            int l = 0;
            int r = nums.Length - 1;
            while (l < r)
            {
                int mid = (l + r) / 2;

                Console.WriteLine($"L:{l},R:{r},M:{mid}");

                if (nums[mid] < target)
                {
                    if (nums[nums.Length - 1] >= target)
                    {
                        // In the normal right part.
                        l = mid + 1;
                    }
                    else if (nums[mid] < nums[nums.Length - 1])
                    {
                        r = mid;
                    }
                    else if (nums[mid] > nums[nums.Length - 1])
                    {
                        l = mid + 1;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (nums[mid] > target)
                {
                    if (nums[0] <= target)
                    {
                        // In the normal left part.
                        r = mid;
                    }
                    else if (nums[mid] > nums[0])
                    {
                        l = mid + 1;
                    }
                    else if (nums[mid] < nums[0])
                    {
                        r = mid;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    return mid;
                }
            }

            if (nums[r] == target) return r;
            return -1;
        }

        // https://leetcode.com/problems/minimum-cost-for-tickets/
        public int MincostTickets(int[] days, int[] costs)
        {
            // Price by the end of each day.
            var dp = new int[days.Length];
            for (int i = 0; i < days.Length; i++) dp[i] = int.MaxValue;


            // 0 - 1D, 1 - 7D, 2-30D
            for (int j = 0; j < days.Length; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    // >=
                    int expireDay = days[j] + (i == 0 ? 1 : (i == 1 ? 7 : 30));
                    int currPrice = (j > 0 ? dp[j - 1] : 0) + costs[i];
                    for (int k = j; k < days.Length; k++)
                    {
                        if (days[k] >= expireDay) break;
                        if (dp[k] > currPrice) dp[k] = currPrice;
                    }
                }
            }

            return dp[days.Length - 1];
        }

        // https://leetcode.com/problems/coin-change/
        public int CoinChange(int[] coins, int amount)
        {
            var dp = new int[amount + 1];
            dp[0] = 0;
            for (int i = 1; i <= amount; i++) dp[i] = -1;
            foreach (int coin in coins)
            {
                for (int i = 0; i < amount; i++)
                {
                    int newAmount = i + coin;
                    if (newAmount <= amount)
                    {
                        if (dp[newAmount] > dp[i] + 1 || dp[newAmount] < 0)
                        {
                            dp[newAmount] = dp[i] + 1;
                        }
                    }
                }
            }

            return dp[amount];
        }

        public int FindPoisonedDuration(int[] timeSeries, int duration)
        {
            int cnt = 0;
            int recoverTs = 0;
            foreach(int ts in timeSeries)
            {
                if (ts > recoverTs)
                {
                    // New poison
                    recoverTs = ts + duration;
                    cnt += duration;
                }
                else if (ts + duration > recoverTs)
                {
                    // Extending poison
                    int newRecoverTime = ts + duration;
                    cnt += (newRecoverTime - recoverTs);
                    recoverTs = newRecoverTime;
                }
            }
            
            return cnt;
        }

        public int MinDeletionSize(string[] A)
        {
            if (A.Length == 0 || A[0].Length == 0) return -1;

            int wLen = A[0].Length;
            int[] dp = new int[wLen];
            for (int i = 0; i < dp.Length; i++) dp[i] = 1;

            for (int i = 0; i < wLen - 1; i++)
            {
                for (int j = i + 1; j < wLen; j++)
                {
                    if (A.All(a => a[i] <= a[j]))
                    {
                        dp[j] = Math.Max(dp[j], dp[i] + 1);
                    }
                }
            }

            return wLen - dp.Max();
        }

        public bool CanReorderDoubled(int[] A)
        {
            SortedDictionary<int, int> count = new SortedDictionary<int, int>();
            foreach (int a in A)
            {
                count[a] = count.ContainsKey(a) ? count[a] + 1 : 1;
            }

            foreach (int a in count.Keys.ToList())
            {
                if (count[a] > 0)
                {
                    if (a < 0)
                    {
                        var b = a / 2;
                        if (a % 2 != 0 || !count.ContainsKey(b)) return false;
                        count[b] -= count[a];
                    }
                    else
                    {
                        var b = a * 2;
                        if (!count.ContainsKey(b)) return false;
                        count[b] -= count[a];
                    }
                }
                else if (count[a] < 0)
                {
                    return false;
                }
            }

            return true;
        }

        public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
        {
            ListNode ret = new ListNode(0);
            ListNode cur = ret;
            while (l1 != null || l2 != null)
            {
                int mVal = (l1?.val ?? 0) + (l2?.val ?? 0) + cur.val;
                
                l1 = l1?.next;
                l2 = l2?.next;
                int preUp = 0;
                if (mVal >= 10)
                {
                    preUp = mVal / 10;
                    mVal = mVal % 10;
                }

                cur.val = mVal;

                if (l1 != null || l2 != null || preUp > 0)
                {
                    cur.next = new ListNode(preUp);
                    cur = cur.next;
                }
            }

            return ret;
        }
    }
}