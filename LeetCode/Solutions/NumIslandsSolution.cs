namespace LeetCode.Csharp.Solutions
{
    public class NumIslandsSolution
    {
        // 200 - https://leetcode.com/problems/number-of-islands/
        public int NumIslands(char[][] grid)
        {
            bool[][] search = new bool[grid.Length][];
            for (int i = 0; i < grid.Length; i++)
            {
                search[i] = new bool[grid[i].Length];
            }

            int counter = 0;
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[0].Length; j++)
                {
                    if (grid[i][j] == '1' && !search[i][j])
                    {
                        counter++;
                        MarkNewIsland(i, j, search, grid);
                    }
                }
            }

            return counter;
        }

        private void MarkNewIsland(int x, int y, bool[][] search, char[][] grid)
        {
            if (x >= 0 && x < grid.Length && y >= 0 && y < grid[0].Length)
            {
                if (!search[x][y] && grid[x][y] == '1')
                {
                    search[x][y] = true;
                    this.MarkNewIsland(x - 1, y, search, grid);
                    this.MarkNewIsland(x + 1, y, search, grid);
                    this.MarkNewIsland(x, y - 1, search, grid);
                    this.MarkNewIsland(x, y + 1, search, grid);
                }
            }
        }
    }
}