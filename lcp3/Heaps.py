from LcModels import ListNode
from typing import List
from collections import deque, defaultdict
from heapq import heappush, heappop

class Solution:
    # https://leetcode.com/problems/merge-k-sorted-lists/
    # a counter has been used to avoid comparing ListNode types.
    def mergeKLists(self, lists: List[ListNode]) -> ListNode:
        heap = list()
        counter = 0
        for ln in lists:
            if ln is not None:
                heappush(heap, (ln.val, counter, ln))
                counter += 1

        head = None
        prev = None
        while len(heap) > 0:
            _, _, ln = heappop(heap)
            if ln.next is not None:
                heappush(heap, (ln.next.val, counter, ln.next))
                counter += 1

            if head == None:
                head = ln
                prev = ln
            else:
                prev.next = ln
                prev = ln
        return head