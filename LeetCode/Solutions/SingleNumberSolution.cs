namespace LeetCode.Csharp.Solutions
{
    public class SingleNumberSolution
    {
        // https://leetcode.com/problems/single-number/
        // https://leetcode.com/problems/single-number-ii/
        public int SingleNumber(int[] nums)
        {
            int[] bitCounters = new int[32];
            int[] bitMasks = new int[32];

            int mask = 1;
            for (int i = 0; i < 32; i++)
            {
                bitMasks[i] = mask;
                mask <<= 1;
            }

            foreach (int num in nums)
            {
                for (int maskIndex = 0; maskIndex < 32; maskIndex++)
                {
                    if ((bitMasks[maskIndex] & num) != 0)
                    {
                        bitCounters[maskIndex]++;
                    }
                }
            }

            int result = 0;
            mask = 1;
            for (int i = 0; i < 32; i++)
            {
                if (bitCounters[i] % 3 == 1)
                {
                    result |= mask;
                }

                mask <<= 1;
            }

            return result;
        }
        
        // https://leetcode.com/problems/single-number-iii/
        public int[] SingleNumber3(int[] nums)
        {
            int axorb = 0;
            foreach (int num in nums)
            {
                axorb ^= num;
            }

            int mask = 1;
            while ((axorb & mask) == 0) mask <<= 1;

            int a = 0;
            int b = 0;
            foreach (int num in nums)
            {
                if ((mask & num) == 0) a ^= num;
                else b ^= num;
            }

            return new[] {a, b};
        }
    }
}