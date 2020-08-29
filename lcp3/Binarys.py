from pprint import pprint
import collections

# https://leetcode.com/problems/time-based-key-value-store/
class TimeMap:

    def __init__(self):
        self.map = collections.defaultdict(list)

    def set(self, key: str, value: str, timestamp: int) -> None:
        self.map[key].append((timestamp, value))

    def get(self, key: str, timestamp: int) -> str:
        t = self.map.get(key, None)
        if t is None: return ""
        elif t[-1][0] <= timestamp: return t[-1][1]
        else:
            l = 0
            r = len(t) - 1
            while l < r:
                m = l + (r - l) // 2
                if t[m][0] <= timestamp:
                    l = m + 1 # make sure we can break the loop
                else:
                    r = m - 1
            return t[l-1][1] if l and t[l-1][0] <= timestamp else ""

if __name__ == "__main__":
    map = collections.defaultdict(list)
    map.get("123").append(2)
    pprint(map.get("123"))