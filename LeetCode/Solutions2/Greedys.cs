namespace LeetCode.Csharp.Solutions2
{
    using System.Collections.Generic;
    using System.Linq;

    class Greedys
    {
        // https://leetcode.com/problems/stamping-the-sequence/
        // 又是一个结论题，因为不知道如何证明确实是贪心可以解决的
        public int[] MovesToStamp(string stamp, string target)
        {
            char[] sa = stamp.ToCharArray();
            char[] ta = target.ToCharArray();

            List<int> steps = new List<int>();
            bool replaced = true;
            while (replaced)
            {
                replaced = false;
                for (int i = 0; i <= ta.Length - sa.Length; i++)
                {

                    int match = 0;
                    for (int j = 0; j < sa.Length; j++)
                    {
                        if (sa[j] == ta[j + i])
                        {
                            match++;
                        }
                        else if (ta[j + i] != '?')
                        {
                            match = 0;
                            break;
                        }
                    }

                    if (match > 0)
                    {
                        replaced = true;
                        steps.Add(i);
                        for (int j = 0; j < sa.Length; j++)
                        {
                            ta[j + i] = '?';
                        }
                        break;
                    }
                }
            }

            // Console.WriteLine(new string(ta));

            if (!ta.All(c => c == '?')) return new int[0];
            else return steps.ToArray().Reverse().ToArray();
        }
    }
}
