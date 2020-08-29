namespace LeetCode.Csharp.Solutions2
{
    class Randoms
    {
        private int Rand7()
        {
            return -1;
        }

        // https://leetcode.com/problems/implement-rand10-using-rand7/
        public int Rand10()
        {
            while (true)
            {
                // 等概率的 1 - 49!
                int num = (Rand7() - 1) * 7 + Rand7();
                if (num <= 40)
                {
                    return num % 10 + 1;
                }
            }
        }
    }
}
