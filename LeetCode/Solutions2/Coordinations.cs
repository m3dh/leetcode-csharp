namespace LeetCode.Csharp.Solutions2
{
    using System.Collections.Generic;
    using System.Linq;

    public class Coordinations
    {
        // https://leetcode.com/problems/perfect-rectangle/
        public bool IsRectangleCover(int[][] rectangles)
        {
            // REVIEW: Idea - 计算面积，且无偶数次重复的顶点应该只有四个.
            Dictionary<string, int[]> nodes = new Dictionary<string, int[]>();
            int area = 0;
            foreach (int[] rect in rectangles)
            {
                // 1,2,2,3
                area += (rect[3] - rect[1]) * (rect[2] - rect[0]);
                if (!nodes.TryAdd($"{rect[0]}-{rect[1]}", new[] { rect[0], rect[1] })) nodes.Remove($"{rect[0]}-{rect[1]}");
                if (!nodes.TryAdd($"{rect[2]}-{rect[3]}", new[] { rect[2], rect[3] })) nodes.Remove($"{rect[2]}-{rect[3]}");
                if (!nodes.TryAdd($"{rect[0]}-{rect[3]}", new[] { rect[0], rect[3] })) nodes.Remove($"{rect[0]}-{rect[3]}");
                if (!nodes.TryAdd($"{rect[2]}-{rect[1]}", new[] { rect[2], rect[1] })) nodes.Remove($"{rect[2]}-{rect[1]}");
            }

            // now that we should have only 4 nodes.
            if (nodes.Count() == 4)
            {
                var nodeArray = nodes.Select(n => n.Value).OrderBy(n => n[0]).ThenBy(n => n[1]).ToArray();
                int[] ll = nodeArray[0]; // 1,2
                int[] lu = nodeArray[1]; // 1,3
                int[] rl = nodeArray[2]; // 2,2
                int[] ru = nodeArray[3]; // 2,3
                if (ll[0] == lu[0] && rl[0] == ru[0] && ll[1] == rl[1] && lu[1] == ru[1] && area == (rl[0] - ll[0]) * (ru[1] - rl[1]))
                {
                    return true;
                }
            }

            return false;
        }

        // https://leetcode.com/problems/sparse-matrix-multiplication/
        public int[][] Multiply(int[][] A, int[][] B)
        {
            // 结果矩阵的大小：[ALen][B[0]Len]
            int[][] result = new int[A.Length][];
            for (int i = 0; i < A.Length; i++)
            {
                result[i] = new int[B[0].Length];
            }

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    if (A[i][j] != 0)
                    {
                        for (int k = 0; k < B[0].Length; k++)
                        {
                            result[i][k] += A[i][j] * B[j][k];
                        }
                    }
                }
            }

            return result;
        }
    }
}
