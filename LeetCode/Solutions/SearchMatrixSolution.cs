namespace LeetCode.Csharp.Solutions
{
    public class SearchMatrixSolution
    {
        // 240 - https://leetcode.com/problems/search-a-2d-matrix-ii/
        public bool SearchMatrix(int[,] matrix, int target)
        {
            int d2Length = matrix.GetLength(1);
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                if (matrix[x, 0] <= target && matrix[x, d2Length - 1] >= target)
                {
                    int l = 0;
                    int r = d2Length - 1;
                    while (l < r)
                    {
                        int mid = (l + r) / 2;
                        if (matrix[x, mid] < target)
                        {
                            l = mid + 1;
                        }
                        else if (matrix[x, mid] > target)
                        {
                            r = mid;
                        }
                        else
                        {
                            return true;
                        }
                    }

                    if (matrix[x, r] == target) return true;

                    // Console.WriteLine($"X:{x},L:{l},R:{r}");
                }
            }

            return false;
        }
    }
}