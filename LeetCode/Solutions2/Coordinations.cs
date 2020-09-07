namespace LeetCode.Csharp.Solutions2
{
    using System;
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

        // https://leetcode.com/problems/image-overlap/
        public int LargestOverlap(int[][] A, int[][] B)
        {
            int m = A.Length;
            int n = A[0].Length;
            
            // move [1-n, n-1]
            int max = 0;
            for (int xd = 1 - n; xd <= n - 1; xd++)
            {
                for (int yd = 1 - m; yd <= m - 1; yd++)
                {
                    int count = 0;
                    for (int i = 0; i < n; i++)
                    {
                        int xt = i - xd;
                        if (xt < 0 || xt >= n) continue;
                        for (int j = 0; j < m; j++)
                        {
                            int yt = j - yd;
                            if (yt >= 0 && yt < m && A[xt][yt] + B[i][j] == 2)
                            {
                                count++;
                            }
                        }
                    }

                    max = Math.Max(max, count);
                }
            }

            return max;
        }
    }
}
