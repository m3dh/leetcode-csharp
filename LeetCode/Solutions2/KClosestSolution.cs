namespace LeetCode.Csharp.Solutions2
{
    using LeetCode.Csharp.Common;

    public class KClosestSolution
    {
        public class PointHeapNode : IHeapNode
        {
            private readonly int _x;
            private readonly int _y;

            public int[] GetPoint()
            {
                return new[] {this._x, this._y};
            }

            public PointHeapNode(int x, int y)
            {
                this._x = x;
                this._y = y;
            }

            public int GetValue()
            {
                return 0 - this._x * this._x - this._y * this._y;
            }
        }

        // 973 - https://leetcode.com/problems/k-closest-points-to-origin/
        public int[][] KClosest(int[][] points, int K)
        {
            MaxHeap<PointHeapNode> pn = new MaxHeap<PointHeapNode>(points.Length);
            foreach (int[] point in points)
            {
                pn.Insert(new PointHeapNode(point[0], point[1]));
            }

            int[][] ret = new int[K][];
            for (int i = 0; i < K; i++)
            {
                ret[i] = pn.GetMax().GetPoint();
                pn.RemoveMax();
            }

            return ret;
        }
    }
}