from typing import List
from collections import deque, defaultdict
from pprint import pprint

class UnionFindSet:
    def __init__(self, size: int):
        self._set = list(range(0, size+1))

    def union(self, a: int, b: int) -> bool:
        fa = self.find(a)
        fb = self.find(b)
        if fa == fb: return False
        else:
            self._set[fa] = fb
            return True

    def find(self, v: int) -> int:
        while self._set[v] != v:
            next = self._set[v]
            self._set[v] = self._set[next]
            v = next
        return v

class Solution:
    # https://leetcode.com/problems/graph-valid-tree/
    # IDEA: 只要是连通所有节点的无环图，就是合法的树 -> 总结：无向图应该用ufs解决问题而不是bfs
    def validTree(self, n: int, edges: List[List[int]]) -> bool:
        if n == 0 or len(edges) != n - 1: return False
        ufs = UnionFindSet(n)
        for edge in edges:
            if not ufs.union(edge[0], edge[1]): return False
        return True
        
if __name__ == "__main__":
    sol = Solution()
    print(sol.validTree(5, [[0,1],[1,2],[2,3],[1,3],[1,4]]))