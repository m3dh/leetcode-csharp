from typing import List
from collections import deque, defaultdict

class Solution:

    # https://leetcode.com/problems/maximum-frequency-stack/
    # 为每个Freq单独维护了一个stack，所以能保证按入栈顺序pop元素！ (无视顺序的话记个数字就行了...)
    class FreqStack:
        def __init__(self):
            self._curMax = 0 # current max frequency
            self._cntMap = defaultdict(int) # map from item to frequency
            self._stkMap = defaultdict(deque) # map from frequency to a stack of items

        def push(self, x: int) -> None:
            self._cntMap[x] += 1
            curCnt = self._cntMap[x]
            self._stkMap[curCnt].append(x)
            self._curMax = max(self._curMax, curCnt)

        def pop(self) -> int:
            while len(self._stkMap[self._curMax]) == 0 and self._curMax > 0:
                self._curMax -= 1

            s = self._stkMap[self._curMax]
            item = s.pop()
            self._cntMap[item] -= 1
            return item

    # https://leetcode.com/problems/longest-continuous-subarray-with-absolute-diff-less-than-or-equal-to-limit/
    # 通过维护两个单调栈 + 队列，来实现 O(n) 复杂度 (滑动窗口)
    def longestSubarray(self, nums: List[int], limit: int) -> int:
        maxLen = 0
        l = 0
        r = 0
        while r < len(nums):
            maxLen = max(maxLen, r-l)

    # https://leetcode.com/problems/longest-repeating-character-replacement/
    def characterReplacement(self, s: str, k: int) -> int:
        maxLen = 0
        r = 0
        cnts = defaultdict(int)
        for l in range(len(s)):
            while r <= len(s):
                curLen = r - l
                isValid = False
                for i in range(26):
                    if cnts[i] >= curLen - k:
                        isValid = True
                        break

                if isValid and curLen > maxLen: maxLen = curLen
                if not isValid: break
                if r < len(s):
                    cnts[ord(s[r]) - ord('A')] += 1

                r += 1 # Ensures that we can break from this
            cnts[ord(s[l]) - ord('A')] -= 1
        return maxLen

if __name__ == "__main__":
    pass