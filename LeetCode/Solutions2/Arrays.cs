namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using LeetCode.Csharp.Common;
    using Newtonsoft.Json;

    public class Arrays
    {
        // 560 - https://leetcode.com/problems/subarray-sum-equals-k/
        public int SubarraySum(int[] nums, int k)
        {
            // REVIEW: 局部性思想
            Dictionary<int, int> sums = new Dictionary<int, int> {{0, 1}};
            int sum = 0;
            int retCount = 0;
            foreach (int num in nums)
            {
                sum += num;
                if (sums.ContainsKey(sum - k)) retCount += sums[sum - k];
                sums[sum] = sums.ContainsKey(sum) ? sums[sum] + 1 : 1;
            }

            return retCount;
        }

        // 32 - https://leetcode.com/problems/longest-valid-parentheses/
        public int LongestValidParentheses(string s)
        {
            int l = 0;
            int r = 0;
            int max = 0;
            foreach (char c in s)
            {
                if (c == '(') l++;
                else if (c == ')') r++;

                if (r > l)
                {
                    l = 0;
                    r = 0;
                }
                else if (r == l) max = Math.Max(max, r * 2);
            }

            // Handle (()
            l = 0;
            r = 0;
            foreach (char c in s.Reverse())
            {
                if (c == '(') l++;
                else if (c == ')') r++;

                if (l > r)
                {
                    l = 0;
                    r = 0;
                }
                else if (r == l) max = Math.Max(max, r * 2);
            }

            return max;
        }

        // 5 - https://leetcode.com/problems/longest-palindromic-substring/
        public string LongestPalindrome(string s)
        {
            string longestSub = "";

            Action<int, int> tryMax = (l, r) =>
            {
                while (l >= 0 && r < s.Length && s[l] == s[r])
                {
                    l--;
                    r++;
                }

                if (r - l + 1 > longestSub.Length)
                {
                    longestSub = s.Substring(l, r - l + 1);
                }
            };

            for (int i = 0; i < s.Length; i++)
            {
                tryMax(i, i);
                tryMax(i, i + 1);
            }

            return longestSub;
        }

        // 11 - https://leetcode.com/problems/container-with-most-water/
        public int MaxArea(int[] height)
        {
            // REVIEW: 贪心思想 - 先移动短的
            int max = 0;
            int i = 0;
            int j = height.Length - 1;
            while (i < j)
            {
                if (Math.Min(height[i], height[j]) * (j - i) > max)
                {
                    max = Math.Min(height[i], height[j]) * (j - i);
                }

                if (height[i] < height[j]) i++;
                else j--;
            }

            return max;
        }

        // 42 - https://leetcode.com/problems/trapping-rain-water/
        public int Trap(int[] height)
        {
            int count = 0;

            // Think about cornor cases.

            for (int i = 0; i < height.Length - 1;)
            {
                if (height[i] == 0)
                {
                    i++;
                    continue;
                }

                int maxVal = -1;
                int maxIdx = -1;
                for (int j = i + 1; j < height.Length; j++)
                {
                    if (height[j] > maxVal)
                    {
                        maxVal = height[j];
                        maxIdx = j;
                    }

                    if (height[j] >= height[i])
                    {
                        // In this case, the first if has been executed.
                        break;
                    }
                }

                int level = Math.Min(height[maxIdx], height[i]);
                for (int j = i + 1; j < maxIdx; j++)
                {
                    count += (level - height[j]);
                }

                // Console.WriteLine($"FROM: {height[i]} MAX:{maxVal} IDX:{i}-{maxIdx}, C:{count}");

                i = maxIdx;
            }

            return count;
        }

        // 844 - https://leetcode.com/problems/backspace-string-compare/
        public bool BackspaceCompare(string S, string T)
        {
            // REVIEW: 倒序处理
            int sIndex = S.Length - 1;
            int tIndex = T.Length - 1;
            while (true)
            {
                sIndex = MoveIndexBack(S, sIndex);
                tIndex = MoveIndexBack(T, tIndex);

                if (sIndex < 0 || tIndex < 0) return sIndex == tIndex;
                if (S[sIndex] != T[tIndex]) return false;

                sIndex--;
                tIndex--;
            }
        }

        private int MoveIndexBack(string str, int index)
        {
            int moveCount = 0;
            while (index >= 0 && (str[index] == '#' || moveCount > 0))
            {
                if (str[index] == '#') moveCount++;
                else moveCount--;
                index--;
            }

            // MOVE: Until the first char that should be compared, or -1.
            return index;
        }

        // 155 - MinStack
        public class MinStack
        {
            // REVIEW: 边界条件，this._currMin >= x 也需要 Push 两次
            private int _currMin = Int32.MaxValue;
            private readonly Stack<int> _stack;

            /** initialize your data structure here. */
            public MinStack()
            {
                this._stack = new Stack<int>();
            }

            public void Push(int x)
            {
                if (this._currMin >= x)
                {
                    // When stack.Pop() == currMin, we pop again for next min value...
                    // The compare should be >= since the min value could be pushed more than once.
                    this._stack.Push(this._currMin);
                    this._currMin = x;
                }

                this._stack.Push(x);
            }

            public void Pop()
            {
                int val = this._stack.Pop();
                if (val == this._currMin)
                {
                    // currMin being poped...
                    this._currMin = this._stack.Pop();
                }
            }

            public int Top()
            {
                return this._stack.Peek();
            }

            public int GetMin()
            {
                return this._currMin;
            }
        }

        // 525 - https://leetcode.com/problems/contiguous-array/
        public int FindMaxLength(int[] nums)
        {
            // REVIEW: The idea is to save the visited (delta) status, and when the same status has been found afterwards, status_new - status_old => equal 0 & 1.
            int maxLen = 0;
            int status = 0;

            Dictionary<int, int> visited = new Dictionary<int, int> {{status, -1}};

            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] == 1) status++;
                else if (nums[i] == 0) status--;

                if (visited.ContainsKey(status))
                {
                    int prevIndex = visited[status];
                    maxLen = Math.Max(maxLen, i - prevIndex);
                }
                else
                {
                    visited[status] = i;
                }
            }

            return maxLen;
        }

        public string StringShift(string s, int[][] shift)
        {
            if (shift.Length == 0) return s;

            // Merge shifts
            int shiftDirection = shift[0][0];
            int shiftCount = shift[0][1];
            for (int i = 1; i < shift.Length; i++)
            {
                if (shiftDirection == shift[i][0]) shiftCount += shift[i][1];
                else
                {
                    shiftCount -= shift[i][1];
                    if (shiftCount < 0)
                    {
                        shiftCount = -shiftCount;
                        shiftDirection = (shiftDirection + 1) % 2;
                    }
                }

                // Console.WriteLine($"Direction:{shiftDirection},Shift:{shiftCount}");
            }


            // Now that we should swap.
            char[] sc = s.ToCharArray();

            int shiftDt = shiftDirection == 0 ? -shiftCount : shiftCount;
            while (shiftDt < 0) shiftDt += sc.Length;
            for (int i = 0; i < sc.Length; i++)
            {
                int target = (i + shiftDt + sc.Length) % sc.Length;
                // Console.WriteLine($"{i}->{target}");
                sc[target] = s[i];
            }

            return new string(sc);
        }

        // 678 - https://leetcode.com/problems/valid-parenthesis-string/
        public bool CheckValidString(string s)
        {
            // The brilliant idea here is, like for strings don't have a '*' (and we use a counter of left c's to valid it),
            // we just maintain the minimal left c's and a maximal, then decide if it's still possible to have it always positive though the iteration.

            int minCnt = 0;
            int maxCnt = 0;

            foreach (char c in s)
            {
                if (c == ')')
                {
                    minCnt--;
                    maxCnt--;
                }
                else if (c == '(')
                {
                    minCnt++;
                    maxCnt++;
                }
                else // c == '*'
                {
                    maxCnt++;
                    minCnt--;
                }

                if (minCnt < 0) minCnt = 0;
                if (maxCnt < 0) return false;
            }

            return minCnt == 0; // minCnt have to be 0 to make it balanced.
        }

        // 48 - https://leetcode.com/problems/rotate-image/
        public void Rotate(int[][] matrix)
        {
            int halfLen = (matrix.Length + 1) / 2; // 3 => 2, 2 => 1
            for (int i = 0; i < halfLen; i++)
            {
                for (int j = 0; j < halfLen - (matrix.Length % 2 == 0 ? 0 : 1); j++)
                {
                    int prevVal = matrix[i][j];
                    int[][] rotatePoints = this.GetRotationPoints(matrix.Length, i, j);
                    for (int k = 0; k < 4; k++)
                    {
                        int nextPrevVal = matrix[rotatePoints[k][0]][rotatePoints[k][1]];
                        matrix[rotatePoints[k][0]][rotatePoints[k][1]] = prevVal;
                        prevVal = nextPrevVal;
                    }
                }
            }
        }

        private int[][] GetRotationPoints(int length, int x, int y)
        {
            return new[]
            {
                new[] {0 + y, length - 1 - x},
                new[] {length - 1 - x, length - 1 - y},
                new[] {length - 1 - y, 0 + x},
                new[] {0 + x, 0 + y} // self
            };
        }

        // 41 - https://leetcode.com/problems/first-missing-positive/
        public int FirstMissingPositive(int[] nums)
        {
            // REVIEW: 结果只能是 1-n+1，因为nums里的数字最多吃掉1-n那么多数字

            // 1. remove neg numbers
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] <= 0)
                {
                    nums[i] = nums.Length + 1; // nums.Length + 1 won't affect the final result.
                }
            }

            // Console.WriteLine(string.Join(",", nums));

            for (int i = 0; i < nums.Length; i++)
            {
                int numVal = Math.Abs(nums[i]);
                if (numVal > 0 && numVal <= nums.Length && nums[numVal - 1] > 0)
                {
                    nums[numVal - 1] = -nums[numVal - 1];
                }
            }

            // Console.WriteLine(string.Join(",", nums));

            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] > 0)
                {
                    return i + 1;
                }
            }

            return nums.Length + 1;
        }

        // 239 - https://leetcode.com/problems/sliding-window-maximum/
        public int[] MaxSlidingWindow(int[] nums, int k)
        {
            // REVIEW: 维护一个有序队列，当新元素加入的时候，删除所有比他小的元素；窗口移动时，删除对应的元素（如果那个元素
            // 恰好是最大值的话...因为这个队列是双有序的 - 大小有序，前后有序
            LinkedList<int> maxQ = new LinkedList<int>();
            List<int> result = new List<int>();
            for (int i = 0; i < nums.Length; i++)
            {
                // 队列是先后进入有序的
                if (maxQ.Count > 0 && maxQ.First() == i - k) maxQ.RemoveFirst();

                while (maxQ.Count > 0 && nums[maxQ.Last()] <= nums[i])
                {
                    // 只需要保留最后一个最大值即可，因为这个最大值此时已经在窗口里
                    maxQ.RemoveLast();
                }

                // 每个元素都会入队一次
                maxQ.AddLast(i);

                if (i >= k - 1)
                {
                    // 队首：值最大且最早入队（其他更早但还在窗口里的元素已经在入队过程中被淘汰了）
                    result.Add(nums[maxQ.First()]);
                }
            }

            return result.ToArray();
        }

        // 480 - https://leetcode.com/problems/sliding-window-median/
        public double[] MedianSlidingWindow(int[] nums, int k)
        {
            List<int> window = nums.Take(k).OrderBy(n => n).ToList();
            List<double> ret = new List<double> {k % 2 == 0 ? ((double) (window[k / 2 - 1] + window[k / 2]) / 2) : (double) window[k / 2]};
            for (int i = k; i < nums.Length; i++)
            {
                // Console.WriteLine("BEGIN: {0}, R:{1}, A:{2}", string.Join(", ", window), nums[i-k], nums[i]);

                window.Remove(nums[i - k]);

                int idx = -1;
                for (int j = 0; j < window.Count; j++)
                {
                    if (window[j] > nums[i])
                    {
                        idx = j;
                        break;
                    }
                }

                if (idx >= 0)
                {
                    window.Insert(idx, nums[i]);
                }
                else
                {
                    window.Add(nums[i]);
                }

                // Console.WriteLine("END: {0}", string.Join(", ", window));

                ret.Add(k % 2 == 0 ? ((double) (window[k / 2 - 1] + window[k / 2]) / 2) : (double) window[k / 2]);
            }

            return ret.ToArray();
        }

        // 45 - https://leetcode.com/problems/jump-game-ii/
        public int Jump2(int[] nums)
        {
            // REVIEW: 每次下一步是来自于上一步拓展的范围内的...
            int steps = 0;
            int currPos = 0;
            int lastPos = 0;
            while (currPos < nums.Length - 1)
            {
                // Get the next max pos.
                int nextPos = currPos;
                while (lastPos <= currPos)
                {
                    nextPos = Math.Max(nextPos, lastPos + nums[lastPos]);
                    lastPos++;
                }

                // lastPos = currPos;
                currPos = nextPos;
                steps++;
            }

            return steps;
        }

        // https://leetcode.com/problems/jump-game-iii/
        public bool CanReach(int[] arr, int start)
        {
            bool[] visited = new bool[arr.Length];
            Queue<int> q = new Queue<int>();
            q.Enqueue(start);
            visited[start] = true;

            while (q.Count > 0)
            {
                int idx = q.Dequeue();

                if (arr[idx] == 0) return true;

                int l = idx - arr[idx];
                int r = idx + arr[idx];
                if (l >= 0 && !visited[l])
                {
                    visited[l] = true;
                    q.Enqueue(l);
                }

                if (r < arr.Length && !visited[r])
                {
                    visited[r] = true;
                    q.Enqueue(r);
                }
            }

            return false;
        }

        // https://leetcode.com/problems/jump-game-iv/
        public int MinJumps(int[] arr)
        {
            Dictionary<int, List<int>> valMap = arr
                .Select((v, i) => new {v = v, i = i})
                .GroupBy(t => t.v)
                .ToDictionary(t => t.Key, t => t.Select(ti => ti.i).ToList());

            int count = 0;
            int start = 0;
            bool[] visited = new bool[arr.Length];
            List<int> step = new List<int>();
            step.Add(start);
            visited[start] = true;
            while (step.Count > 0)
            {
                List<int> nextStep = new List<int>();
                foreach (int idx in step)
                {
                    if (idx == arr.Length - 1) return count;

                    if (idx - 1 >= 0 && !visited[idx - 1])
                    {
                        visited[idx - 1] = true;
                        nextStep.Add(idx - 1);
                    }

                    if (idx + 1 < arr.Length && !visited[idx + 1])
                    {
                        visited[idx + 1] = true;
                        nextStep.Add(idx + 1);
                    }

                    foreach (int ni in valMap[arr[idx]])
                    {
                        if (!visited[ni])
                        {
                            visited[ni] = true;
                            nextStep.Add(ni);
                        }
                    }

                    valMap[arr[idx]] = new List<int>();
                }

                count++;
                step = nextStep;
            }

            return -1;
        }

        public int[] TwoSum(int[] nums, int target)
        {
            var tuples = nums.Select((n, i) => Tuple.Create(n, i)).OrderBy(t => t.Item1).ToArray();
            int l = 0;
            int r = nums.Length - 1;
            while (l < r)
            {
                int sum = tuples[l].Item1 + tuples[r].Item1;
                if (sum == target)
                {
                    return new[] {tuples[l].Item2, tuples[r].Item2};
                }
                else if (sum > target)
                {
                    r--;
                }
                else
                {
                    l++;
                }
            }

            return null;
        }

        // 15 - https://leetcode.com/problems/3sum/
        public IList<IList<int>> ThreeSum(int[] nums)
        {
            List<IList<int>> ret = new List<IList<int>>();
            nums = nums.OrderBy(n => n).ToArray();
            for (int i = 0; i < nums.Length - 2; i++)
            {
                if (i > 0 && nums[i - 1] == nums[i]) continue;

                int l = i + 1;
                int r = nums.Length - 1;
                while (l < r)
                {
                    int sum = nums[l] + nums[r];
                    if (nums[i] + sum == 0)
                    {
                        ret.Add(new[] {nums[i], nums[l], nums[r]});
                        l++;
                        while (r > l && nums[l - 1] == nums[l])
                        {
                            l++;
                        }

                        r--;
                        while (r > l && nums[r + 1] == nums[r])
                        {
                            r--;
                        }
                    }
                    else if (nums[i] + sum > 0)
                    {
                        r--;
                    }
                    else
                    {
                        l++;
                    }
                }
            }

            return ret;
        }

        public int ThreeSumClosest(int[] nums, int target)
        {
            int closets = nums[0] + nums[1] + nums[2];
            nums = nums.OrderBy(n => n).ToArray();

            for (int i = 0; i < nums.Length - 2; i++)
            {
                int l = i + 1;
                int r = nums.Length - 1;
                while (l < r)
                {
                    int sum = nums[l] + nums[r] + nums[i];

                    if (Math.Abs(target - sum) < Math.Abs(target - closets))
                    {
                        closets = sum;
                    }

                    if (sum == target)
                    {
                        return 0;
                    }
                    else if (sum > target)
                    {
                        r--;
                    }
                    else
                    {
                        l++;
                    }
                }
            }

            return closets;
        }

        // https://leetcode.com/problems/single-element-in-a-sorted-array/
        public int SingleNonDuplicate(int[] nums)
        {
            int l = 0;
            int r = nums.Length - 1; // r % 2 == 0
            while (l < r)
            {
                int mid = (l + r) / 2; // This cannot be r, since in that case l >= r.
                if (mid % 2 == 1) mid = mid - 1; // => mid % 2 == 0

                // Result only appears in i % 2 == 0
                if (nums[mid] != nums[mid + 1])
                {
                    r = mid;
                }
                else
                {
                    l = mid + 2;
                }
            }

            return nums[r];
        }

        // 493 - https://leetcode.com/problems/reverse-pairs/
        public int ReversePairs(int[] nums)
        {
            return Divide(nums, 0, nums.Length - 1);
        }

        private int Divide(int[] nums, int l, int r)
        {
            if (l >= r) return 0;

            int mid = (l + r) / 2;

            // 注意mid+1的必须有，否则会爆栈
            int ret = Divide(nums, l, mid) + Divide(nums, mid + 1, r);

            // REVIEW: 分治法，用有序的两个子数组，通过游标就能确定逆序对的数量
            int li = l;
            int ji = mid + 1;
            while (li < mid + 1)
            {
                while (ji <= r && nums[li] > (long) nums[ji] * 2) ji++;
                ret += (ji - mid - 1);
                li++;
            }

            List<int> vals = nums.Skip(l).Take(r - l + 1).OrderBy(v => v).ToList();
            for (int i = 0; i < vals.Count; i++)
            {
                nums[i + l] = vals[i];
            }

            // Console.WriteLine($"FROM:{l}, TO:{r}, RET: {ret}");

            return ret;
        }

        // https://leetcode.com/problems/remove-k-digits/
        public string RemoveKdigits(string num, int k)
        {
            // REVIEW: Idea: Remove the first digit that is the last of a increasing sequence.
            // It is in higher digit pos than digits after but could make it smaller than digits before.
            // (Remove the first peak digit)
            Stack<char> s = new Stack<char>();
            s.Push('0');

            foreach (char c in num)
            {
                // Console.WriteLine(new string(s.Reverse().ToArray()));

                if (c >= s.Peek())
                {
                    s.Push(c);
                }
                else
                {
                    while (s.Peek() > c && k > 0)
                    {
                        s.Pop();
                        k--;
                    }

                    s.Push(c);
                }
            }

            while (k > 0)
            {
                s.Pop();
                k--;
            }

            var ret = (new string(s.Reverse().ToArray())).TrimStart('0');
            return string.IsNullOrEmpty(ret) ? "0" : ret;
        }

        // https://leetcode.com/problems/create-maximum-number/
        public int[] MaxNumber(int[] nums1, int[] nums2, int k)
        {
            // REVIEW: 分解成3个问题：1.枚举从两个数组分别取几个（用单调栈），2.拼接最大数字，3.计算最大值

            // TODO: Finish this before 5/18

            return null;
        }

        public int[] FindMaxSubseqFrom(int size, int[] nums)
        {
            Stack<int> s = new Stack<int>();
            for (int i = 0; i < nums.Length; i++)
            {
                while (s.Count > 0 && nums.Length - i > size - s.Count && s.Peek() < nums[i])
                {
                    // 这里是特殊的，因为只要前面的位尽可能大整个数字就更大，所以即使后面的数字会小也没关系，只要能填满就行。
                    s.Pop();
                }

                if (s.Count < size)
                {
                    s.Push(nums[i]);
                }
            }

            return s.Reverse().ToArray();
        }

        // https://leetcode.com/problems/maximum-sum-circular-subarray/
        public int MaxSubarraySumCircular(int[] A)
        {
            // REVIEW: 思路：寻找大的子数组和最小的子数组
            int sum = 0;
            int min = int.MaxValue;
            int max = int.MinValue;
            int curMin = 0;
            int curMax = 0;
            foreach (int num in A)
            {
                // Find the max of cont. values.
                curMax = Math.Max(curMax + num, num);
                max = Math.Max(max, curMax);

                // Find the min of cont. values.
                curMin = Math.Min(curMin + num, num);
                min = Math.Min(min, curMin);

                sum += num;
            }

            return A.All(a => a <= 0) ? max : Math.Max(max, sum - min);
        }

        // https://leetcode.com/problems/longest-repeating-character-replacement/
        public int CharacterReplacement(string s, int k)
        {
            if (k + 1 >= s.Length) return s.Length;

            // REVIEW: 扩大滑动窗口
            int[] cnts = new int[26];
            int r = -1;
            int max = 1 + k;
            for (int l = 0; l < s.Length && r < s.Length; l++)
            {
                Console.WriteLine($"MOVE L: {l}");
                while (r < s.Length)
                {
                    int len = r - l + 1;
                    bool f = false;
                    for (int i = 0; i < 26; i++)
                    {
                        if (cnts[i] >= len - k)
                        {
                            f = true;
                            break;
                        }
                    }

                    Console.WriteLine($"MOVE R: {r}, LEN: {len}, F: {f}");

                    if (f)
                    {
                        max = Math.Max(max, len);
                        r++;

                        if (r < s.Length)
                        {
                            cnts[s[r] - 'A']++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                cnts[s[l] - 'A']--;
            }

            return max;
        }

        public bool CheckInclusion(string s1, string s2)
        {
            int[] cnts = new int[26];
            foreach (char c in s1)
            {
                cnts[c - 'a']++;
            }

            int[] cnta = new int[26];
            int r = -1;
            for (int l = 0; l <= s2.Length - s1.Length; l++)
            {
                while (r - l + 1 < s1.Length && r < s2.Length)
                {
                    r++;
                    cnta[s2[r] - 'a']++;
                }

                if (r >= s2.Length) break;

                bool f = true;
                for (int i = 0; i < 26; i++)
                {
                    if (cnta[i] != cnts[i])
                    {
                        f = false;
                        break;
                    }
                }

                if (f)
                {
                    return true;
                }
                else
                {
                    cnta[s2[l] - 'a']--;
                }
            }

            return false;
        }

        public int[][] Insert(int[][] intervals, int[] newInterval)
        {
            if (intervals.Length == 0) return new[] {newInterval};
            if (newInterval[1] < intervals[0][0]) return new List<int[]> {newInterval}.Concat(intervals).ToArray();
            if (newInterval[0] > intervals.Last()[1]) return intervals.Concat(new List<int[]> {newInterval}).ToArray();

            List<int[]> ret = new List<int[]>();
            bool inserted = false;
            bool foundRight = false;
            int insertIdx = -1;
            for (int i = 0; i < intervals.Length; i++)
            {
                if (!inserted)
                {
                    // case 1 : merge with left overlap.
                    if (intervals[i][0] <= newInterval[0] && intervals[i][1] >= newInterval[0])
                    {
                        inserted = true;
                        if (intervals[i][1] >= newInterval[1])
                        {
                            // done.
                            foundRight = true;
                        }
                        else
                        {
                            // start finding right boundary.
                            insertIdx = i;
                            intervals[i][1] = newInterval[1]; // in case this is the last element.
                        }
                    }
                    // case 2 : no overlap.
                    else if (intervals[i][0] > newInterval[1])
                    {
                        inserted = true;
                        foundRight = true;
                        ret.Add(newInterval);
                    }
                    // case 3 : right overlap.
                    else if (intervals[i][0] <= newInterval[1] && intervals[i][1] >= newInterval[1])
                    {
                        // since this is not covered by case 1, there's no left overlaps.
                        intervals[i][0] = Math.Min(newInterval[0], intervals[i][0]);
                        inserted = true;
                        foundRight = true;
                    }
                    // case 4 : cover
                    else if (intervals[i][0] >= newInterval[0] && intervals[i][1] <= newInterval[1])
                    {
                        inserted = true;
                        foundRight = intervals[i][1] == newInterval[1];
                        intervals[i][0] = newInterval[0];
                        intervals[i][1] = newInterval[1];
                        if (!foundRight) insertIdx = i;
                    }
                    // case 5 : smaller
                    else if (intervals[i][0] <= newInterval[0] && intervals[i][1] >= newInterval[1])
                    {
                        inserted = true;
                        foundRight = true;
                    }
                }
                else if (!foundRight && inserted)
                {
                    if (intervals[i][1] >= newInterval[1])
                    {
                        foundRight = true;
                        if (intervals[i][0] > newInterval[1])
                        {
                            intervals[insertIdx][1] = newInterval[1];
                        }
                        else
                        {
                            intervals[insertIdx][1] = intervals[i][1];
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                ret.Add(intervals[i]);
            }

            return ret.ToArray();
        }

        // 901 - https://leetcode.com/problems/online-stock-span/
        public class StockSpanner
        {
            // REVIEW: 吃掉前面所有比我大的数的Span，因为我是最大的以后的人不需要再check我。
            private readonly Stack<Span> _spans = new Stack<Span>();

            public StockSpanner()
            {
            }

            public int Next(int price)
            {
                int sp = 1;
                while (this._spans.Count > 0 && this._spans.Peek().Price <= price)
                {
                    // This span will never be visited since the current price is higher and later.
                    sp += this._spans.Pop().SpanVal;
                }

                this._spans.Push(new Span {Price = price, SpanVal = sp});
                return sp;
            }

            private class Span
            {
                public int Price { get; set; }
                public int SpanVal { get; set; }
            }
        }

        // https://leetcode.com/problems/132-pattern/
        public bool Find132pattern(int[] nums)
        {
            int thirdNum = int.MinValue; // the number '2'
            Stack<int> secNums = new Stack<int>();
            foreach (int num in nums.Reverse())
            {
                if (num < thirdNum && secNums.Any())
                {
                    return true;
                }

                while (secNums.Any() && secNums.Peek() < num)
                {
                    thirdNum = Math.Max(thirdNum, secNums.Pop());
                }

                secNums.Push(num);
            }

            return false;
        }

        // https://leetcode.com/problems/next-greater-element-i/
        public int[] NextGreaterElement1(int[] nums1, int[] nums2)
        {
            Dictionary<int, int> lookup = new Dictionary<int, int>();
            Stack<int> rbs = new Stack<int>();
            for (int i = nums2.Length - 1; i >= 0; i--)
            {
                while (rbs.Count > 0 && rbs.Peek() < nums2[i])
                {
                    rbs.Pop();
                }

                lookup.Add(nums2[i], rbs.Count == 0 ? -1 : rbs.Peek());
                rbs.Push(nums2[i]);
            }

            return nums1.Select(n => lookup[n]).ToArray();
        }

        public int[] NextGreaterElements(int[] nums)
        {
            Stack<int> rbs = new Stack<int>();
            int[] res = new int[nums.Length];
            for (int i = nums.Length * 2 - 1; i >= 0; i--)
            {
                int ri = i % nums.Length;
                while (rbs.Count > 0 && rbs.Peek() <= nums[ri])
                {
                    rbs.Pop();
                }

                res[ri] = rbs.Count == 0 ? -1 : rbs.Peek();
                rbs.Push(nums[ri]);
            }

            return res;
        }

        public int NextGreaterElement(int n)
        {
            int input = n;

            List<int> nums = new List<int>();
            while (n > 0)
            {
                if (nums.Count > 0 && n % 10 < nums.Last())
                {
                    int minGreater = nums.Where(nu => nu > n % 10).Min();
                    nums.Remove(minGreater);
                    nums.Add(n % 10);

                    n -= n % 10;
                    n += minGreater;
                    nums = nums.OrderBy(nu => nu).ToList();
                    for (int i = 0; i < nums.Count; i++)
                    {
                        n *= 10;
                        n += nums[i];
                    }

                    return n > input ? n : -1; // In corner cases n might be smaller due to overflow.
                }

                nums.Add(n % 10);
                n /= 10;
            }

            return -1;
        }

        // https://leetcode.com/problems/daily-temperatures/
        public int[] DailyTemperatures(int[] T)
        {
            // stack of <temp, index>
            Stack<Tuple<int, int>> s = new Stack<Tuple<int, int>>();
            int[] ret = new int[T.Length];
            for (int i = ret.Length - 1; i >= 0; i--)
            {
                while (s.Count > 0 && s.Peek().Item1 <= T[i])
                {
                    s.Pop();
                }

                ret[i] = s.Count > 0 ? s.Peek().Item2 - i : 0;
                s.Push(Tuple.Create<int, int>(T[i], i));
            }

            return ret;
        }

        // https://leetcode.com/problems/find-median-from-data-stream/
        public class MedianFinder
        {
            // Min heap for bigger numbers.
            private MaxHeap<MinHeapNode> _minHeap = new MaxHeap<MinHeapNode>(1000);

            // Max heap for smaller numbers.
            private MaxHeap<MaxHeapNode> _maxHeap = new MaxHeap<MaxHeapNode>(1000);

            /** initialize your data structure here. */
            public MedianFinder()
            {
            }

            public void AddNum(int num)
            {
                // always ensure maxHeap.Count == minHeap.Count or maxHeap.Count == minHeap.Count + 1

                if (this._maxHeap.Count == this._minHeap.Count)
                {
                    if (this._maxHeap.Count == 0 || num <= this._minHeap.GetMax().Val)
                    {
                        this._maxHeap.Insert(new MaxHeapNode(num));
                    }
                    else
                    {
                        // num > minHeap.min: pop one node from min to max heap.
                        MinHeapNode minTopNode = this._minHeap.GetMax();
                        this._maxHeap.Insert(new MaxHeapNode(minTopNode.Val));
                        this._minHeap.RemoveMax();
                        this._minHeap.Insert(new MinHeapNode(num));
                    }
                }
                else if (this._maxHeap.Count == this._minHeap.Count + 1)
                {
                    if (num >= this._maxHeap.GetMax().Val)
                    {
                        this._minHeap.Insert(new MinHeapNode(num));
                    }
                    else
                    {
                        MaxHeapNode maxTopNode = this._maxHeap.GetMax();
                        this._minHeap.Insert(new MinHeapNode(maxTopNode.Val));
                        this._maxHeap.RemoveMax();
                        this._maxHeap.Insert(new MaxHeapNode(num));
                    }
                }
                else
                {
                    throw new Exception("DATA ISSUE!");
                }
            }

            public double FindMedian()
            {
                if (this._maxHeap.Count == 0)
                {
                    return 0;
                }
                else if (this._maxHeap.Count == this._minHeap.Count)
                {
                    return (double) (this._maxHeap.GetMax().Val + this._minHeap.GetMax().Val) / 2;
                }
                else if (this._maxHeap.Count == this._minHeap.Count + 1)
                {
                    return this._maxHeap.GetMax().Val;
                }
                else
                {
                    throw new Exception("DATA ISSUE!");
                }
            }

            private class MaxHeapNode : IHeapNode
            {
                public int Val { get; }

                public MaxHeapNode(int val)
                {
                    this.Val = val;
                }

                public int GetValue()
                {
                    return this.Val;
                }
            }

            private class MinHeapNode : IHeapNode
            {
                public int Val { get; }

                public MinHeapNode(int val)
                {
                    this.Val = val;
                }

                public int GetValue()
                {
                    return -this.Val;
                }
            }
        }

        // https://leetcode.com/problems/interval-list-intersections/
        public int[][] IntervalIntersection(int[][] A, int[][] B)
        {
            List<int[]> result = new List<int[]>();

            int ai = 0;
            int bi = 0;
            while (ai < A.Length && bi < B.Length)
            {
                if (A[ai][1] < B[bi][0])
                {
                    ai++;
                }
                else if (A[ai][0] > B[bi][1])
                {
                    bi++;
                }
                else
                {
                    int[] overlap = new[] {Math.Max(A[ai][0], B[bi][0]), Math.Min(A[ai][1], B[bi][1])};
                    result.Add(overlap);
                    if (A[ai][1] < B[bi][1])
                    {
                        ai++;
                    }
                    else
                    {
                        bi++;
                    }
                }
            }

            return result.ToArray();
        }

        // https://leetcode.com/problems/course-schedule-iii/solution/
        public int ScheduleCourse(int[][] courses)
        {
            // 贪心法：尽可能使得当前耗时小
            List<int> costList = new List<int>();
            int currTime = 0;

            // Pick the courses with early due date first.
            foreach (int[] course in courses.OrderBy(c => c[1]))
            {
                if (currTime + course[0] <= course[1])
                {
                    currTime += course[0];
                    costList.Add(course[0]);
                }
                else
                {
                    // 尝试替换掉当前耗时最长的课程以降低总开销...
                    int maxIdx = 0;
                    for (int i = 1; i < costList.Count; i++)
                    {
                        if (costList[i] > costList[maxIdx])
                        {
                            maxIdx = i;
                        }
                    }

                    if (costList[maxIdx] > course[0])
                    {
                        currTime -= (costList[maxIdx] - course[0]);
                        costList[maxIdx] = course[0];
                    }
                }
            }

            return costList.Count;
        }

        // https://leetcode.com/problems/rotate-array/
        public void Rotate(int[] nums, int k)
        {
            // REVIEW: It could be proved that if n % k != 0, the nested loop will cover all the numbers once...

            int count = 0;
            k = k % nums.Length;
            for (int i = 0; count < nums.Length; i++)
            {
                int idx = (i + k) % nums.Length;
                int prev = nums[i];
                while (idx != i)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(nums));


                    int tmp = nums[idx];
                    nums[idx] = prev;
                    prev = tmp;

                    count++;
                    idx = (idx + k) % nums.Length;
                }

                nums[idx] = prev;
                count++;
            }
        }

        // https://leetcode.com/problems/palindrome-partitioning/
        public IList<IList<string>> Partition(string s)
        {
            List<string>[] memo = new List<string>[s.Length];
            return this.PalindromePartitioning(s, 0, memo)
                .Select(ms => (IList<string>) ms.Split('|', StringSplitOptions.RemoveEmptyEntries))
                .ToList();
        }

        public List<string> PalindromePartitioning(string s, int idx, List<string>[] memo)
        {
            if (memo[idx] != null)
            {
                return memo[idx];
            }

            List<string> result = new List<string>();
            for (int len = 1; len <= s.Length - idx; len++)
            {
                if (this.IsPalindrome(s, idx, len))
                {
                    int nextBegin = idx + len;
                    if (nextBegin == s.Length)
                    {
                        result.Add(s.Substring(idx));
                    }
                    else
                    {
                        string curPs = s.Substring(idx, len);
                        List<string> subPs = this.PalindromePartitioning(s, idx + len, memo);
                        foreach (string p in subPs)
                        {
                            result.Add($"{curPs}|{p}");
                        }
                    }
                }
            }

            memo[idx] = result;
            return result;
        }

        private bool IsPalindrome(string s, int idx, int len)
        {
            if (len == 1)
            {
                return true;
            }

            for (int i = 0; i < len / 2; i++)
            {
                if (s[idx + i] != s[idx + len - i - 1])
                {
                    return false;
                }
            }

            return true;
        }

        // https://leetcode.com/problems/palindrome-partitioning-ii/
        public int MinCut(string s)
        {
            bool[,] p = new bool [s.Length, s.Length];
            int[] dp = new int[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                dp[i] = i;
                for (int j = 0; j <= i; j++)
                {
                    if (s[i] == s[j] && (i - j < 2 || p[i - 1, j + 1]))
                    {
                        Console.WriteLine($"{j},{i} > true");

                        p[i, j] = true;
                        dp[i] = j == 0 ? 0 : Math.Min(dp[i], dp[j - 1] + 1);
                    }
                }
            }

            Console.WriteLine(string.Join(",", dp));
            return dp[s.Length - 1];
        }


        // https://leetcode.com/problems/palindromic-substrings/
        public int CountSubstrings(string s)
        {
            int cnt = 0;
            bool[,] p = new bool [s.Length, s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if (s[i] == s[j] && (i - j < 2 || p[i - 1, j + 1]))
                    {
                        p[i, j] = true;
                        cnt++;
                    }
                }
            }

            return cnt;
        }

        // https://leetcode.com/problems/substring-with-concatenation-of-all-words/
        public IList<int> FindSubstring(string s, string[] words)
        {
            List<int> ret = new List<int>();
            if (s.Length == 0 || words.Length == 0) return ret;

            Dictionary<string, int> cnts = new Dictionary<string, int>();
            foreach (string word in words)
            {
                if (cnts.ContainsKey(word)) cnts[word]++;
                else cnts[word] = 1;
            }

            // readonly
            int wLen = words[0].Length;
            int tCnt = cnts.Count; // Unique word count.

            for (int offset = 0; offset < wLen; offset++)
            {
                int zCnt = 0; // Zero count
                int nextIdx = offset;

                Dictionary<string, int> curCnts = new Dictionary<string, int>(cnts);
                Queue<string> q = new Queue<string>();
                while (nextIdx <= s.Length - wLen)
                {
                    string curWord = s.Substring(nextIdx, wLen);

                    if (!curCnts.ContainsKey(curWord))
                    {
                        // REVIEW: 为了维护zCnt，跳过不存在的词组
                        // Skip 'til next word.
                        q.Clear();
                        zCnt = 0;
                        curCnts = new Dictionary<string, int>(cnts);
                    }
                    else
                    {
                        if (q.Count == words.Length)
                        {
                            string dequeueWord = q.Dequeue();
                            int dequeueWordCnt = curCnts[dequeueWord];
                            curCnts[dequeueWord] = dequeueWordCnt + 1;

                            if (dequeueWordCnt + 1 == 0)
                            {
                                // dequeue-ed unnecessary word.
                                zCnt++;
                            }
                            else if (dequeueWordCnt == 0)
                            {
                                zCnt--;
                            }

                            // Console.WriteLine($"DEQUEUE: {dequeueWord}, ZCNT: {zCnt}");
                        }

                        q.Enqueue(curWord);
                        int wCnt = curCnts[curWord];
                        curCnts[curWord] = wCnt - 1;
                        if (wCnt - 1 == 0)
                        {
                            // me to zero.
                            zCnt++;

                            if (zCnt == tCnt) // all words found.
                            {
                                ret.Add(nextIdx - (words.Length - 1) * wLen);
                            }
                        }
                        else if (wCnt == 0)
                        {
                            zCnt--;
                        }

                        // Console.WriteLine($"ENQUEUE: {curWord}, ZCNT: {zCnt}");
                    }

                    nextIdx += wLen;
                }
            }

            return ret;
        }

        // https://leetcode.com/problems/queue-reconstruction-by-height/
        public int[][] ReconstructQueue(int[][] people)
        {
            List<int[]> result = new List<int[]>();
            List<int[]> pSorted = people
                .OrderByDescending(p => p[0])
                .ThenBy(p => p[1]) // 确保前面有多人的情况能插在更前面（后插的能够更前）
                .ToList();
            foreach (int[] p in pSorted)
            {
                result.Insert(p[1], p);
            }

            return result.ToArray();
        }

        public int UniqueLetterString(string s)
        {
            // REVIEW: 计算每个字符单独能出现的字串数量 (前一个同样字符 - 后一个同样字符)，因为每个不同的字母都相当于是单独计数的！
            Dictionary<char, List<int>> indices = new Dictionary<char, List<int>>();
            for (int i = 0; i < s.Length; i++)
            {
                if (indices.TryGetValue(s[i], out List<int> ind)) ind.Add(i);
                else indices.Add(s[i], new List<int> {i});
            }

            // REVIEW: 计算数量的方式
            long count = 0;
            foreach (List<int> idx in indices.Values)
            {
                for (int i = 0; i < idx.Count; i++)
                {
                    int l = (i == 0 ? 0 : idx[i - 1] + 1);
                    int r = (i == idx.Count - 1 ? s.Length - 1 : idx[i + 1] - 1);
                    count += (idx[i] - l + 1) * (r - idx[i] + 1); // 0 - 2, 1
                }
            }

            return (int) (count % 1000000007);
        }

        public void SortColors(int[] nums)
        {
            int zp = 0;
            int tp = nums.Length - 1;
            int i = 0;

            while (i < nums.Length)
            {
                if (nums[i] == 0 && zp < i)
                {
                    while (zp < i && nums[zp] == 0) zp++;
                    if (zp < i)
                    {
                        int tmp = nums[zp];
                        nums[zp] = nums[i];
                        nums[i] = tmp;
                    }
                }
                else if (nums[i] == 2 && tp > i)
                {
                    while (tp > i && nums[tp] == 2) tp--;
                    if (tp > i)
                    {
                        int tmp = nums[tp];
                        nums[tp] = nums[i];
                        nums[i] = tmp;
                    }
                }
                else
                {
                    i++;
                }
            }
        }

        // https://leetcode.com/problems/insert-delete-getrandom-o1/
        public class RandomizedSet
        {
            private int _realCount = 0;
            private List<int> _nums = new List<int>();
            private Dictionary<int, int> _indexMap = new Dictionary<int, int>();
            private Random _random = new Random();

            /** Initialize your data structure here. */
            public RandomizedSet()
            {

            }

            /** Inserts a value to the set. Returns true if the set did not already contain the specified element. */
            public bool Insert(int val)
            {
                if (!this._indexMap.ContainsKey(val))
                {
                    int index = this._realCount;
                    if (this._nums.Count > index)
                    {
                        this._nums[index] = val;
                    }
                    else
                    {
                        this._nums.Add(val);
                    }

                    this._indexMap[val] = index;
                    this._realCount++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /** Removes a value from the set. Returns true if the set contained the specified element. */
            public bool Remove(int val)
            {
                if (!this._indexMap.TryGetValue(val, out int index))
                {
                    return false;
                }
                else
                {
                    this._indexMap.Remove(val);
                    this._realCount--;

                    int lastIndex = this._realCount;
                    if (lastIndex != index)
                    {
                        this._nums[index] = this._nums[lastIndex];
                        this._indexMap[this._nums[index]] = index;
                    }

                    return true;
                }
            }

            /** Get a random element from the set. */
            public int GetRandom()
            {
                if (this._realCount == 0)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    int index = this._random.Next(this._realCount);
                    return this._nums[index];
                }
            }
        }

        // https://leetcode.com/problems/insert-delete-getrandom-o1-duplicates-allowed/
        public class RandomizedCollection
        {
            private int _realCount = 0;
            private List<int> _nums = new List<int>();
            private Dictionary<int, List<int>> _indexMap = new Dictionary<int, List<int>>();
            private Random _random = new Random();

            /** Initialize your data structure here. */
            public RandomizedCollection()
            {
            }

            /** Inserts a value to the collection. Returns true if the collection did not already contain the specified element. */
            public bool Insert(int val)
            {
                int index = this._realCount;
                this._realCount++;
                if (this._nums.Count > index)
                {
                    this._nums[index] = val;
                }
                else
                {
                    this._nums.Add(val);
                }

                if (!this._indexMap.TryGetValue(val, out List<int> indices))
                {
                    this._indexMap[val] = new List<int> {index};
                    return true;
                }
                else
                {
                    indices.Add(index);
                    return false;
                }
            }

            /** Removes a value from the collection. Returns true if the collection contained the specified element. */
            public bool Remove(int val)
            {
                if (this._indexMap.TryGetValue(val, out List<int> indices) && indices.Count > 0)
                {
                    int index = indices[indices.Count - 1];
                    indices.RemoveAt(indices.Count - 1);
                    this._realCount--;
                    int lastIndex = this._realCount;
                    if (lastIndex != index)
                    {
                        this._nums[index] = this._nums[lastIndex];
                        List<int> repIndices = this._indexMap[this._nums[index]];
                        for (int i = 0; i < repIndices.Count; i++)
                        {
                            if (repIndices[i] == lastIndex)
                            {
                                repIndices[i] = index;
                                break;
                            }
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

            /** Get a random element from the collection. */
            public int GetRandom()
            {
                if (this._realCount == 0)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    int index = this._random.Next(this._realCount);
                    return this._nums[index];
                }
            }
        }

        public string ValidIPAddress(string IP)
        {
            Dictionary<char, int> hexLetters = new Dictionary<char, int>
            {
                ['a'] = 10,
                ['A'] = 10,
                ['b'] = 11,
                ['B'] = 11,
                ['c'] = 12,
                ['C'] = 12,
                ['d'] = 13,
                ['D'] = 13,
                ['e'] = 14,
                ['E'] = 14,
                ['f'] = 15,
                ['F'] = 15,
            };

            if (!string.IsNullOrEmpty(IP))
            {
                bool ipv4 = IP.Count(c => c == '.') == 3;
                bool ipv6 = IP.Count(c => c == ':') == 7;
                if (ipv4 && !ipv6)
                {
                    string[] sections = IP.Split('.', StringSplitOptions.RemoveEmptyEntries);
                    if (sections.Length == 4)
                    {
                        foreach (string section in sections)
                        {
                            if (section.Length > 1 && section[0] == '0') return "Neither";

                            int val = 0;
                            foreach (char c in section)
                            {
                                if (!char.IsDigit(c)) return "Neither";
                                val = val * 10 + c - '0';

                                if (val > 255) return "Neither";
                            }
                        }

                        return "IPv4";
                    }
                }
                else if (!ipv4 && ipv6)
                {
                    string[] sections = IP.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    if (sections.Length == 8)
                    {
                        foreach (string section in sections)
                        {
                            if (section.Length > 4) return "Neither";
                            int val = 0;
                            foreach (char c in section)
                            {
                                if (char.IsDigit(c))
                                {
                                    val = val * 16 + c - '0';
                                }
                                else if (hexLetters.TryGetValue(c, out int v))
                                {
                                    val = val * 16 + v;
                                }
                                else
                                {
                                    return "Neither";
                                }
                            }

                            if (val > 0xffff) return "Neither";
                        }

                        return "IPv6";
                    }
                }
            }

            return "Neither";
        }

        // https://leetcode.com/problems/largest-multiple-of-three/
        public string LargestMultipleOfThree(int[] digits)
        {
            // REVIEW: 各位数字之和能被3整除的数能被3整除，remove least numbers.
            int[] cnts = new int[10];
            long sum = 0;
            int cnt = digits.Length;
            foreach (int digit in digits)
            {
                cnts[digit]++;
                sum += digit;
            }

            while (cnt > 0 && sum % 3 != 0)
            {
                if (sum % 3 == 1)
                {
                    for (int i = 1; i <= 7; i += 3)
                    {
                        if (cnts[i] > 0)
                        {
                            cnts[i]--;
                            sum -= i;
                            break;
                        }
                    }

                    if (sum % 3 == 1)
                    {
                        for (int i = 2; i <= 8; i += 3)
                        {
                            if (cnts[i] > 0)
                            {
                                cnts[i]--;
                                sum -= i;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 2; i <= 8; i += 3)
                    {
                        if (cnts[i] > 0)
                        {
                            cnts[i]--;
                            sum -= i;
                            break;
                        }
                    }

                    if (sum % 3 == 2)
                    {
                        for (int i = 1; i <= 7; i += 3)
                        {
                            if (cnts[i] > 0)
                            {
                                cnts[i]--;
                                sum -= i;
                                break;
                            }
                        }
                    }
                }
            }

            StringBuilder b = new StringBuilder();
            for (int i = 9; i >= 0; i--)
            {
                while (cnts[i] > 0)
                {
                    b.Append(i.ToString());
                    cnts[i]--;
                }
            }

            string ret = b.ToString();
            return ret.Length > 1 && ret[0] == '0' ? "0" : ret;
        }

        // https://leetcode.com/problems/decode-string/submissions/
        public string DecodeString(string s)
        {
            int idx = 0;
            return Decode(1, s, ref idx);
        }

        private string Decode(int factor, string s, ref int idx)
        {
            StringBuilder b = new StringBuilder();
            while (true)
            {
                if (idx == s.Length)
                {
                    return Return(factor, b.ToString());
                }
                else if (s[idx] == ']')
                {
                    idx++;
                    return Return(factor, b.ToString());
                }
                else if (char.IsDigit(s[idx]))
                {
                    int nf = s[idx++] - '0';
                    while (s[idx] != '[')
                    {
                        nf = nf * 10 + s[idx++] - '0';
                    }

                    idx++; // from '['
                    b.Append(Decode(nf, s, ref idx));
                }
                else
                {
                    b.Append(s[idx++]);
                }
            }
        }

        private string Return(int factor, string s)
        {
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < factor; i++) b.Append(s);
            return b.ToString();
        }

        // https://leetcode.com/problems/longest-duplicate-substring/
        public string LongestDupSubstring(string S)
        {
            // REVIEW: 二分查找最大长度的思想 + 字符串编码 Robin-Karp.
            int[] str = S.ToCharArray().Select(c => c - 'a').ToArray();

            int l = 1;
            int r = S.Length - 1;
            while (l < r)
            {
                int m = (l + r) / 2;
                if (LongestDupSubstring(str, m, S) >= 0)
                {
                    l = m + 1;
                }
                else
                {
                    r = m;
                }
            }

            int idx = LongestDupSubstring(str, l, S);
            if (idx >= 0)
            {
                return S.Substring(idx, l);
            }
            else
            {
                idx = LongestDupSubstring(str, l - 1, S);
                return idx >= 0 ? S.Substring(idx, l - 1) : "";
            }
        }

        private int LongestDupSubstring(int[] str, int len, string s)
        {
            // Starting index and their robin-karp decoded values.
            Dictionary<long, int> rkHash = new Dictionary<long, int>();
            long mod = (long) Math.Pow(2, 63);
            long removal = 1;
            for (int i = 1; i < len; i++) removal = removal * 26 % mod;

            long hash = 0;
            for (int i = 0; i < len; i++)
            {
                hash = (hash * 26 + str[i]) % mod;
            }

            rkHash[hash] = 0;
            for (int i = 1; i <= str.Length - len; i++)
            {
                hash = (hash - removal * (str[i - 1])) * 26 + str[i + len - 1];

                if (rkHash.TryGetValue(hash, out int idx))
                {
                    return idx;
                }

                rkHash[hash] = i;
            }

            return -1;
        }

        public string GetPermutation(int n, int k)
        {
            StringBuilder b = new StringBuilder();

            int m = 1; // the count of sub permutations.
            int f = 1; // the last multiply factor.
            for (; f < n; f++) m = m * f;

            k = k - 1; // k starts from 0.
            bool[] used = new bool[n];

            while (b.Length < n)
            {
                f--;

                int ordinal = k / m;
                k = k % m;

                int skip = 0;
                for (int i = 0; i < used.Length; i++)
                {
                    if (!used[i])
                    {
                        if (skip == ordinal)
                        {
                            b.Append((i + 1).ToString());
                            used[i] = true;
                            break;
                        }
                        else
                        {
                            skip++;
                        }
                    }
                }

                if (f > 0)
                {
                    m = m / f;
                }
            }

            return b.ToString();
        }

        // https://leetcode.com/problems/next-permutation/
        public void NextPermutation(int[] nums)
        {
            // the idea is to find the right most number that could be replaced by a larger number on it's right, or there's not result.
            for (int i = nums.Length - 1; i >= 1; i--)
            {
                if (nums[i - 1] < nums[i])
                {
                    int numIdx = i - 1;
                    int minIdx = -1;
                    for (int j = i; j < nums.Length; j++)
                    {
                        // Corner case : if use <, may not be increasing. 
                        if (nums[j] <= nums[numIdx])
                        {
                            break;
                        }
                        else
                        {
                            // Find till the last bigger than num
                            minIdx = j;
                        }
                    }

                    // swap
                    int tmp = nums[minIdx];
                    nums[minIdx] = nums[numIdx];
                    nums[numIdx] = tmp;

                    Array.Reverse(nums, i, nums.Length - i);
                    return;
                }
            }

            Array.Reverse(nums);
        }

        // https://leetcode.com/problems/decoded-string-at-index/
        public string DecodeAtIndex(string S, int K)
        {
            int i = 0;
            long len = 0;
            while (len < K)
            {
                if (char.IsDigit(S[i]))
                {
                    len *= (S[i] - '0');
                }
                else
                {
                    len++;
                }

                i++;
            }

            i--;
            while (i >= 0)
            {
                if (char.IsDigit(S[i]))
                {
                    len /= (S[i] - '0');

                    if (K % len == 0)
                    {
                        K = (int) len;
                    }
                    else
                    {
                        K = (int) (K % len);
                    }
                }
                else
                {
                    if (K == len)
                    {
                        return S[i].ToString();
                    }
                    else
                    {
                        len--;
                    }
                }

                i--;
            }

            return null;
        }
        
        // https://leetcode.com/problems/delete-columns-to-make-sorted-ii/
        public int MinDeletionSize(string[] A)
        {
            // REVIEW ME
            
            if (A.Length == 0) return 0;

            int ret = 0;
            int sLen = A[0].Length;

            // marks that the current item has clearly wins the [previous] one.
            bool[] cut = new bool[A.Length];
            for (int i = 0; i < sLen; i++)
            {
                bool canKeep = true;
                for (int j = 1; j < A.Length; j++)
                {
                    if (!cut[j] && A[j][i] < A[j - 1][i])
                    {
                        ret++;

                        // current column cannot used to cut. (to be removed)
                        canKeep = false;
                        break;
                    }
                }

                if (canKeep)
                {
                    for (int j = 1; j < A.Length; j++)
                    {
                        // not a tie
                        if (A[j][i] > A[j - 1][i])
                        {
                            cut[j] = true;
                        }
                    }
                }
            }

            return ret;
        }

        public string ReverseWords(string s)
        {
            List<char> ret = new List<char>();

            bool needASpace = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ')
                {
                    needASpace = ret.Count() > 0;
                }
                else
                {
                    if (needASpace)
                    {
                        ret.Add(' ');
                        needASpace = false;
                    }

                    ret.Add(s[i]);
                }
            }

            int l = 0;
            for (int i = 0; i < ret.Count(); i++)
            {
                if (i + 1 == ret.Count() || ret[i + 1] == ' ')
                {
                    int r = i;
                    for (int k = 0; k < (r - l + 1) / 2; k++)
                    {
                        char t = ret[k + l];
                        ret[k + l] = ret[r - k];
                        ret[r - k] = t;
                    }

                    l = r + 2;
                }
            }

            return new string(ret.Where(c => true).Reverse().ToArray());
        }

        public int EraseOverlapIntervals(int[][] intervals)
        {
            intervals = intervals.OrderBy(iv => iv[0]).ToArray();

            int cnt = 0;
            int prev = 0;
            for (int i = 1; i < intervals.Length; i++)
            {
                if(intervals[i][0] < intervals[prev][1])
                {
                    cnt++;
                    if (intervals[i][1] < intervals[prev][1])
                    {
                        prev = i;
                    }
                }
                else
                {
                    prev = i;
                }
            }

            return cnt;
        }

        public void Run()
        {
            Console.WriteLine(ReverseWords("the sky is blue"));
        }
    }
}