from typing import List
from collections import deque, defaultdict
from math import sqrt
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

    # https://leetcode.com/problems/critical-connections-in-a-network/
    # IDEA: 桥边 - 从上一个节点来的边，如果从本节点回溯的最早节点[的层数]等于本节点，就是桥边 - 注意处理回边，因为需要把无向图的边处理成双向边
    def criticalConnections(self, n: int, connections: List[List[int]]) -> List[List[int]]:
        result = []
        graph = defaultdict(list)
        for conn in connections:
            graph[conn[0]].append(conn[1])
            graph[conn[1]].append(conn[0])

        # update canReachLevel
        def dfs(lvl: int, cur: int, parent: int, canReach: List[int]):
            canReach[cur] = lvl # can always reach current node
            for t in graph[cur]:
                if t == parent: continue # ignore reverse edges
                elif canReach[t] < 0: # not yet visited
                    dfs(lvl+1, t, cur, canReach)

                # update even when dfs didn't happen for 't' -> because previous level should have been visited already
                if canReach[t] < canReach[cur]:
                    canReach[cur] = canReach[t]

            if canReach[cur] == lvl and parent >= 0:
                result.append([parent, cur])

        canReach = [-1] * n
        dfs(0, 0, -1, canReach)
        return result

if __name__ == "__main__":
    sol = Solution()
    print(sol.criticalConnections(4, [[0,1],[1,2],[2,0],[1,3]]))